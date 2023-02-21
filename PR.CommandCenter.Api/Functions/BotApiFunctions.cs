using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PR.CommandCenter.Modules.Bots.Application;

namespace PR.CommandCenter.Api.Functions
{
  public class BotApiFunctions
  {
    private readonly IMediator _mediator;

    public BotApiFunctions(IMediator mediator) => _mediator = mediator;

    [FunctionName(nameof(BotApiFunctions) + nameof(Post))]
    public async Task<IActionResult> Post(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Bots")] [FromBody] BotCreate.Request request,
      HttpRequest req,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(request);
        log.LogInformation($"{nameof(BotApiFunctions)}:{obj}");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(BotApiFunctions)}:{nameof(Post)}");
        return new ExceptionResult(ex, true);
      }
    }

    [FunctionName(nameof(BotApiFunctions) + nameof(Put))]
    public async Task<IActionResult> Put(
      [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Bots")][FromBody] BotModify.Request request,
      HttpRequest req,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(request);
        log.LogInformation($"BotModify: {obj.Id}.");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(BotApiFunctions)}:{nameof(Put)}");
        return new ExceptionResult(ex, true);
      }
    }

    [FunctionName(nameof(BotApiFunctions) + nameof(Get))]
    public async Task<IActionResult> Get(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Bots/{id}")] HttpRequest req,
      Guid id,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(new BotGet.Request(id));
        log.LogInformation($"{nameof(BotApiFunctions)}:{obj}");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (WebException wEx)
      {
        return new BadRequestObjectResult(new { response = "no bot with that id exists" });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(BotApiFunctions)}:{nameof(Get)}");
        return new ExceptionResult(ex, true);
      }
    }

    [FunctionName(nameof(BotApiFunctions) + nameof(GetList))]
    public async Task<IActionResult> GetList(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Bots")] HttpRequest req,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(new BotList.Request());
        log.LogInformation($"{nameof(BotApiFunctions)}:{obj}");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(BotApiFunctions)}:{nameof(GetList)}");
        return new ExceptionResult(ex, true);
      }
    }

    [FunctionName(nameof(BotApiFunctions) + nameof(Delete))]
    public async Task<IActionResult> Delete(
      [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Bots")][FromBody] BotDelete.Request request,
      HttpRequest req,
      ILogger log
    )
    {
      try
      {
        var obj = await _mediator.Send(request);
        log.LogInformation($"BotDeletedId: {obj.Id}.");
        return new OkObjectResult(obj);
      }
      catch (ValidationException vEx)
      {
        return new BadRequestObjectResult(new { vEx.Errors });
      }
      catch (Exception ex)
      {
        log.LogCritical(ex, $"{nameof(BotApiFunctions)}:{nameof(Delete)}");
        return new ExceptionResult(ex, true);
      }
    }
  }
}