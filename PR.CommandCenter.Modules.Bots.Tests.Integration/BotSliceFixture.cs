using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PR.CommandCenter.Api;
using PR.CommandCenter.BuildingBlocks.Application;
using PR.CommandCenter.Modules.Bots.Application;
using PR.CommandCenter.Tests.Helper;
using Respawn;

namespace PR.CommandCenter.Modules.Bots.Tests.Integration;

public class BotSliceFixture : SliceFixtureBase
{
  public IServiceProvider ServiceProvider { get; }

  public BotSliceFixture()
  {
  }
}