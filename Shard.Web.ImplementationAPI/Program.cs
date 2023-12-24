using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Handlers;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.Fighting;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;
using Shard.Web.ImplementationAPI.Wormholes;
using SystemClock = Shard.Shared.Core.SystemClock;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAuthentication("Basic").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton<ISystemsRepository, SystemsRepository>();
builder.Services.AddSingleton<ISystemsService, SystemsService>();

builder.Services.AddSingleton<IUnitsRepository, UnitsRepository>();
builder.Services.AddSingleton<IUnitsService, UnitsService>();

builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUsersService, UsersService>();

builder.Services.AddSingleton<IBuildingsRepository, BuildingsRepository>();
builder.Services.AddSingleton<IBuildingsService, BuildingsService>();

builder.Services.AddSingleton<ICommon, Common>();
builder.Services.AddSingleton<IClock, SystemClock>();

builder.Services.Configure<MapGeneratorOptions>(options => options.Seed = configuration["MapGenerator:Options:Seed"]);
builder.Services.AddSingleton<MapGenerator>();

builder.Services.AddHostedService<UnitsArenaHostedService>();

builder.Services.Configure<WormholeOptions>(
    options => options.WormholeDatas = configuration.GetSection("Wormholes").GetChildren().Select(
        section => new WormholeData(
            section.Key,
            section.GetValue<string>("baseUri") ?? throw new InvalidOperationException(),
            section.GetValue<string>("system") ?? throw new InvalidOperationException(),
            section.GetValue<string>("user") ?? throw new InvalidOperationException(),
            section.GetValue<string>("sharedPassword") ?? throw new InvalidOperationException()
        )
    ).ToList()
);
builder.Services.AddHttpClient<IWormholesService, WormholesService>();
builder.Services.AddSingleton<IWormholesService, WormholesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();


namespace Shard.Web.ImplementationAPI
{
    public partial class Program
    {
    }
}