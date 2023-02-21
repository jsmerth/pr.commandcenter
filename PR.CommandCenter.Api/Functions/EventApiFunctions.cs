using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using PR.CommandCenter.Modules.Events.Application;
using System.Web.Http;
using FluentValidation;
using PR.CommandCenter.Modules.Bots.Application;

namespace PR.CommandCenter.Api.Functions
{
  public class EventApiFunctions
  {
    private readonly IMediator _mediator;

    public EventApiFunctions(IMediator mediator) => _mediator = mediator;

    [FunctionName(nameof(EventApiFunctions) + nameof(Post))]
    public async Task<IActionResult> Post(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Events")] [FromBody] EventCreate.Request request,
      HttpRequest req,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(request);
        log.LogInformation($"{nameof(EventApiFunctions)}:{obj}");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(EventApiFunctions)}:{nameof(Post)}");
        return new ExceptionResult(ex, true);
      }
    }

    [FunctionName(nameof(EventApiFunctions) + nameof(GetList))]
    public async Task<IActionResult> GetList(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Events/{id}")] HttpRequest req,
      Guid id,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(new EventList.Request(id));
        log.LogInformation($"{nameof(EventApiFunctions)}:{obj}");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(EventApiFunctions)}:{nameof(GetList)}");
        return new ExceptionResult(ex, true);
      }
    }
  }
}
