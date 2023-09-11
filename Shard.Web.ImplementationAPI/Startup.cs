using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems;

namespace Shard.Web.ImplementationAPI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        MapGeneratorOptions options = new MapGeneratorOptions
        {
            Seed = Configuration.GetSection("MapGenerator").GetSection("Options").GetSection("Seed").Value,
        };
        
        MapGenerator mapGenerator = new MapGenerator(options);
        // Add all the services here that are needed by the controllers and should be considered as singletons
        services.AddSingleton<SystemsService>();
        services.AddSingleton(mapGenerator);
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}