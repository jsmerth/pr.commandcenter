using Dapper;
using FluentValidation;
using MediatR;
using PR.CommandCenter.BuildingBlocks.Application;

namespace PR.CommandCenter.Modules.Bots.Application;

public class BotList
{
  public class Request : IRequest<Response>
  {
  }
  public class Response
  {
    public List<Bot>? Items { get; set; }
  }
  public class Validator : AbstractValidator<Request>
  {
    public Validator()
    {
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
      LEFT JOIN BotTypes bt ON b.TypeId = bt.Id";

      var items = await connection.QueryAsync<Bot>(sql);

      return new Response() { Items = items.ToList() };
    }
  }
}