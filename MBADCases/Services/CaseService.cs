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
        private  IMongoCollection<Case> _case;
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

                //_case = MBADDatabase.GetCollection<Case>(settings.CasesCollectionName);
            }
            catch { throw; }
        }
        public void Gettenant(string tenantid)
        {
            IMongoCollection<Tenant> _tenant = MBADDatabase.GetCollection<Tenant>("Tenants");
            try
            {
                Tenant oten = _tenant.Find<Tenant>(book => book.Tenantname == tenantid).FirstOrDefault();
                if (oten == null)
                {
                    oten = new Tenant();
                    oten._owner = tenantid;
                    oten.Tenantname = tenantid;
                    oten.Tenantdesc = "";
                    oten.Createdate = DateTime.UtcNow.ToString();
                    //register new tenant
                    _tenant.InsertOne(oten);
                 
                }
                TenantDatabase = _client.GetDatabase(oten._id);
                SetMBADMessage( ICaseTypes.TENANT , oten._id, tenantid, "TENANT", "Success", "Tenant login", tenantid, null);
                _case = TenantDatabase.GetCollection<Case>(_settings.CasesCollectionName);
            }
            catch { throw; };
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

                var casecoll = MBADDatabase.GetCollection<BsonDocument>(_settings.CasesCollectionName);
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

        public Message SetMBADMessage(string callrtype, string caseid, string srequest, string srequesttype, string sMessageCode, string sMessagedesc, string userid, Exception ex)
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
            Message oms = new Message
            {
                Callerid = caseid,
                Callertype = callrtype,
                Messagecode = _MessageCode,
                Messageype = _MessageType,
                MessageDesc = _MessageDesc,
                Callerrequest = srequest,
                Callerrequesttype = srequesttype,
                Userid = userid,
                Messagedate = DateTime.UtcNow.ToString()
            };

            MessageService omesssrv = new MessageService(_settings, MBADDatabase);
            oms = omesssrv.Create(oms);

            return oms;

        }
    }
}
