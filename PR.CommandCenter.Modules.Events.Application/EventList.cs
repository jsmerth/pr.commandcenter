using Dapper;
using FluentValidation;
using MediatR;
using PR.CommandCenter.BuildingBlocks.Application;

namespace PR.CommandCenter.Modules.Events.Application;

public class EventList
{
  public class Request : IRequest<Response>
  {
    public Request() { }
    public Request(Guid id) => BotId = id;
    public Guid BotId { get; set; }
  }

  public class Response
  {
    public List<Event>? Items { get; set; }
  }

  public class Validator : AbstractValidator<Request>
  {
    public Validator()
    {
      RuleFor(m => m.BotId).NotEmpty().NotNull();
    }
  }
  internal class Handler : IRequestHandler<Request, Response>
  {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    public Handler(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
      var connection = _sqlConnectionFactory.GetOpenConnection();

      const string sql = "SELECT * FROM Events WHERE BotId=@Id";

      var items = await connection.QueryAsync<Event>(sql, new { Id = request.BotId});

      return new Response() { Items = items.ToList() };
    }
  }
}