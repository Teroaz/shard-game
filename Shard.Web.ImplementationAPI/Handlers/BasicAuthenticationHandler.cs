using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Claim[]? claims;
        ClaimsIdentity? identity;
        ClaimsPrincipal? principal;
        AuthenticationTicket? ticket;
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            claims = new[] { new Claim(ClaimTypes.Name, Roles.Anonymous) };
            identity = new ClaimsIdentity(claims, Scheme.Name);
            principal = new ClaimsPrincipal(identity);
            ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
        var authorizationContent = Encoding.UTF8.GetString(credentialBytes);

        if (string.IsNullOrEmpty(authorizationContent)) return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        // Getting the admin authorization
        if (authorizationContent == "admin:password")
        {
            var user = new UserModel("admin", "password", DateTime.Now);
            claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Pseudo),
                new Claim(ClaimTypes.Role, Roles.Admin)
            };
        }
        else
        {
            // Try to get the shard authorization
            var shardAuthorization = authorizationContent.Split(':');
            if (shardAuthorization.Length == 2 && shardAuthorization[0].StartsWith("shard-"))
            {
                var shardName = shardAuthorization[0][6..];
                // var shardSharedPassword = shardAuthorization[1];

                claims = new[]
                {
                    new Claim(ClaimTypes.Name, shardName),
                    new Claim(ClaimTypes.Role, Roles.Shard)
                };
            }
            else
            {
                claims = new[] { new Claim(ClaimTypes.Name, Roles.User) };
            }
        }

        identity = new ClaimsIdentity(claims, Scheme.Name);
        principal = new ClaimsPrincipal(identity);
        ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}