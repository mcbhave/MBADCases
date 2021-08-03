using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MBADCases.Models
{
    public class Tenant : IWixItem
    {
         
        public string Tenantname { get; set; }
        public string Tenantdesc { get; set; }
        public string _id { get; set; }
        public string _owner { get; set; }
    }
}
