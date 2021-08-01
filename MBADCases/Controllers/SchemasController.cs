using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
namespace MBADCases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemasController : ControllerBase
    {
        [Route("")]
        [Route("controller")]
        [HttpPost]
        public string Index()
        {

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

            List<WixDB.Schema> lsch = new List<WixDB.Schema>();
 
             IDictionary<string, FieldValue> ofields = new Dictionary<string, FieldValue>();
            ofields.Add(new KeyValuePair<string,FieldValue>("_id",
                       new FieldValue()
                        {
                            DisplayName = "_id",
                            QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                            Type = "text"
                       })
                );
            ofields.Add(new KeyValuePair<string, FieldValue>("_owner",
                      new FieldValue()
                      {
                          DisplayName = "_owner",
                          QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                          Type = "text"
                      })
               );
            ofields.Add(new KeyValuePair<string, FieldValue>("make",
                    new FieldValue()
                    {
                        DisplayName = "make",
                        QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("model",
                    new FieldValue()
                    {
                        DisplayName = "model",
                        QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("year",
                 new FieldValue()
                 {
                     DisplayName = "year",
                     QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                     Type = "number"
                 })
          );
            ofields.Add(new KeyValuePair<string, FieldValue>("date_added",
                 new FieldValue()
                 {
                     DisplayName = "date_added",
                     QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                     Type = "datetime"
                 })
          );


            WixDB.Schema osch = new WixDB.Schema
            {
                DisplayName = "Car",
                Id = "car",
                AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" },
                MaxPageSize=50,
                ttl = 3600,
                Fields = ofields 
            };
            lsch.Add(osch);



            ///second schema manufracture

            ofields = new Dictionary<string, FieldValue>();
            ofields.Add(new KeyValuePair<string, FieldValue>("_id",
                       new FieldValue()
                       {
                           DisplayName = "_id",
                           QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                           Type = "text"
                           
                       })
                );
            ofields.Add(new KeyValuePair<string, FieldValue>("_owner",
                      new FieldValue()
                      {
                          DisplayName = "_owner",
                          QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                          Type = "text"
                      })
               );
            ofields.Add(new KeyValuePair<string, FieldValue>("name",
                    new FieldValue()
                    {
                        DisplayName = "name",
                        QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("country",
                    new FieldValue()
                    {
                        DisplayName = "country",
                        QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("share_price",
                 new FieldValue()
                 {
                     DisplayName = "share_price",
                     QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                     Type = "number"
                 })
          );
            ofields.Add(new KeyValuePair<string, FieldValue>("established",
                 new FieldValue()
                 {
                     DisplayName = "established",
                     QueryOperators = new string[] { "eq","lt","gt","hasSome","and","lte","gte","or","not","ne","startsWith","endsWith" },
                     Type = "datetime"
                 })
          );


             osch = new WixDB.Schema
            {
                DisplayName = "Manufacturer",
                Id = "manufacturer",
                AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" },
                 MaxPageSize = 50,
                 ttl=3600,
                 Fields = ofields


            };
            lsch.Add(osch);


            WixDB.DBSchemas osc = new WixDB.DBSchemas { Schemas = lsch };
            
         
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, osc);
        }

        [Route("list")]
        [Route("list/{id?}")]
        [HttpPost]
        public IActionResult list(WixDB.find id)
        {
            List<WixDB.Schema> lsch = new List<WixDB.Schema>();

            WixDB.Schema osch = GetSchema("Tenants", "List of All Tenants", 50, 3600);
            lsch.Add(osch);

            osch = GetSchema("Case Types", "All case types", 50, 3600);
            lsch.Add(osch);

            osch = GetSchema("Case1", "My Case Type 1", 50, 3600);
            lsch.Add(osch);

            WixDB.DBSchemas osc = new WixDB.DBSchemas { Schemas = lsch };
            
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, osc);
        }

        private static WixDB.Schema GetSchema(string name, string description ,int maxPage, int ttl)
        {
            WixDB.Schema osch = new WixDB.Schema
            {
                DisplayName = name,
                Id = description,
                AllowedOperations = new string[] { "get", "find", "count", "update", "insert", "remove" },
                MaxPageSize = 50,
                ttl = 3600,
                Fields = GetFields()
            };
            return osch;
        }
        private static IDictionary<string, FieldValue> GetFields()
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
            ofields.Add(new KeyValuePair<string, FieldValue>("make",
                    new FieldValue()
                    {
                        DisplayName = "make",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("model",
                    new FieldValue()
                    {
                        DisplayName = "model",
                        QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                        Type = "text"
                    })
             );
            ofields.Add(new KeyValuePair<string, FieldValue>("year",
                 new FieldValue()
                 {
                     DisplayName = "year",
                     QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                     Type = "number"
                 })
          );
            ofields.Add(new KeyValuePair<string, FieldValue>("date_added",
                 new FieldValue()
                 {
                     DisplayName = "date_added",
                     QueryOperators = new string[] { "eq", "lt", "gt", "hasSome", "and", "lte", "gte", "or", "not", "ne", "startsWith", "endsWith" },
                     Type = "datetime"
                 })
          );

            return ofields;
        }
    }
}
