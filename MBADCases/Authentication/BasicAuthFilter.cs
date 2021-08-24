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
    public class BasicAuthFilter : Attribute,  IAuthorizationFilter
    {
        private readonly string _realm = string.Empty;
    

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
            MongoClient _client;
            IMongoDatabase MBADDatabase;
            IMongoCollection<Message> _messagemaster;
            Message omess = new Message();
            string xrapidhost="";
            _client = new MongoClient("mongodb://yardilloadmin:1pkGpqdqHV42AvOD@cluster0-shard-00-00.tj6lt.mongodb.net:27017,cluster0-shard-00-01.tj6lt.mongodb.net:27017,cluster0-shard-00-02.tj6lt.mongodb.net:27017/yardillo_dev?ssl=true&replicaSet=atlas-d5jcxa-shard-0&authSource=admin&retryWrites=true&w=majority");
            MBADDatabase = _client.GetDatabase("YARDILLO");
            _messagemaster = MBADDatabase.GetCollection<Message>("Logins");

            try
            {
                bool allpass = true ;
                
                string srapidsecretkey = context.HttpContext.Request.Headers["X-RapidAPI-Proxy-Secret"];
                string rapiduserid = context.HttpContext.Request.Headers["X-RapidAPI-User"];
                var ssubs = context.HttpContext.Request.Headers["X-RapidAPI-Subscription"];
                string sauthsrc = context.HttpContext.Request.Headers["Y-Auth-Src"]; 
                string srapidapikey = context.HttpContext.Request.Headers["x-rapidapi-key"];

                omess.Callertype = "Headers";
                omess.Messagecode = "yardillo";
                string sheaders=   Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers);
                omess.Headerrequest = sheaders;
                omess.Userid = rapiduserid;
                omess.YAuthSource = sauthsrc;

                if (srapidsecretkey != "1f863a60-f3b6-11eb-bc3e-c3f329db9ee7" && srapidsecretkey != "6acc1280-fde1-11eb-b480-3f057f12dc26" && srapidsecretkey != "ade9f2f0-fe3e-11eb-8e8b-29cf15887162")
                { allpass = false; omess.Messageype = "Unauthorized"; omess.Messagecode =  "00001"; }
               
                if (allpass) 
                { 
                    if (sauthsrc != "yardillo" && sauthsrc != "WixAdapter")
                    { allpass = false; omess.Messageype = "Unauthorized"; omess.Messagecode = "00002"; }
                     
                }

                if (allpass)
                {
                    xrapidhost = context.HttpContext.Request.Headers["x-rapidapi-host"];
                    omess.Callerid = xrapidhost;
                    if (xrapidhost != "mbad.p.rapidapi.com" && xrapidhost != "yardillo.p.rapidapi.com" && xrapidhost != "mongodb-wix.p.rapidapi.com")
                    { allpass = false; omess.Messageype = "Unauthorized"; omess.Messagecode = "00003"; }

                }
                IMongoCollection<TenantUser> _tenantusercoll=null;
                if (allpass)
                {
                    _tenantusercoll = MBADDatabase.GetCollection<TenantUser>("TenantUsers");
                }

                if (allpass)
                {
                   
                    if (rapiduserid == null || rapiduserid == "")
                    {
                        allpass = false; omess.Messageype = "Unauthorized"; omess.Messagecode = "00004";
                    }
                    else
                    {
                        context.HttpContext.Session.SetString("mbaduserid", rapiduserid);
                    }

                    //try to find users tenants
                   
                    if (allpass && _tenantusercoll!=null)
                    {
                        string ytenantname = context.HttpContext.Request.Headers["y-auth-tenantname"];
                        List<TenantUser> ou;
                        //get the user for the tenant created for a source
                        if ((ou = _tenantusercoll.Find<TenantUser>(book => book.Userid.ToUpper() == rapiduserid.ToUpper() && book.YAuthSource == sauthsrc).ToList() ) != null)
                        {
                            if (ou.Count > 1)
                            {
                                //multiple present look for y-auth-tenantname
                                //continue
                                if (ytenantname != null && ytenantname != "")
                                {
                                    TenantUser o = ou.Where(t => t.Tenantname.ToUpper() == ytenantname.ToUpper()).FirstOrDefault();
                                    if (o != null)
                                    {
                                        context.HttpContext.Session.SetString("mbadtanent", o.Tenantname);
                                        omess.Tenantid = o.Tenantname;
                                        return;
                                    }
                                    else
                                    {
                                        allpass = false;
                                        omess.Messageype = "Unauthorized";
                                        omess.Messagecode = "00005";

                                    }
                                }
                                {
                                    //pick the top one
                                    context.HttpContext.Session.SetString("mbadtanent", ou[0].Tenantname);
                                    omess.Tenantid = ou[0].Tenantname;
                                    return;
                                }
                             
                            }
                            if (ou.Count ==1 )
                            {
                                //only one present then default to that
                                context.HttpContext.Session.SetString("mbadtanent", ou[0].Tenantname);
                                omess.Tenantid = ou[0].Tenantname;
                                return;
                            }
                            else
                            {
                                //continue
                            }
                        }
                        
                    }
                
                    //so far goood, check if tenant name is passed
                    if (allpass)
                    {
                        string ytenantname = context.HttpContext.Request.Headers["y-auth-tenantname"];
                        //get the tenant from user name
                        if (ytenantname == null || ytenantname == "")
                        {
                            //generate one for this user
                            IMongoCollection<Tenant> _tenantcoll;
                            _tenantcoll = MBADDatabase.GetCollection<Tenant>("Tenants");
                            Tenant oten = new Tenant();
                            oten.Tenantname = "TENANT" + "_" + helperservice.RandomString(7, false); ;
                            oten.Tenantdesc = rapiduserid;
                            oten.Createdate = DateTime.UtcNow.ToString();
                            oten.Createuser = rapiduserid;
                            oten.YAuthSource = sauthsrc;
                            oten.Rapidsubscription = ssubs.ToString();
                            oten.Rapidhost = xrapidhost;
                            //set the rapid key for the first user who creates the tenant. Use this key for all calls
                            oten.Rapidapikey = srapidapikey;
                            string snewtenname = oten.Tenantname.ToUpper();
                            //tenant names are uniqe in entire yardillo
                            if (_tenantcoll.Find<Tenant>(book => book.Tenantname.ToUpper() == snewtenname.ToUpper()).FirstOrDefault() != null)
                            {
                                snewtenname = "TENANT" + "_" + helperservice.RandomString(7, false);
                                while (_tenantcoll.Find<Tenant>(book => book.Tenantname.ToUpper() == snewtenname.ToUpper()).FirstOrDefault() != null)
                                {
                                    //name must be unique assing a random string
                                    snewtenname = "TENANT" + "_" + helperservice.RandomString(7, false);
                                }
                                oten.Tenantname = snewtenname;
                            };

                            _tenantcoll.InsertOne(oten);
                            if (oten._id != "")
                            {
                                context.HttpContext.Session.SetString("mbadtanent", oten.Tenantname.ToUpper());
                                //and register as tenantuser
                                TenantUser ousr = new TenantUser();
                                ousr.Userid = rapiduserid;
                                ousr.Tenantname = oten.Tenantname;
                                ousr.Createdate = DateTime.UtcNow.ToString();
                                ousr.Createuserid = rapiduserid;
                                ousr.YAuthSource = sauthsrc;
                                ousr.RapidAPIkey = srapidapikey;
                                _tenantusercoll.InsertOne(ousr);
                                if (ousr._id != "")
                                {
                                    context.HttpContext.Session.SetString("mbaduserid", ousr.Userid);
                                    return;
                                }
                            }
                            else
                            {
                                omess.MessageDesc = "Unabel to create tenant for userid = " + rapiduserid;
                                _messagemaster.InsertOneAsync(omess);
                                allpass = false; omess.Messageype = "Unauthorized"; omess.Messagecode =   "00006";
                            }                         
                        }
                        else
                        {
                            allpass=false; omess.Messageype = "Unauthorized"; omess.Messagecode = "00007";
                        }
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
            catch (FormatException e)
            {
                omess.MessageDesc =  "Unabel to validate user" + e.ToString();
               // _messagemaster.InsertOneAsync(omess);
                ReturnUnauthorizedResult(context);
            }
            finally
            {
                _messagemaster.InsertOneAsync(omess);
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
