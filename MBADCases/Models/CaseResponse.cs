using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace MBADCases.Models
{
    public class CaseResponse
    {
        public CaseResponse(Case ocase, Message oms)
        {
            _id = ocase._id;
            //Casenumber = ocase.Casenumber;
            Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Casenumber { get; set; }
       
        public MessageResponse Message { get; set; }
    }
}
