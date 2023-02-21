using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using Xunit;

namespace PR.CommandCenter.Tests.Helper
{
  public abstract class SliceFixtureBase : IAsyncLifetime
  {
    protected Checkpoint _checkpoint;
    protected IConfiguration _config;
    protected IServiceScopeFactory _scopeFactory;
    protected IHost _host;

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
      using var scope = _scopeFactory.CreateScope();

      try
      {
        await action(scope.ServiceProvider);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
      using var scope = _scopeFactory.CreateScope();

      try
      {
        var result = await action(scope.ServiceProvider);

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
      return ExecuteScopeAsync(sp =>
      {
        var mediator = sp.GetRequiredService<IMediator>();

        return mediator.Send(request);
      });
    }

    public Task SendAsync(IRequest request)
    {
      return ExecuteScopeAsync(sp =>
      {
        var mediator = sp.GetRequiredService<IMediator>();

        return mediator.Send(request);
      });
    }

    public Task InitializeAsync()
    {
      return Task.Delay(10);
    }

    public Task DisposeAsync()
    {
      _host?.Dispose();

      return Task.CompletedTask;
    }

  }
}