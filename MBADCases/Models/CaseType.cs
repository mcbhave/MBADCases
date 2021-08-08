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
        public string Fieldname { get; set; }
        public int Seq { get; set; }
        public bool Required { get; set; }
        public string message { get; set; }
        public string Value { get; set; }
        public List<Option> Options { get; set; }
       
    }
    public class SetCasetypefield
    {
        public string Fieldid { get; set; }
        public string Value { get; set; }
    }
    public class FieldOption
    {
        public string Optionid { get; set; }
    }

    public class Adapterresponse
    {
        public Adapterresponse()
        {
            Fields = new List<SetCasetypefield>();
        }
        public string Response { get; set; }
        public List<SetCasetypefield> Fields { get; set; }
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
        public bool Actioncomplete { get; set; }
        public string Actionname { get; set; }
        public string Actionstatus { get; set; }
        public string Actionparentid { get; set; }
        public string Actionparentresponse { get; set; }
        public bool Isdisabled { get; set; }
        public string Actionid { get; set; }
        public string Activityid { get; set; }
        public string Caseid { get; set; }
        public string Actiontype { get; set; }
        public Actionauth Actionauth { get; set; }
        public string Adapterid { get; set; }
        public string Adapterresponseattr { get; set; }
        public List<Adapterresponse> Adapterresponse { get; set; }
        public List<Casetypefield> Fields { get; set; }
        public int Activityseq { get; set; }
      
        // Default comparer for Part type.
        public int CompareTo(Action compareSeq)
        {
            // A null value means that this object is greater.
            if (compareSeq == null)
                return 1;

            else
                return this.Actionseq.CompareTo(compareSeq.Actionseq);
        }
    }
    public class Actionauth
    {
        public string Fieldid { get; set; }
        public string ValueX { get; set; }
        public string Type { get; set; }
        public string Oprator { get; set; }
        public string ValueY { get; set; }
        public bool Defaultreturn { get; set; }
        public bool Returniftrue { get; set; }
        public bool Returniffalse { get; set; }
    }
    public class Activity
    {
        public Activity()
           {
            Actions = new List<Action>();
            Isdisabled = false;
            Activityseq = 0;
            }
        public string Activityid { get; set; }
        public bool Activitycomplete { get; set; }
        public bool Isdisabled { get; set; }
        public int Activityseq { get; set; }
        public string Activityname { get; set; }
        public string Activitydesc { get; set; }
        public List<Action> Actions { get; set; }
        public int CompareTo(Activity compareSeq)
        {
            // A null value means that this object is greater.
            if (compareSeq == null)
                return 1;
            else
                return this.Activityseq.CompareTo(compareSeq.Activityseq);
        }
    }

}
