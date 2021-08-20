using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MBADCases.Authentication;
using MBADCases.Models;
using MBADCases.Services;
using static MBADCases.Models.WixDB;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MBADCases.Controllers
{
    [Route("data")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/data")]
    [BasicAuthWix("wix")]
    public class WixdataController : ControllerBase
    {
        private readonly CaseService _cases;
        public WixdataController(CaseService cases)
        {
            _cases = cases;
        }
        [Route("insert")]
        [Route("insert/{id?}")]
        [HttpPost]
        public IActionResult data(object  oid)
        {
            string usrid = HttpContext.Session.GetString("mbaduserid");
            string tenantid = HttpContext.Session.GetString("mbadtanent");
            string srequest = "";
            string smessage = "";
            string scasetypes = "";
            string sresponse = "";
            try
            {
                srequest= oid.ToString();
                 WixDB.data id = Newtonsoft.Json.JsonConvert.DeserializeObject<WixDB.data>(oid.ToString());
                 string js = Newtonsoft.Json.JsonConvert.SerializeObject(id.item);
                 var oitm =  Newtonsoft.Json.JsonConvert.DeserializeObject<Case>(js);
                 DataItem<WixCase> oi = new DataItem<WixCase>();
               List< DataItem<object>> od = new List<DataItem<object>>();
                foreach (Case c in _cases.Searchcases("Casetype=" + id.collectionName))
                {
                    DataItem<WixCase> o = new DataItem<WixCase>();
                    o.item._id = c._id;
                    o.item._owner = c.Createuser;
                    o.item.casetitle = c.Casetitle;
                    o.item.casetype = c.Casetype;
                    o.item.casestatus = c.Casestatus;
                    o.item.currentactivityid = c.Currentactivityid;
                    o.item.currentactionid = c.Currentactionid;
                    o.item.casedescription = c.Casedescription;
                    o.item.createdate = c.Createdate;
                    o.item.createuser = c.Createuser;
                    o.item.updatedate = c.Updatedate;
                    o.item.updateuser = c.Updateuser;
                    string oitem=  Newtonsoft.Json.JsonConvert.SerializeObject(o);
                 
                    JObject job = JObject.Parse(oitem);
                    var comparer = new MyCaseFieldOrder();
                     c.Fields.Sort(comparer);
                    foreach (Casefield f in c.Fields)
                    { 
                        job.Add(f.Fieldid, f.Value);
                    }
                    od.Add(job);
                  }
                                            
                sresponse = Newtonsoft.Json.JsonConvert.SerializeObject(od);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, od);

            }
            catch (Exception ex)
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix data", Messageype = "ERROR", MessageDesc = smessage + " " + ex.ToString(), Tenantid = tenantid, Userid = usrid });

                throw;
            }
            finally
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix data", MessageDesc = smessage, Tenantid = tenantid, Userid = usrid });
            }
        }

        [Route("get")]
        [Route("get/{id?}")]
        [HttpPost]
        public IActionResult getitem(object sid)
        {
            string usrid = HttpContext.Session.GetString("mbaduserid");
            string tenantid = HttpContext.Session.GetString("mbadtanent");
            string srequest = "";
            string smessage = "";
            string scasetypes = "";
            string sresponse = "";
            try
            {
                srequest= sid.ToString();
               WixDB.data id = Newtonsoft.Json.JsonConvert.DeserializeObject<WixDB.data>(sid.ToString());
                _cases.Gettenant(tenantid);
                string js = Newtonsoft.Json.JsonConvert.SerializeObject(id.item);
                var oitm = Newtonsoft.Json.JsonConvert.DeserializeObject<Case>(js);
                               
                DataItem<Case> oi = new DataItem<Case>();
                oi.item = _cases.Get(oitm._id);
                sresponse = Newtonsoft.Json.JsonConvert.SerializeObject(oi);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);

                  
 
            }
            catch (Exception ex)
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix dataItem", Messageype = "ERROR", MessageDesc = smessage + " " + ex.ToString(), Tenantid = tenantid, Userid = usrid });

                throw;
            }
            finally
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix dataItem", MessageDesc = smessage, Tenantid = tenantid, Userid = usrid });
            }


        }
        [Route("find")]
        [Route("find/{id?}")]
        [HttpPost]
        public IActionResult finditem(WixDB.data id)
        {
             
             
            string usrid = HttpContext.Session.GetString("mbaduserid");
            string tenantid = HttpContext.Session.GetString("mbadtanent");
            string srequest = "";
            string smessage = "";
            string scasetypes = "";
            string sresponse = "";
            try
            {
                _cases.Gettenant(tenantid);
                string js = Newtonsoft.Json.JsonConvert.SerializeObject(id.item);
                var oitm = Newtonsoft.Json.JsonConvert.DeserializeObject<Case>(js);
                DataItem<WixCase> oi = new DataItem<WixCase>();
                // List<FindItems<JObject>> od = new List<FindItems<JObject>>();
                List<JObject> o = new List<JObject>();
                foreach (Case c in _cases.Searchcases("Casetype=" + id.collectionName))
                {
                    WixCase ocase = new WixCase();
                    //job.Add("Casedescription", c.Casedescription);

                    ocase.casedescription = c.Casedescription;
                    ocase._id = c._id;
                    ocase._owner = c.Createuser;
                    ocase.casestatus = c.Casestatus;
                    ocase.casetitle = c.Casetitle;
                    ocase.casetype = c.Casetype;
                    ocase.createdate = c.Createdate;
                    ocase.createuser = c.Createuser;
                    ocase.currentactionid = c.Currentactionid;
                    ocase.currentactivityid = c.Currentactivityid;
                    ocase.updatedate = c.Updatedate;
                    ocase.updateuser = c.Updateuser;

                    string oitem = Newtonsoft.Json.JsonConvert.SerializeObject(ocase);

                    JObject job = JObject.Parse(oitem);
                    foreach (Casefield f in c.Fields)
                    {
                        job.Add(f.Fieldid.ToLower(), f.Value);
                    }
                    
                    o.Add(job);
                }

                FindItems<JObject> olistdata = new FindItems<JObject>();
                // MBADCases.Data.TenantData otendata = new Data.TenantData(id.collectionName);
                //List<Tenant> listtn = otendata.getTenants();
                olistdata.items = o;
                olistdata.totalCount = o.Count;
                sresponse = Newtonsoft.Json.JsonConvert.SerializeObject(olistdata);

                var retj = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(sresponse);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, olistdata);

            }
            catch (Exception ex)
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix data", Messageype = "ERROR", MessageDesc = smessage + " " + ex.ToString(), Tenantid = tenantid, Userid = usrid });

                throw;
            }
            finally
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix data", MessageDesc = smessage, Tenantid = tenantid, Userid = usrid });
            }
        }

        [Route("update")]
        [Route("update/{id?}")]
        [HttpPost]
        public IActionResult updateitem(object id)
        {
            string usrid = HttpContext.Session.GetString("mbaduserid");
            string tenantid = HttpContext.Session.GetString("mbadtanent");
            string srequest = "";
            string smessage = "";
            string scasetypes = "";
            string sresponse = "";
            try
            {
                srequest= Newtonsoft.Json.JsonConvert.SerializeObject(id);
                DataItem<item> oi = new DataItem<item>();
                WixDB.item oitem = new WixDB.item();// { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), model = "Camry" };
           
                oi.item = oitem;
                sresponse = Newtonsoft.Json.JsonConvert.SerializeObject(oitem);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
            }
            catch (Exception ex)
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix dataUpdate", Messageype = "ERROR", MessageDesc = smessage + " " + ex.ToString(), Tenantid = tenantid, Userid = usrid });

                throw;
            }
            finally
            {
                _cases.SetMessage(new Message() { Callerid = "Wix", Callerrequest = srequest, Callresponse = sresponse, Callerrequesttype = scasetypes, Callertype = "Wix dataUpdate", MessageDesc = smessage, Tenantid = tenantid, Userid = usrid });
            }
        }

        [Route("remove")]
        [Route("remove/{id?}")]
        [HttpPost]
        public IActionResult removeitem(object id)
        {
            helperservice.LogWixMessages("removeitem", Newtonsoft.Json.JsonConvert.SerializeObject(id));
            DataItem<item> oi = new DataItem<item>();
            WixDB.item oitem = new WixDB.item();// { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToString("MM-DD-YYYY HH:mm:ss") };
            oi.item = oitem;
          
            
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
        }

        [Route("count")]
        [Route("count/{id?}")]
        [HttpPost]
        public IActionResult countitem(object id)
        {
            helperservice.LogWixMessages("countitem", Newtonsoft.Json.JsonConvert.SerializeObject(id));
            DataCount ocount = new DataCount();
            ocount.totalCount = 50;
          

           
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocount);
        }
        
    }
}
