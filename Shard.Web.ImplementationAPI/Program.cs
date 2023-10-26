using System.Text.Json;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();


namespace Shard.Web.ImplementationAPI
{
    public partial class Program
    {
    }
}