using FluentValidation;
using MediatR;

namespace PR.CommandCenter.Modules.Events.Application
{
  public class EventCreate
  {
    public class Request : IRequest<Response>
    {
      public Guid BotId { get; set; }
      public string Name { get; set; } = "";
    }

    public class Response
    {
      public Guid Id { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
      public Validator()
      {
        RuleFor(m => m.BotId).NotEmpty().NotNull();
        RuleFor(m => m.Name).NotEmpty().NotNull();
      }
    }
    internal class Handler : IRequestHandler<Request, Response>
    {
      public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
      {
        var response = new Response
        {
          Id = Guid.NewGuid()
        };

        return await Task.FromResult(response);
      }
    }
  }
}