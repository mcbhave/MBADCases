using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MBADCases.Models
{
    public class CaseType
    {
        public CaseType()
        {
           Activities=new List<Activity>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Casetype { get; set; }
        public string Casestypedesc { get; set; }
        public bool Casetypedisabled { get; set; }
        public string Createdate { get; set; }
        public string Createuser { get; set; }
        public string Updateuser { get; set; }
        public string Updatedate { get; set; }
        public string Defaultactivityid { get; set; }
        public List<Activity> Activities { get; set; }

        public MessageResponse Message { get; set; }
    }
    public class Casetypefield
    {
        public string Fieldid { get; set; }
        public string Value { get; set; }
    }

    public class Adapterresponse
    {
        public Adapterresponse()
        {
            Attributes = new List<Casetypefield>();
        }
        public string Response { get; set; }
        public string Adapterid { get; set; }
        public List<Casetypefield> Attributes { get; set; }
        public string Actionresponse { get; set; }
    }

    public class Action
    {
        public Action()
        {
            Adapterresponse = new List<Adapterresponse>();
            Fields = new List<Casetypefield>();
        }
        public int Actionseq { get; set; }
        public string Actionparentid { get; set; }
        public string Actionparentresponse { get; set; }
        public bool Actionenabled { get; set; }
        public string Actionid { get; set; }
        public string Actiontype { get; set; }
        public string Adapterid { get; set; }
        public string Adapterresponseattr { get; set; }
        public List<Adapterresponse> Adapterresponse { get; set; }
        public List<Casetypefield> Fields { get; set; }
    }

    public class Activity
    {
        public Activity()
           {
            Actions = new List<Action>();
            }
        public string Activityid { get; set; }
        public string Activityname { get; set; }
        public string Activitydesc { get; set; }
        public string Duedays { get; set; }
        public string Duedaystype { get; set; }
        public string Roles { get; set; }
        public List<Action> Actions { get; set; }
    }

    

}
