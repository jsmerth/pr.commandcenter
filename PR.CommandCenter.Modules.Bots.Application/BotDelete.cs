using FluentValidation;
using MediatR;

namespace PR.CommandCenter.Modules.Bots.Application;

public class BotDelete
{
  public class Request : IRequest<Response>
  {
    public Guid Id { get; set; }
  }
  public class Response
  {
    public Guid Id { get; set; }
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
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
      return await Task.FromResult(new Response());
    }
  }
}