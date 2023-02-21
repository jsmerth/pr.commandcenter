using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PR.CommandCenter.Api.Functions;
using PR.CommandCenter.Modules.Bots.Application;
using PR.CommandCenter.Tests.Helper;
using Shouldly;
using Xunit;

namespace PR.CommandCenter.Modules.Bots.Tests.Integration.Api;

public class BotApiFunctionPostTests : BotApiFunctionTestsBase, IClassFixture<BotSliceFixture>
{
  private readonly BotSliceFixture _fixture;
  public BotApiFunctionPostTests(BotSliceFixture fixture) => _fixture = fixture;

  [Fact]
  [Trait("Bots", "Function")]
  public async Task POST_WhenCorrect_ShouldReturnA200Response()
  {
    //Arrange
    // var uut = new BotApiFunctions(_fixture.ServiceProvider.GetRequiredService<IMediator>());
    //
    // //Act
    // var result = (OkObjectResult)await uut.Post(CommandCreate, Request, Logger);
    //
    // //Assert
    // result.ShouldNotBeNull();
    // result.StatusCode.ShouldBe(200);
    // result.Value.ShouldBeOfType<BotCreate.Response>();
    // Logger.VerifyLogMustHaveHappened(LogLevel.Information, $"BotCreate: {result.Value}.");
  }
}