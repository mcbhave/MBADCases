using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MBADCases.Models
{
    public class Case
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Casetype { get; set; }
 
        public string Casetitle { get; set; }
        public string Casedescription { get; set; }
        public string CaseAttributes { get; set; }

    }
}
