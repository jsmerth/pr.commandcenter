using System.Net;
using Dapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using PR.CommandCenter.BuildingBlocks.Application;

namespace PR.CommandCenter.Modules.Bots.Application;

public class BotGet
{
  public class Request : IRequest<Response>
  {
    public Request() { }
    public Request(Guid id) => Id = id;
    public Guid Id { get; set; }
  }

  public class Response : Bot
  {
  }
  public class Validator : AbstractValidator<Request>
  {
    public Validator()
    {
      RuleFor(m => m.Id).NotEmpty().NotNull();
    }
  }

  internal class Handler : IRequestHandler<Request, Response>
  {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    public Handler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
      var connection = _sqlConnectionFactory.GetOpenConnection();

      const string sql = @$"SELECT b.*,e.Type AS Status,bt.Name AS Type 
      FROM Bots b 
      LEFT JOIN Events e ON e.BotId = b.Id 
        AND e.Id = (
          SELECT TOP (1) Id FROM [PR.CommandCenter.Db].[dbo].[Events] WHERE BotId=b.Id ORDER BY DateCreated DESC
        )
      LEFT JOIN BotTypes bt ON b.TypeId = bt.Id
      WHERE b.Id = @Id";

      // TODO: Add Polly Retry
      var item = await connection.QueryFirstOrDefaultAsync<Bot>(sql, new { Id = request.Id });
      if (item == null) throw new WebException();

      // TODO: Fix this type converting hack
      return JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(item));
    }
  }
}