using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Veloquix.BotRunner.SDK.Authentication;
internal static class JwtAuth
{
    private static string _authority = "https://auth.veloquix.com/";
    private static string _accountId = string.Empty;
    private static string _applicationId = string.Empty;
    private static ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    public static void Setup(string accountId, string applicationId, string environmentSuffix = "")
    {
        if (!string.IsNullOrWhiteSpace(environmentSuffix))
        {
            _authority = $"https://auth.veloquix{environmentSuffix}.com/";
        }

        _accountId = accountId;
        _applicationId = applicationId;

        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{_authority}.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever());
    }
    public static async Task<bool> ValidateToken(this HttpContext ctx)
    {
        var discoveryDocument = await _configurationManager.GetConfigurationAsync();
        var signingKeys = discoveryDocument.SigningKeys;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _authority,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = signingKeys,
            ValidateLifetime = true
        };

        var (parsed, token) = ParseToken(ctx);

        if (!parsed)
        {
            return false;
        }

        try
        {
            var result = await TokenHandler.ValidateTokenAsync(token, validationParameters);

            if (!result.IsValid)
            {
                Err($"Token Validation Failed: {result.Exception.Message}");
                return false;
            }

            var isValid = true;

            if (!result.Claims.TryGetValue(TokenConstants.AccountIdClaimName, out var accClaim))
            {
                isValid = false;
                Err("No AccountId Claim");
            }
            else if (accClaim is not string claimStr ||
                     !string.Equals(claimStr, _accountId, StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = false;
                Err($"Invalid AccountId Claim - Does not match {_accountId}");
            }

            if (!result.Claims.TryGetValue(TokenConstants.ApplicationIdClaimName, out var appClaim))
            {
                isValid = false;
                Err("No ApplicationId Claim");
            }
            else if (appClaim is not string claimStr ||
                     !string.Equals(claimStr, _applicationId, StringComparison.InvariantCultureIgnoreCase))
            {
                isValid = false;
                Err($"Invalid ApplicationId Claim - Does not match {_applicationId}");
            }

            return isValid;
        }
        catch (SecurityTokenValidationException ex)
        {
            Err($"Token Validation Failed: {ex.Message}");
            // Log the reason why the token is not valid
            return false;
        }
    }

    private static (bool Parsed, string Token) ParseToken(HttpContext ctx)
    {
        if (ctx.Request.Headers.Authorization.Count == 0)
        {
            return Failure("Incoming request has no Authorization header");
        }

        var authHeader = ctx.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            return Failure("Authorization header is empty");
        }

        var split = authHeader.Split(' ');

        if (split.Length != 2 || split[0] != "Bearer")
        {
            return Failure("Authorization header is malformed");
        }

        var token = split[1];

        if (!TokenHandler.CanReadToken(token))
        {
            return Failure("JWT not recognized as such");
        }

        return (true, token);

        (bool Parsed, string Token) Failure(string msg)
        {
            Err(msg);
            return (false, null);
        }
    }

    private static void Err(string err)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Unable to Execute BotRunner Webhook: Auth Failure. {err}");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
