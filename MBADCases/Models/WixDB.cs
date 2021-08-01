using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBADCases.Models
{
    public class WixDB
    {
        public partial class Schema
        {
            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("Id")]
            public string Id { get; set; }

            [JsonProperty("allowedOperations")]
            public string[] AllowedOperations { get; set; }

            [JsonProperty("maxPageSize")]
            public long MaxPageSize { get; set; }

            [JsonProperty("ttl")]
            public long ttl { get; set; }

            [JsonProperty("fields")]
            public IDictionary<string, FieldValue> Fields { get; set; }

            [JsonProperty("defaultSort")]
            public defaultSort defaultSort { get; set; }

             
            
        }
        public class DBSchemas
        {
            [JsonProperty("Schemas")]
            public List<Schema> Schemas { get; set; }
           
            
        }
        public partial class RequestContext
        {
            [JsonProperty("settings")]
            public Settings Settings { get; set; }

            [JsonProperty("instanceId")]
            public Guid InstanceId { get; set; }

            [JsonProperty("installationId")]
            public Guid InstallationId { get; set; }

            [JsonProperty("memberId")]
            public Guid MemberId { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }
        }
        public class provision
        {
            public RequestContext RequestContext { get; set; }

        }
        public class data
        {
            public RequestContext RequestContext { get; set; }
            public string collectionName { get; set; }

            public item item { get; set; }
            
        }
        public class DataItem
        {
            public item item { get; set; }
            
        }
        public class DataCount
        {
            public int totalCount { get; set; }
           
            
        }
        public class DataItems
        {
       
            public List<item> item {get;set;}
            public DataItems()
            {
                item = new List<item>();
            }
             
            public int totalCount { get; set; }
            
        }
        public class FindItems
        {

            public List<item> items { get; set; }
            public FindItems()
            {
                items = new List<item>();
            }

            public int totalCount { get; set; }

        }
        public class item
        {
            [JsonProperty("_id")]
            public string _id { get; set; }

            [JsonProperty("_owner")]
            public string _owner { get; set; }


            [JsonProperty("make")]
            public string make { get; set; }


            [JsonProperty("model")]
            public string model { get; set; }

          

            [JsonProperty("year")]
            public int year { get; set; }

            [JsonProperty("date_added")]
            public string date_added { get; set; }
           
            
        }
        public class DateAdded
        {
            [JsonProperty("$date")]
            public string date { get; set; }
        }
        public class find
        {
            public RequestContext RequestContext { get; set; }
            public string[] schemaIds { get; set; }
        }
        public class Settings
        {
            [JsonProperty("Authorization")]
            public string Authorization { get; set; }
        }
    }

    public class defaultSort
    {
        public string fieldName { get; set; }
        public string direction { get; set; }
    }
    public class FieldValue
    {
        
            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("queryOperators")]
            public string[] QueryOperators { get; set; }
         

    }

}
