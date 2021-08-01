using DnsClient.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MBADCases.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, (Microsoft.Extensions.Logging.ILoggerFactory) logger, encoder, clock)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
            }
            string authHeader = Request.Headers["Authorization"];
            AuthenticationHeaderValue authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
            try
            {


                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var credentials = Encoding.UTF8
                                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                        .Split(':', 2);
                    if (credentials.Length == 2)
                    {
                        //if (credentials[0] == "6bc8cee0-a03e-430b-9711-420ab0d6a596" & credentials[1] == "sQLcN1LHLuu7MfK5On92lUEWajEBpEDc")
                        if (credentials[0] == "demo" & credentials[1] == "demo")
                        {
                            //_appSettings.Value.Clientid = credentials[0];

                            var claims = new[]
                            {
                                        new Claim(ClaimTypes.NameIdentifier, "6bc8cee0-a03e-430b-9711-420ab0d6a596")

                                    };
                            var identity = new ClaimsIdentity(claims, Scheme.Name);
                            var principle = new ClaimsPrincipal(identity);
                            var ticket = new AuthenticationTicket(principle, Scheme.Name);

                            return Task.FromResult(AuthenticateResult.Success(ticket));

                        }

                    }

                }
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));

            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
            }
        }
    }
}
 
