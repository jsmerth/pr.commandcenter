namespace PR.CommandCenter.Modules.Bots.Application;

public class Bot
{
  public Guid Id { get; set; }
  public string Business { get; set; } = "";
  public string City { get; set; } = "";
  public string Region { get; set; } = "";
  public string Country { get; set; } = "";
  public decimal Lat { get; set; } 
  public decimal Lon { get; set; }
  public string Status { get; set; } = "";
  public string Type { get; set; } = "";
  public int TypeId { get; set; } = 0;
  public DateTime DateModified { get; set; } 
  public DateTime DateCreated { get; set; } 
}