using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MBADCases.Models;
using MBADCases.Authentication;
using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Http;
using System.IO;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;

namespace MBADCases.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [BasicAuthFilter()]
    public class mpclubController : Controller
    {
        [MapToApiVersion("1.0")]
        [HttpPost()]
        public IActionResult Post(string id)
        {

            try
            {
                
                HttpClient _client = new HttpClient();
                //  _client.DefaultRequestHeaders.Add(shead[0], shead[1]);
                _client.DefaultRequestHeaders.Accept.Add(
                  new MediaTypeWithQualityHeaderValue("application/json"));
                //get users in the current timezone
                StringBuilder slog = new StringBuilder();
                StringBuilder slog1 = new StringBuilder();

                string param = "";//"?constraints=[{\"key\":\"timezone_text\",\"constraint_type\": \"equals\", \"value\": \"" + timezone + "\"}]";
                string susers = helperservice.GetRESTResponse("GET", "https://wimsupapp.bubbleapps.io/version-test/api/1.1/obj/mpclubusers" + param, "", _client, slog1);

                 mpclubuser colusers = Newtonsoft.Json.JsonConvert.DeserializeObject<mpclubuser>(susers);

                List<mpclubmessageResult> allmessages = new List<mpclubmessageResult>();

                if (colusers != null) {

                    if (colusers.response != null)
                    {
                        if (colusers.response.results != null)
                        {
                            if (colusers.response.count > 0)
                            {
                                List<mpclubuserResult> AllActiveUsers = colusers.response.results.FindAll(x => x.active_boolean == true);

                                var DistinctItems = AllActiveUsers.GroupBy(x => x.current_location_text).Select(y => y.First());
                                foreach (var item in DistinctItems)
                                {
                                    string sFromdate = "";
                                    string sToDate = "";
                                    string timezone = item.current_location_text;// "India Standard Time";
                                    try
                                    {
                                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.Now,
                                            TimeZoneInfo.FindSystemTimeZoneById(timezone));
                                          sFromdate = indianTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"); //DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'")
                                          sToDate = indianTime.AddMinutes(30).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
                                    }
                                    catch { }

                                    if (sFromdate != "") {
                                          _client = new HttpClient();
                                      
                                        _client.DefaultRequestHeaders.Accept.Add(
                                          new MediaTypeWithQualityHeaderValue("application/json"));
                                        //get messages at this time
                                        param = "?constraints=[{\"key\":\"publish_date\",\"constraint_type\": \"greater than\", \"value\": \"" + sFromdate + "\"},{\"key\":\"publish_date\",\"constraint_type\": \"less than\", \"value\": \"" + sToDate + "\"}]";
                                    string smessages = helperservice.GetRESTResponse("GET", "https://wimsupapp.bubbleapps.io/version-test/api/1.1/obj/mpclubmessages" + param, "", _client, slog1);
                                    mpclubmessage colmessages = Newtonsoft.Json.JsonConvert.DeserializeObject<mpclubmessage>(smessages);

                                    if (colmessages!=null && colmessages.response.results != null)
                                    {
                                        foreach (mpclubmessageResult omess in colmessages.response.results)
                                        {
                                                omess.yardillo_location = timezone;
                                            allmessages.Add(omess);
                                        }
                                    }
                                    }

                                }

                                //if messages are avaliable
                                if (allmessages.Count > 0)
                                {
                                    foreach (mpclubmessageResult omess in allmessages)
                                    {
                                       List<mpclubuserResult> allvalidmessageusers = AllActiveUsers.FindAll(x => x.current_location_text == omess.yardillo_location);
                                        //all active users
                                        foreach (mpclubuserResult u in allvalidmessageusers)
                                        {
                                            //get all messages if any
                                            //this sends out message to paid user and message type is send to all paid
                                            
                                            if (u.subscription_text.ToUpper() == "ALL THE LOVE" && (omess.send_to_all_paid_text!=null && omess.send_to_all_paid_text=="Yes"))
                                            {
                                                slog.Append("Usecase 1 : User : " + u.subscription_text.ToUpper() + " , message send to all : " + omess.send_to_all_paid_text);

                                                if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                {
                                                    //send an email this message to this user
                                                    slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                }
                                                else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                {
                                                    //send a text message this message to this user
                                                    slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                }
                                            }
                                            else if (u.subscription_text.ToUpper() == "ALL THE LOVE" && (omess.send_to_all_paid_text == null || omess.send_to_all_paid_text == ""))
                                            {
                                                slog.Append("Usecase 2 : User : " + u.subscription_text.ToUpper() + " , message send to all : NULL ");
                                               
                                                //this is based on the sun 
                                                if (u.astro_sign_text != "" && omess.message_text.Contains(u.astro_sign_text))
                                                {
                                                    slog.Append(" Astro sign : " + u.astro_sign_text);
                                                    if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                    {
                                                        //send an email this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                    }
                                                    else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                    {
                                                        //send a text message this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                    }
                                                }

                                                //this is based on the   life 
                                                if (u.life_path_number_number > 0 && omess.message_text.Contains("LP" + u.life_path_number_number))
                                                {
                                                    slog.Append(" LP : " + u.life_path_number_number);
                                                    if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                    {
                                                        //send an email this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                    }
                                                    else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                    {
                                                        //send a text message this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                    }
                                                }
                                                //this is based on the   life 
                                                if (u.myers_briggs_type_text != "" && omess.mb_type_text.Contains(u.myers_briggs_type_text))
                                                {
                                                    slog.Append(" MB : " + u.myers_briggs_type_text);
                                                  
                                                    if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                    {
                                                        //send an email this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                    }
                                                    else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                    {
                                                        //send a text message this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                    }
                                                }
                                                //this is based on the   life 
                                                if (u.enneagram_number_text != "" && omess.enneagram_text.Contains(u.enneagram_number_text))
                                                {
                                                    slog.Append(" En : " + u.enneagram_number_text);
                                                    if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                    {
                                                        //send an email this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                    }
                                                    else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                    {
                                                        //send a text message this message to this user
                                                        slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                    }
                                                }
                                            }
                                            else if (u.subscription_text.ToUpper() == "FREE LOVE" &&  (omess.subscription_type_text != null && omess.subscription_type_text =="Free"))
                                            {
                                                slog.Append("Usecase 3 : User : " + u.subscription_text.ToUpper() + " , message send to all : Free ");
                                             
                                                if (u.delivery_method_text.ToUpper() == "EMAIL")
                                                {
                                                    //send an email this message to this user
                                                    slog.Append("Message:" + omess.message_text + ", EMAIL SENT");
                                                }
                                                else if (u.delivery_method_text.ToUpper() == "TEXT")
                                                {
                                                    //send a text message this message to this user
                                                    slog.Append("Message:" + omess.message_text + ", SMS SENT");
                                                }
                                            }
                                        }
                                    }
                                   
                                   
                                }
                              
                            }
                        }
                    }
                }


                //var currtimeoffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                //DateTime sdate = DateTime.Now;
                //switch (sdate.Hour)
                //{
                //    case 1:
                //        break;
                //    default:
                //        break;

                //}

             

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, slog);
            }
            catch (Exception ex)
            {


                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, null);
            }
        }
       
    }
  }
