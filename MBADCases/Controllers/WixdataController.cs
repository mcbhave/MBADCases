using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MBADCases.Authentication;
using MBADCases.Models;
using MBADCases.Services;
using static MBADCases.Models.WixDB;

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
        [Route("insert")]
        [Route("insert/{id?}")]
        [HttpPost]
        public IActionResult data(object  oid)
        {
            helperservice.LogWixMessages("data", oid.ToString());
            WixDB.data id = Newtonsoft.Json.JsonConvert.DeserializeObject<WixDB.data>(oid.ToString());

            string js = Newtonsoft.Json.JsonConvert.SerializeObject(id.item);

            //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
            //FindTenants olistdata = new FindTenants();
            switch (id.collectionName.ToLower())
            {
                case "tenants":
                    var oitm =  Newtonsoft.Json.JsonConvert.DeserializeObject<Tenant>(js);
                    Tenant oten = new Tenant()
                    {
                        _id = Guid.NewGuid().ToString(),
                        Tenantname = oitm.Tenantname,
                      Tenantdesc=oitm.Tenantdesc ,
                       _owner=oitm._owner

                    };
                  Data.TenantData  ot = new Data.TenantData(oitm._id);
                 List<Tenant> colten =  ot.getTenants();
                    colten.Add(oten);
                   string strnetnt= Newtonsoft.Json.JsonConvert.SerializeObject(colten);
                    string spath = "Data/tenants.json";
                      System.IO.File.WriteAllText (spath, strnetnt);

                    //MBADCases.Data.TenantData otendata = new Data.TenantData(id.collectionName);
                    DataItem<Tenant> oi = new DataItem<Tenant>();
                    oi.item = oten;
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);

                case "casetypes":
                    //MBADCases.Data.CaseTypesData ocasetypesdata = new Data.CaseTypesData(id.collectionName);
                    //DataItem<CaseType> oicase = new DataItem<CaseType>();
                    //oicase.item = ocasetypesdata.GetCaseType(id.item._id);
                    //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oicase);
                    break;
                case "casetypefields":
                    //MBADCases.Data.CaseTypesFieldData ocasetypesFielddata = new Data.CaseTypesFieldData(id.collectionName);
                    //DataItem<CaseTypeField> oicasef = new DataItem<CaseTypeField>();
                    //oicasef.item = ocasetypesFielddata.GetCaseTypeField(id.item._id);
                    //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oicasef);
                    break;
            }
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, null);
        }

        [Route("get")]
        [Route("get/{id?}")]
        [HttpPost]
        public IActionResult getitem(object  sid)
        {
            helperservice.LogWixMessages("getitem", sid.ToString());
            WixDB.data id = Newtonsoft.Json.JsonConvert.DeserializeObject<WixDB.data>(sid.ToString());

            string js = Newtonsoft.Json.JsonConvert.SerializeObject(id.item);
            var oitm = Newtonsoft.Json.JsonConvert.DeserializeObject<Tenant>(js);

            switch (id.collectionName.ToLower())
            {
                case "tenants":
                   
                    MBADCases.Data.TenantData otendata = new Data.TenantData(id.collectionName);
                    DataItem<Tenant> oi = new DataItem<Tenant>();
                    oi.item = otendata.GetTenant(oitm._id);
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);

                case "casetypes":
                    MBADCases.Data.CaseTypesData ocasetypesdata = new Data.CaseTypesData(id.collectionName);
                    DataItem<CaseType> oicase = new DataItem<CaseType>();
                    oicase.item = ocasetypesdata.GetCaseType(oitm._id);
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oicase);

                case "casetypefields":
                    MBADCases.Data.CaseTypesFieldDataNOTUSED ocasetypesFielddata = new Data.CaseTypesFieldDataNOTUSED(id.collectionName);
                    DataItem<CaseTypeFieldnotused> oicasef = new DataItem<CaseTypeFieldnotused>();
                    oicasef.item = ocasetypesFielddata.GetCaseTypeField(oitm._id);
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oicasef);

            }
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, null);
            //WixDB.item oitem = new WixDB.item();// { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            //oi.item = oitem;




        }
        [Route("find")]
        [Route("find/{id?}")]
        [HttpPost]
        public IActionResult finditem(WixDB.data id)
        {
            helperservice.LogWixMessages("finditem", Newtonsoft.Json.JsonConvert.SerializeObject(id));
            switch (id.collectionName.ToLower())
            {
                case "tenants":
                    FindItems<Tenant> olistdata = new FindItems<Tenant>();
                    MBADCases.Data.TenantData otendata = new Data.TenantData(id.collectionName);
                    //List<Tenant> listtn = otendata.getTenants();
                    olistdata.items=  otendata.getTenants();
                    olistdata.totalCount = olistdata.items.Count;
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, olistdata);
                    
                case "casetypes":
                    FindItems<CaseType> olistcasetypedata = new FindItems<CaseType>();
                    MBADCases.Data.CaseTypesData ocasetypesdata = new Data.CaseTypesData(id.collectionName);
                    //List<Tenant> listtn = otendata.getTenants();
                    olistcasetypedata.items = ocasetypesdata.GetCaseTypes();
                    olistcasetypedata.totalCount = olistcasetypedata.items.Count;
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, olistcasetypedata);
                case "casetypefields":
                    FindItems<CaseTypeFieldnotused> olistcasetypefields = new FindItems<CaseTypeFieldnotused>();
                    MBADCases.Data.CaseTypesFieldDataNOTUSED ocasetypefieldsdata = new Data.CaseTypesFieldDataNOTUSED(id.collectionName);
                    //List<Tenant> listtn = otendata.getTenants();
                    olistcasetypefields.items = ocasetypefieldsdata.GetCaseTypeFields();
                    olistcasetypefields.totalCount = olistcasetypefields.items.Count;
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, olistcasetypefields);
                default:
                    //MBADCases.Data.TenantData otendata = new Data.TenantData(id.collectionName);
                    //olistdata.items = otendata.getTenants();
                    break;
            }

           

            //FindItems olistdata = new FindItems();
            //Guid g = Guid.NewGuid();


            //WixDB.item oitem = new WixDB.item() { _id =   Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToString("mmm dd, yyyy hh:mm tt") };
            //olistdata.items.Add(oitem);
            //oitem = new WixDB.item() { _id =   Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Ford", model = "Mustang", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            //olistdata.items.Add(oitem);
            //oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Tesla", model = "ES", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            //olistdata.items.Add(oitem);

            //olistdata.totalCount = olistdata.items.Count;


            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, null);
        }

        [Route("update")]
        [Route("update/{id?}")]
        [HttpPost]
        public IActionResult updateitem(object id)
        {
            helperservice.LogWixMessages("updateitem", Newtonsoft.Json.JsonConvert.SerializeObject(id));
            DataItem<item> oi = new DataItem<item>();
            WixDB.item oitem = new WixDB.item();// { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), model = "Camry" };
           
            oi.item = oitem;
           
          
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
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
