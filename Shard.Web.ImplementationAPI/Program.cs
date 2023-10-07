using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ISystemsRepository, SystemsRepository>();
builder.Services.AddSingleton<ISystemsService, SystemsService>();
builder.Services.AddSingleton<MapGenerator>(_ =>
{
    
    var options = new MapGeneratorOptions
    {
        Seed = configuration["MapGenerator:Options:Seed"],
    };

    return new MapGenerator(options);
});

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