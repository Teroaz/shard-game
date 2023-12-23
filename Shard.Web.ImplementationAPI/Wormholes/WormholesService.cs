using Microsoft.Extensions.Options;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Wormholes;

public class WormholesService : IWormholesService
{
    private WormholeOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<WormholesService> _logger;

    public WormholesService(IOptions<WormholeOptions> options, HttpClient httpClient, ILogger<WormholesService> logger)
    {
        _options = options.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public KeyValuePair<string, WormholeData> GetShardData(string shard)
    {
        KeyValuePair<string, WormholeData> theShard;
        try
        {
            theShard = _options.shards.First(pair => pair.Key == shard);
        }
        catch (InvalidOperationException)
        {
            throw new Exception($"Could not find any shard matching with: {shard}");
        }

        return theShard;
    }


    private async Task PutUserInDistantShard(UserModel user, string shard)
    {
        var theShard = GetShardData(shard);
        var body = JsonContent.Create(new UserDto(user));

        var httpResponse = await _httpClient.PutAsync($"{theShard.Value.BaseUri}/users/{user.Id}", body);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(httpResponse.StatusCode.ToString());
        }
    }

    private async Task<string> PutUnitInDistantShard(UserModel user, UnitModel unit, string shard)
    {
        var theShard = GetShardData(shard);
        var body = JsonContent.Create(new UnitsDto(unit));

        var httpResponse = await _httpClient.PutAsync($"{theShard.Value.BaseUri}/users/{user.Id}/units/{unit.Id}", body);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(httpResponse.StatusCode.ToString());
        }

        return $"{theShard.Value.BaseUri}/users/{user.Id}/units/{unit.Id}";
    }

    public async Task<string> Jump(UserModel user, UnitModel unit, string shard)
    {
        await PutUserInDistantShard(user, shard);
        return await PutUnitInDistantShard(user, unit, shard);
    }
}