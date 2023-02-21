using Dapper;
using FluentValidation;
using MediatR;
using PR.CommandCenter.BuildingBlocks.Application;

namespace PR.CommandCenter.Modules.Bots.Application
{
  public class BotCreate
  {
    public class Request : IRequest<Response>
    {
      public string Business { get; set; } = "";
      public string City { get; set; } = "";
      public string Region { get; set; } = "";
      public string Country { get; set; } = "";
      public int TypeId { get; set; } = 0;
      public decimal Lat { get; set; }
      public decimal Lon { get; set; }
    }

    public class Response
    {
      public Response() { }
      public Response(Guid id) => Id = id;
      public Guid Id { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
      public Validator()
      {
        RuleFor(m => m.Business).NotEmpty().NotNull();
        RuleFor(m => m.City).NotEmpty().NotNull();
        RuleFor(m => m.Region).NotEmpty().NotNull();
        RuleFor(m => m.Country).NotEmpty().NotNull();
      }
    }

    internal class Handler : IRequestHandler<Request, Response>
    {
      private readonly ISqlConnectionFactory _sqlConnectionFactory;
      public Handler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;
      public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
      {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = "INSERT INTO [dbo].[Bots] (Id,Business,City,Region,Country,TypeId,Lat,Lon) VALUES (@Id,@Business,@City,@Region,@Country,@TypeId,@Lat,@Lon)";
        var id = Guid.NewGuid();
        await connection.ExecuteAsync(sql,
          new
          {
            Id = id,
            Business = request.Business,
            City = request.City,
            Region = request.Region,
            Country = request.Country,
            TypeId = request.TypeId,
            Lat = request.Lat,
            Lon = request.Lon,
          });

        return new Response(id);
      }
    }
  }
}