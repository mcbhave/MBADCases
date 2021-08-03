using MBADCases.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MBADCases.Data
{
    public class CaseTypesFieldData
    {
        private string _id { get; set; }
        public CaseTypesFieldData(string id)
        {
            _id = id;
        }
        public List<CaseTypeField> GetCaseTypeFields()
        {
            List<CaseTypeField> oten = GetAllCaseTypeFieldsfromDB();

            return oten;
        }
        public CaseTypeField GetCaseTypeField(string _id)
        {
            List<CaseTypeField> oten = GetAllCaseTypeFieldsfromDB();
            return (CaseTypeField)oten.SingleOrDefault(x => x._id == _id);

        }
        private static List<CaseTypeField> GetAllCaseTypeFieldsfromDB()
        {
            string spath = "Data/casetypefieldsdata.json";
            string sSchemas = System.IO.File.ReadAllText(spath);
            List<CaseTypeField> lsch = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CaseTypeField>>(sSchemas);

            return lsch;
        }
    }
}
