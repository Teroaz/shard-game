using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Handler;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        UserModel? user = null;

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Context.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var pseudo = credentials[0];
            var password = credentials[1];

            user = ValidateCredentials(pseudo, password);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        if (user == null)
            return AuthenticateResult.Fail("Invalid Username or Password");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Pseudo),
            new Claim(ClaimTypes.Role, Roles.Admin)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private UserModel? ValidateCredentials(string pseudo, string password)
    {
        return pseudo == "admin" && password == "password" ? new UserModel(pseudo, password) : null;
    }
}