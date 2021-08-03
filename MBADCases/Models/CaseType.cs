using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MBADCases.Models
{
    public class CaseType
    {
        public CaseType()
        {
            //CaseTypeFields = new List<CaseTypeField>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Tenantid { get; set; }
        public string Casetypename { get; set; }
        public string Casetypedesc { get; set; }

        //public List<CaseTypeField> CaseTypeFields { get; set; }
    }
}
