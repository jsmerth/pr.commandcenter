using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace PR.CommandCenter.Db.Transitions
{
  public class Program
  {
    public static IConfiguration? Configuration { get; private set; }

    public static void Main(string[] args)
    {
      var builder = new ConfigurationBuilder()
        .AddJsonFile($"local.settings.json", false);

      Configuration = builder.Build();
      
      // TODO: Add Mediatr Send for adding new events / bots
      while (true)
      {
        Console.WriteLine("------------------------ PR Command Center Console ------------------------");
        Console.WriteLine($"0. Drop Tables");
        Console.WriteLine($"1. Empty Tables");
        Console.WriteLine($"2. Seed the DB with mock data");
        Console.WriteLine("Ctrl+C to close");
        Console.Write("Enter your choice: ");
        var input = Console.ReadLine();

        try
        {
          switch (input)
          {
            case "0":
              DropTables();
              break;
            case "1":
              EmptyTables();
              break;
            case "2":
              RunSeed(100);
              break;
          }
        }
        catch (Exception ex)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(ex.Message);
          Console.ResetColor();
        }

        Console.WriteLine("");
      }
    }

    private static void DropTables()
    {
      var conString = Configuration?.GetConnectionString("Default");
      var connection = new SqlConnection(conString);
      connection.Open();

      //- TODO check db for table existence before dropping
      var sql = @"DROP TABLE [dbo].[Bots]";
      connection.Execute(sql);
      sql = @"DROP TABLE [dbo].[Events]";
      connection.Execute(sql);
      sql = @"DROP TABLE [dbo].[BotTypes]";
      connection.Execute(sql);
    }

    public static void EmptyTables()
    {
      var conString = Configuration?.GetConnectionString("Default");
      var connection = new SqlConnection(conString);
      connection.Open();
      
      var sql = @"DELETE FROM [dbo].[Bots]";
      connection.Execute(sql);
      sql = @"DELETE FROM [dbo].[Events]";
      connection.Execute(sql);
      sql = @"DELETE FROM [dbo].[BotTypes]";
      connection.Execute(sql);
      sql = @"DBCC CHECKIDENT (BotTypes, RESEED, 0)";
      connection.Execute(sql);
    }

    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 1000")]
    private static void RunSeed(int bots)
    {
      var locations = LoadLocations();
      var businessNames = LoadBusinessNames();
      var botIds = new List<Guid>();
      
      var conString = Configuration?.GetConnectionString("Default");
      var connection = new SqlConnection(conString);
      connection.Open();

      var botTypes = new List<string> { "KetyBot", "BellaBot", "FlashBot", "Puductor 2", "Hola Bot", "CC1 Bot" };
      var eventStatuses = new List<string> { "ERROR", "OFFLINE", "ONLINE", "OK" };

      botTypes.ForEach(bt =>
      {
        var sql1 = "INSERT INTO [dbo].[BotTypes] (Name) VALUES (@Name)";
        connection.Execute(sql1, new { Name = bt });
      });

      for (int i = 0; i < bots; i++)
      {
        var location = locations.OrderBy(a => Guid.NewGuid()).First();
        var businessName = businessNames.OrderBy(a => Guid.NewGuid()).First();
        var id = Guid.NewGuid();
        botIds.Add(id);
        var sql2 = "INSERT INTO [dbo].[Bots] (Id,TypeId,Business,City,Region,Country,Lat,Lon,DateCreated) VALUES (@Id,@TypeId,@Business,@City,@Region,@Country,@Lat,@Lon,@DateCreated)";
        connection.Execute(sql2,
          new
          {
            Id = id,
            TypeId = new Random().Next(1,botTypes.Count + 1),
            Business = businessName,
            City = location.City,
            Region = location.Region,
            Country = location.Country,
            Lat = location.Lat,
            Lon = location.Lon,
            DateCreated = DateTime.Now.AddDays(-i)
          });

        var events = new Random(i).Next(1,10);
        for (int j = 0; j < events; j++)
        {
          var eventStatus = eventStatuses.OrderBy(a => Guid.NewGuid()).First();

          var sql3 = "INSERT INTO [dbo].[Events] (Id,BotId,Type,Content,DateCreated) VALUES (@Id,@BotId,@Type,@Content,@DateCreated)";
          connection.Execute(sql3,
            new
            {
              Id = Guid.NewGuid(),
              BotId = id,
              Type = eventStatus,
              Content = $"{eventStatus}: details would go here",
              DateCreated = DateTime.Now.AddMinutes(-j)
            });
        }
      }
    }

    private static List<string> LoadBusinessNames()
    {
      var names = new List<string>();
      var path = Path.Combine(ToApplicationPath(), "Assets\\business-names.csv");
      var lines = System.IO.File.ReadAllLines(path);

      return lines.ToList();
    }
    private static List<Location> LoadLocations()
    {
      // COLUMNS "city","city_ascii","lat","lng","country","iso2","iso3","admin_name","capital","population","id"
      var locations = new List<Location>();
      var path = Path.Combine(ToApplicationPath(), "Assets\\world-cities.csv");
      var lines = System.IO.File.ReadAllLines(path);
      foreach (var line in lines)
      {
        var columns = line.Split(',');
        var l = new Location(columns[1], columns[7], columns[4], columns[2], columns[3]);
        locations.Add(l);
      }

      return locations;
    }
    private static string ToApplicationPath()
    {
      var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
      var appRoot = appPathMatcher.Match(exePath ?? "").Value;
      return appRoot;
    }
  }
  public class Location {
    public Location(string? city, string? region, string? country, string? lat, string? lon)
    {
      City = city?.Replace("\"","");
      Region = region?.Replace("\"", "");
      Country = country?.Replace("\"", "");
      Lat = lat?.Replace("\"", "");
      Lon = lon?.Replace("\"", "");
    }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? Lat { get; set; }
    public string? Lon { get; set; }
  }

}