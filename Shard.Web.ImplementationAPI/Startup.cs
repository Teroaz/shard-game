using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;

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
        services.AddSingleton<ISystemsRepository, SystemsRepository>();
        services.AddSingleton<ISystemsService, SystemsService>();
        services.AddSingleton<IUnitsService, UnitsService>();
        services.AddSingleton<IUserService, UsersService>();
        services.AddSingleton<IUserRepository, UsersRepository>();
        services.AddSingleton<MapGenerator>((_) =>
        {
            var options = new MapGeneratorOptions
            {
                Seed = Configuration.GetSection("MapGenerator:Options:Seed").Value,
            };
            
            return new MapGenerator(options);
        });
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