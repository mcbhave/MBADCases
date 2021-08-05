using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MBADCases.Models
{
    public class Case  
    {   
        public Case()
        {
            Fields = new List<Casefield>();
            CaseActions = new List<CaseAction>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Casenumber { get; set; }
        public string Casetitle { get; set; }
        public string Casetype { get; set; }
        public string Casestatus { get; set; }
        public string Casedescription { get; set; }
        public string Createdate { get; set; }
        public string Createuser { get; set; }

        public string Updatedate { get; set; }
        public string Updateuser { get; set; }
        public string Sladate { get; set; }
         
        public List<Casefield> Fields { get; set; }
        public List<CaseAction> CaseActions { get; set; }
        public MessageResponse Message { get; set; }

    }
     
    public class Option
    {
         
        public string Optionid { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public string Value { get; set; }
    }

    public class Casefield
    {
      
        public string Fieldid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public List<Option> Options { get; set; }
    }
    public class CaseAction
    {
        
        public string Actionid { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public string Value { get; set; }
        public List<Option> Options { get; set; }
    }
}