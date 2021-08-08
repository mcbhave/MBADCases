using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
 

namespace MBADCases.Models
{
    public class Adapter
    {
        public Adapter()
        {
            
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Macroname { get; set; }
        public bool Isdisabled { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public List<string> Headers { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public MessageResponse Message { get; set; }

    }
}
