using Microsoft.AspNetCore.Http;
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
using MBADCases.Services;
using MBADCases.Models;
using MongoDB.Driver;

namespace MBADCases.Authentication
{
    public class BasicAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _realm = string.Empty;
        private readonly TenantService _tenantservice;
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
                bool allpass = true ;
                string srapidsecretkey = context.HttpContext.Request.Headers["X-RapidAPI-Proxy-Secret"];
                if (srapidsecretkey != "1f863a60-f3b6-11eb-bc3e-c3f329db9ee7")
                { allpass = false; }
                
                if(allpass) 
                { 
                    string xrapidhost = context.HttpContext.Request.Headers["x-rapidapi-host"];
                    if (xrapidhost != "mbad.p.rapidapi.com")
                    { allpass = false; }
                }
                
                if(allpass)
                {
                    string srapidapikey = context.HttpContext.Request.Headers["x-rapidapi-key"];
                    if (srapidapikey == null || srapidapikey == "") { 
                        allpass = false; 
                    }
                    else
                    {
                        context.HttpContext.Session.SetString("mbadtanent", srapidapikey);
                    }
                    
                }
                               

                if (allpass) { return; }


                if (allpass) 
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
                                context.HttpContext.Session.SetString("mbadtanent", credentials[0]);

                                return;
                                //}
                            }
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
