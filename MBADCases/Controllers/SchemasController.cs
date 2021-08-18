using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MBADCases.Authentication;
using MBADCases.Models;
using MBADCases.Services;
using Microsoft.AspNetCore.Http;

namespace MBADCases.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [BasicAuthWix("wix")]
    public class SchemasController : ControllerBase
    {
        [Route("")]
        [Route("controller")]
        [HttpPost]
        public string Index()
        {
            helperservice.LogWixMessages("Index", "");

            //List<WixDB.Schema> lsch = new List<WixDB.Schema>();
            //WixDB.Schema osch = new WixDB.Schema
            //{
            //    DisplayName = "Car",
            //    _Id = "car",
            //    AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" }



            //};
            //lsch.Add(osch);
            //return Newtonsoft.Json.JsonConvert.SerializeObject(lsch);
            return "{}";
        }

        [Route("find")]
        [Route("find/{id?}")]
        [HttpPost]
      
        public IActionResult find(WixDB.find id)
        {
            helperservice.LogWixMessages("find", Newtonsoft.Json.JsonConvert.SerializeObject(id));


            List<WixDB.Schema> lsch = new List<WixDB.Schema>();

            IDictionary<string, FieldValue> ofields = new Dictionary<string, FieldValue>();
            ofields.Add(new KeyValuePair<string, FieldValue>("_id",
                       new FieldValue()
                       {
                           DisplayName = "_id",
                           QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                           Type = "text"
                       })
                );
            ofields.Add(new KeyValuePair<string, FieldValue>("_owner",
                      new FieldValue()
                      {
                          DisplayName = "_owner",
                          QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                          Type = "text"
                      })
               );
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantname",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Name",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantdesc",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Description",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
             
         
            WixDB.Schema osch = new WixDB.Schema
            {
                DisplayName = "tenants",
                Id = "tenants",
                AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" },
                MaxPageSize = 50,
                ttl = 3600,
                Fields = ofields
            };
            lsch.Add(osch);

             


            WixDB.DBSchemas osc = new WixDB.DBSchemas { Schemas = lsch };


             
            helperservice.LogWixMessages("find_response", Newtonsoft.Json.JsonConvert.SerializeObject(osc));
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, osc);
        }

        [Route("list")]
        [Route("list/{id?}")]
        [HttpPost]
        public IActionResult list(WixDB.find id)
        {

            var usrid = HttpContext.Session.GetString("mbaduserid");
            var tenantid = HttpContext.Session.GetString("mbadtanent");

            helperservice.LogWixMessages("list", Newtonsoft.Json.JsonConvert.SerializeObject(id));

            List<WixDB.Schema> lsch = new List<WixDB.Schema>();

            WixDB.Schema osch = GetSchema("tenants", "Tenants", 50, 3600);
            lsch.Add(osch);



            //osch = GetSchema("casetypes", "Case Types", 50, 3600);
            //lsch.Add(osch);

            //osch = GetSchema("casetypefields", "Case Type Fields", 50, 3600);
            //lsch.Add(osch);
            string sjson = Newtonsoft.Json.JsonConvert.SerializeObject(lsch);


            WixDB.DBSchemas osc = new WixDB.DBSchemas { Schemas = lsch };
           
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, osc);
        }

        private static List<WixDB.Schema> GetAllSchemasfromDB()
        {
            string spath =   "Models/Schemas.json";
            string sSchemas = System.IO.File.ReadAllText(spath);
            List<WixDB.Schema> lsch =   Newtonsoft.Json.JsonConvert.DeserializeObject<List<WixDB.Schema>>(sSchemas);

            return lsch;
        }

        private static WixDB.Schema GetSchema(string name, string description ,int maxPage, int ttl)
        {
            WixDB.Schema osch = new WixDB.Schema
            {
                DisplayName = description,
                Id = name,
                AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" },
                MaxPageSize = 50,
                ttl = 3600,
                Fields = GetFields(name)
            };
            return osch;
        }
        private static IDictionary<string,FieldValue> GetFields(string name)
        {
            IDictionary<string, FieldValue> ofields=null;
            switch (name.ToLower())
            {
                case "tenants":
                    ofields= GetTenantFields();
                    break;
                case "casetypes":
                    ofields = GetCaseTypes();
                    break;
                case "casetypefields":
                    ofields = GetCaseTypeFields();
                    break;
            }
            return ofields;
        }
        private static IDictionary<string, FieldValue> GetWixFields()
        {
            IDictionary<string, FieldValue> ofields = new Dictionary<string, FieldValue>();
            ofields.Add(new KeyValuePair<string, FieldValue>("_id",
                       new FieldValue()
                       {
                           DisplayName = "_id",
                           QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                           Type = "text"
                       })
                );
            ofields.Add(new KeyValuePair<string, FieldValue>("_owner",
                      new FieldValue()
                      {
                          DisplayName = "_owner",
                          QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                          Type = "text"
                      })
               );

            return ofields;
        }
        private static IDictionary<string, FieldValue> GetTenantFields()
        {
            IDictionary<string, FieldValue> ofields = GetWixFields();
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantname",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Name",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantdesc",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Description",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            
            return ofields;
        }
        private static IDictionary<string, FieldValue> GetCaseTypes()
        {
            IDictionary<string, FieldValue> ofields = GetWixFields();
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantid",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Id",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("casetypename",
                    new FieldValue()
                    {
                        DisplayName = "Case Type Description",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("casetypedesc",
                  new FieldValue()
                  {
                      DisplayName = "Case Type Description",
                      QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                      Type = "text"
                  })
           );
            return ofields;
        }

        private static IDictionary<string, FieldValue> GetCaseTypeFields()
        {
            IDictionary<string, FieldValue> ofields = GetWixFields();
            ofields.Add(new KeyValuePair<string, FieldValue>("tenantid",
                    new FieldValue()
                    {
                        DisplayName = "Tenant Id",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("casetypeid",
                    new FieldValue()
                    {
                        DisplayName = "Case Type Id",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("fieldname",
                  new FieldValue()
                  {
                      DisplayName = "Field Name",
                      QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                      Type = "text"
                  })
           );
            ofields.Add(new KeyValuePair<string, FieldValue>("fielddesc",
                 new FieldValue()
                 {
                     DisplayName = "Field Description",
                     QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                     Type = "text"
                 })
          ); 
            ofields.Add(new KeyValuePair<string, FieldValue>("fieldtype",
                  new FieldValue()
                  {
                      DisplayName = "Field Type",
                      QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                      Type = "text"
                  })
           );
            return ofields;
        }
      
    }
}
