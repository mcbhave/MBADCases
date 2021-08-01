using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 

namespace MBADCases.Authentication
{
    public class BasicAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _realm;

        public BasicAuthFilter()
        {
            //_realm = realm;
            //if (string.IsNullOrWhiteSpace(_realm))
            //{
            //    throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            //}
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                                            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                            .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            //if (IsAuthorized(context, credentials[0], credentials[1]))
                            //{
                            //    return;

                            return;
                            //}
                        }
                    }
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        //public bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
        //{
        //    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        //    return true;// userService.IsValidUser(username, password);
        //}

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }
    }
    //public class BasicAuthFilter : Attribute, IAuthenticationFilter
    //{
    //    internal const string AuthTokenParmName = "Authorization";
    //    public BasicAuthFilter() : base()
    //    {

    //    }

    //    public bool AllowMultiple => throw new NotImplementedException();

    //    public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
    //    {
    //        string encryptedToken = default(string);
    //        string token = default(string); ;
    //        IEnumerable<string> headerValues;
    //        HttpRequestMessage request = context.ActionContext.Request;
    //        if (request.Headers.TryGetValues(AuthTokenParmName, out headerValues))
    //        {
    //            encryptedToken = headerValues.First();
    //            if (string.IsNullOrWhiteSpace(encryptedToken))
    //            {

    //                throw new Exception("Authorization header missing");
    //            }
    //            token = Base64Decode(encryptedToken);
    //            // string authHeader = request.Headers.TryGetValues(AuthTokenParmName, out headerValues);
    //            //AuthenticationHeaderValue authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
    //            if (string.IsNullOrWhiteSpace(token))
    //            {

    //                throw new Exception("Authorization header wrong");
    //            }
    //            else
    //            {
    //                if (token[0] == '{')
    //                {


    //                    AuthenticationContext tokenDetails = (AuthenticationContext)Newtonsoft.Json.JsonConvert.DeserializeObject(token);
    //                    if (string.IsNullOrWhiteSpace(tokenDetails.ToString()))
    //                    {

    //                    }
    //                }
    //            }
    //        }

    //        throw new NotImplementedException();
    //    }
    //    private string Base64Decode(string tok)
    //    {
    //        byte[] data = Convert.FromBase64String(tok);
    //        return Encoding.UTF8.GetString(data);
    //    }
    //    public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
