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
        private  IMongoCollection<Case> _casecollection;
        private IMongoDatabase MBADDatabase;
        private IMongoDatabase TenantDatabase;
        ICasesDatabaseSettings _settings;
        private MongoClient _client;
        public CaseService(ICasesDatabaseSettings settings)
        {
            try
            {
                _settings = settings;
                _client = new MongoClient(settings.ConnectionString);
                MBADDatabase = _client.GetDatabase(settings.DatabaseName);

            }
            catch { throw; }
        }
        public void Gettenant(string tenantid)
        {
            try {  
            TenantDatabase = helperservice.Gettenant(tenantid, _client, MBADDatabase, _settings);
                _casecollection =  TenantDatabase.GetCollection<Case>(_settings.CasesCollectionName);
            }
            catch { throw; };
        }
        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public Case Get(string id) {
            try { return _casecollection.Find<Case>(book => book._id == id).FirstOrDefault(); } catch { throw; };
        }
         
        public Case Create(Case ocase)
        {
            try
            {
                _casecollection.InsertOneAsync(ocase);
                return ocase;
            }
            catch
            {
                throw;
            }
          
        }

        public void Update(string id,Case CaseIn) 
        {
            try { 
            foreach (Casefield csat in CaseIn.Fields)
            {
                var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id))
                        & Builders<BsonDocument>.Filter.Eq("Fields.Fieldid", csat.Fieldid);
                var arrayUpdate = Builders<BsonDocument>.Update.Set("Fields.$.Value", csat.Value);

                var casecoll = TenantDatabase.GetCollection<BsonDocument>(_settings.CasesCollectionName);
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
                _casecollection.DeleteOne(book => book._id == id);
            }
            catch { throw; }
        }

        public Message SetMessage(string  callrtype,string caseid, string srequest,string srequesttype, string sMessageCode, string sMessagedesc,string userid, Exception ex)
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
                Callertype = callrtype,
                Messagecode = _MessageCode, 
                Messageype = _MessageType,
                MessageDesc= _MessageDesc,
                Callerrequest=srequest,
                Callerrequesttype=srequesttype,
                Userid= userid,
                Messagedate=DateTime.UtcNow.ToString()
        };
            
            MessageService omesssrv = new MessageService(_settings,TenantDatabase);
            oms= omesssrv.Create(oms);
           
            return oms;

        }

        
    }
}
