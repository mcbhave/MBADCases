using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MBADCases.Models
{
    public class TenantUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Tenantname { get; set; }
        public string Userid { get; set; }

        public string Createdate { get; set; }

        public string Createuserid { get; set; }
    }
}
