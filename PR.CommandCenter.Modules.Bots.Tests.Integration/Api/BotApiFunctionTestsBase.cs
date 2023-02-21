using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using PR.CommandCenter.Modules.Bots.Application;

namespace PR.CommandCenter.Modules.Bots.Tests.Integration.Api
{
  public class BotApiFunctionTestsBase
  {
    public ILogger Logger;
    public BotCreate.Request CommandCreate;
    public BotModify.Request CommandModify;
    public BotDelete.Request CommandDelete;
    public DefaultHttpRequest Request;

    public BotApiFunctionTestsBase()
    {
      Logger = A.Fake<ILogger>();
      CommandCreate = new BotCreate.Request()
      {
        Business = "name",
        City = "city",
        Region = "region",
        Country = "country"
      };
      CommandModify = new BotModify.Request()
      {
        Id = Guid.NewGuid()
      };
      CommandDelete = new BotDelete.Request()
      {
        Id = Guid.NewGuid()
      };
      Request = new DefaultHttpRequest(new DefaultHttpContext());
    }
  }
}