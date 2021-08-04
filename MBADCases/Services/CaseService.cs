using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MBADCases.Services
{
    public class CaseService  
    {
        private readonly IMongoCollection<Case> _case;
        private IMongoDatabase database;
        ICasesDatabaseSettings _settings;
        public CaseService(ICasesDatabaseSettings settings)
        {
            try
            {
                _settings = settings;
                var client = new MongoClient(settings.ConnectionString);
                database = client.GetDatabase(settings.DatabaseName);

                _case = database.GetCollection<Case>(settings.CasesCollectionName);
            }
            catch { throw; }
        }

        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public Case Get(string id) {
            try { return _case.Find<Case>(book => book._id == id).FirstOrDefault(); } catch { throw; };
        }
         
        public Case Create(Case ocase)
        {
            try
            {
                _case.InsertOneAsync(ocase);
                return ocase;
            }
            catch
            {
                throw;
            }
          
        }

        public void Update(string id,List<Caseattribute> caseAttrIn) 
        {
            try { 
            foreach (Caseattribute csat in caseAttrIn)
            {
                var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id))
                        & Builders<BsonDocument>.Filter.Eq("Caseattributes._id", csat.Id);
                var arrayUpdate = Builders<BsonDocument>.Update.Set("Caseattributes.$.Value", csat.Value);

                var casecoll = database.GetCollection<BsonDocument>(_settings.CasesCollectionName);
                casecoll.UpdateOne(arrayFilter, arrayUpdate);
            }
            }
            catch { throw; }
        }
        //public void Update(string id, Case caseIn) =>
        //    _case.ReplaceOne(ocase => ocase._id == id, caseIn);

        //public void Remove(Case caseIn) =>
        //    _case.DeleteOne(ocase => ocase._id == caseIn._id);

        public void Remove(string id)
        {
            try {
                _case.DeleteOne(book => book._id == id);
            }
            catch { throw; }
        }

        public Message SetMessage(Case ocase, string caseid, string srequest,string srequesttype, string sMessageCode, string sMessagedesc,string userid, Exception ex)
        {
          
            var _MessageType = string.Empty;
            var _MessageCode = string.Empty;
            var _MessageDesc = string.Empty;
            if (ex != null)
            {
                _MessageType = "ERROR";
                _MessageCode = ex.Message;
                _MessageDesc = ex.ToString();
            }
            else
            {
                _MessageType = "INFO";
                _MessageCode = sMessageCode;
                _MessageDesc = sMessagedesc;
            }
            Message oms = new Message {
                Callerid = caseid,
                Callertype = ICaseTypes.CASE,
                Messagecode = _MessageCode, 
                Messageype = _MessageType,
                MessageDesc= _MessageDesc,
                Callerrequest=srequest,
                Callerrequesttype=srequesttype,
                Userid= userid,
                Messagedate=DateTime.UtcNow.ToString()
        };
            
            MessageService omesssrv = new MessageService(_settings);
            oms= omesssrv.Create(oms);
           
            return oms;

        }
    }
}
