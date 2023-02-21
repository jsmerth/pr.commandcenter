using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PR.CommandCenter.Api;
using PR.CommandCenter.BuildingBlocks.Application;
using PR.CommandCenter.Modules.Bots.Application;
using PR.CommandCenter.BuildingBlocks.Infrastructure;
using PR.CommandCenter.Modules.Events.Application;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace PR.CommandCenter.Api;

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    // var config = new ConfigurationBuilder()
    //   .AddJsonFile("local.settings.json", true)
    //   .AddEnvironmentVariables();
    // var configuration = config.Build();
    //var connString = configuration.GetConnectionString("Default");

    // TODO: Fix config builder issue to import the connection string
    var connString =
      "Data Source=(localdb)\\ProjectModels;Initial Catalog=PR.CommandCenter.Db;Integrated Security=True;Pooling=False;Connect Timeout=30";
    builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(cfg =>
      new SqlConnectionFactory(connString));

    AssemblyScanner
      .FindValidatorsInAssembly(typeof(BotCreate).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));
    AssemblyScanner
      .FindValidatorsInAssembly(typeof(BotDelete).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));
    AssemblyScanner
      .FindValidatorsInAssembly(typeof(BotGet).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));
    AssemblyScanner
      .FindValidatorsInAssembly(typeof(BotList).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));
    AssemblyScanner
      .FindValidatorsInAssembly(typeof(BotModify).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));

    AssemblyScanner
      .FindValidatorsInAssembly(typeof(EventCreate).Assembly)
      .ForEach(x => builder.Services.AddScoped(x.InterfaceType, x.ValidatorType));

    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblyContaining<BotCreate>();
      cfg.RegisterServicesFromAssemblyContaining<BotDelete>();
      cfg.RegisterServicesFromAssemblyContaining<BotGet>();
      cfg.RegisterServicesFromAssemblyContaining<BotList>();
      cfg.RegisterServicesFromAssemblyContaining<BotModify>();

      cfg.RegisterServicesFromAssemblyContaining<EventCreate>();
    });
    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
  }
}