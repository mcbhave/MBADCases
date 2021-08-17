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
    public class BasicAuthWix : Attribute, IAuthorizationFilter
    {
        private readonly string _realm = string.Empty;
        public BasicAuthWix(string realm)
        {
            _realm = realm;
            if (string.IsNullOrWhiteSpace(_realm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            return;
            MongoClient _client;
            IMongoDatabase MBADDatabase;
            IMongoCollection<Message> _messagemaster;
            Message omess = new Message();
            _client = new MongoClient("mongodb://yardilloadmin:1pkGpqdqHV42AvOD@cluster0-shard-00-00.tj6lt.mongodb.net:27017,cluster0-shard-00-01.tj6lt.mongodb.net:27017,cluster0-shard-00-02.tj6lt.mongodb.net:27017/yardillo_dev?ssl=true&replicaSet=atlas-d5jcxa-shard-0&authSource=admin&retryWrites=true&w=majority");
            MBADDatabase = _client.GetDatabase("YARDILLO");
            _messagemaster = MBADDatabase.GetCollection<Message>("WIXlogins");

            try
            {
               
                bool allpass = true;
                //string srapidsecretkey = context.HttpContext.Request.Headers["X-RapidAPI-Proxy-Secret"];
                //if (srapidsecretkey!="1f863a60-f3b6-11eb-bc3e-c3f329db9ee7"|| srapidsecretkey!="5f188e6b3emsh22dc968fbdea35fp1d0668jsn84272bcf0086")
                //{ allpass = false; }
                string srapidsecretkey = context.HttpContext.Request.Headers["X-RapidAPI-Proxy-Secret"];

                omess.Callertype = "Headers";
                omess.Messagecode = "wix";
                string sheaders = Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers);
                omess.MessageDesc = sheaders;
                _messagemaster.InsertOneAsync(omess);
              

                if (srapidsecretkey != "1f863a60-f3b6-11eb-bc3e-c3f329db9ee7" && srapidsecretkey != "6acc1280-fde1-11eb-b480-3f057f12dc26")
                { allpass = false; }

                if (allpass)
                {
                    string xrapidhost = context.HttpContext.Request.Headers["x-rapidapi-host"];
                    if (xrapidhost != "mbad.p.rapidapi.com" && xrapidhost != "yardillo.p.rapidapi.com")
                    { allpass = false; }

                }
               

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException e)
            {
                omess.MessageDesc = "Unabel to validate user" + e.ToString();
                _messagemaster.InsertOneAsync(omess);
                ReturnUnauthorizedResult(context);
            }
        }
        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }
    }
   
}
