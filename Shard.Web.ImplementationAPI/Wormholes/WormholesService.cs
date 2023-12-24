using Microsoft.Extensions.Options;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Wormholes;

public class WormholesService : IWormholesService
{
    private readonly WormholeOptions _wormholeOptions;
    private readonly HttpClient _httpClient;

    public WormholesService(IOptions<WormholeOptions> options, HttpClient httpClient, ILogger<WormholesService> logger)
    {
        _wormholeOptions = options.Value;
        _httpClient = httpClient;
    }

    public WormholeData? GetWormholeByShardName(string shard)
    {
        return _wormholeOptions.WormholeDatas.FirstOrDefault(pair => pair.ShardName == shard);
    }

    private async Task SendUserInShard(UserModel user, string shard)
    {
        var wormhole = GetWormholeByShardName(shard);
        if (wormhole == null) throw new Exception("Shard not found");

        var body = JsonContent.Create(new UserDto(user));

        var httpResponse = await _httpClient.PutAsync($"{wormhole.BaseUri}/users/{user.Id}", body);
        if (!httpResponse.IsSuccessStatusCode) throw new Exception(httpResponse.StatusCode.ToString());
    }

    private async Task<string> SendUnitInShard(UnitModel unit, string shard)
    {
        var wormhole = GetWormholeByShardName(shard);
        if (wormhole == null) throw new Exception("Shard not found");

        var body = JsonContent.Create(new UnitsDto(unit));

        var httpResponse = await _httpClient.PutAsync($"{wormhole.BaseUri}/users/{unit.User.Id}/units/{unit.Id}", body);
        if (!httpResponse.IsSuccessStatusCode) throw new Exception(httpResponse.StatusCode.ToString());

        return $"{wormhole.BaseUri}/users/{unit.User.Id}/units/{unit.Id}";
    }

    public async Task<string> Jump(UserModel user, UnitModel unit, string shard)
    {
        await SendUserInShard(user, shard);
        return await SendUnitInShard(unit, shard);
    }
}