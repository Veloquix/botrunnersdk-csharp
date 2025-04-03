using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Veloquix.BotRunner.SDK.Conversation;

public class Constants
{
    public const string LanguageName = "Language";

    /// <summary>
    /// If you want to manually set up to handle incoming requests on a controller,
    /// these constants are useful in setting up the token validation.
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// A recommended auth scheme name to use
        /// </summary>
        public const string Scheme = "VeloquixToken";
        /// <summary>
        /// The name of the AccountId claim provided in a BotRunner token.
        /// </summary>
        public const string AccountIdClaimName = "AccountId";
        /// <summary>
        /// The name of the ApplicationId claim provided in a BotRunner token.
        /// </summary>
        public const string ApplicationIdClaimName = "ApplicationId";
        internal static OptionsMonitor<JwtBearerOptions> JwtOptions;
    }
}
