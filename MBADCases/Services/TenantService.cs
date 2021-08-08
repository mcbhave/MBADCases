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
    partial class TenantService
    {
        private readonly IMongoCollection<Tenant> _tenant;
        private IMongoDatabase MBADDatabase;
        ICasesDatabaseSettings _settings;
        private string _tenantid;
        public TenantService(ICasesDatabaseSettings settings)
        {
            try
            {
                _settings = settings;
                var client = new MongoClient(settings.ConnectionString);
                MBADDatabase = client.GetDatabase(settings.DatabaseName);
                _tenant = MBADDatabase.GetCollection<Tenant>("Tenants");
               
            }
            catch { throw; }
        }
        
        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public Tenant Get(string id)
        {
            try { return _tenant.Find<Tenant>(book => book._id == id).FirstOrDefault(); } catch { throw; };
        }

        public Tenant Create(Tenant ocase)
        {
            try
            {
                _tenant.InsertOneAsync(ocase);
                return ocase;
            }
            catch
            {
                throw;
            }

        }

        public void Update(string id, List<Casetypefield> caseAttrIn)
        {
            try
            {
                foreach (Casetypefield csat in caseAttrIn)
                {
                    var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id))
                            & Builders<BsonDocument>.Filter.Eq("Caseattributes.Attributeid", csat.Fieldid);
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
            try
            {
                _tenant.DeleteOne(book => book._id == id);
            }
            catch { throw; }
        }

        public Message SetMessage(Tenant ocase, string caseid, string srequest, string srequesttype, string sMessageCode, string sMessagedesc, string userid, Exception ex)
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
                Tenantid = ocase.Tenantname,
                Callerid = caseid,
                Callertype = ICallerType.TENANT,
                Messagecode = _MessageCode,
                Messageype = _MessageType,
                MessageDesc = _MessageDesc,
                Callerrequest = srequest,
                Callerrequesttype = srequesttype,
                Userid = userid,
                Messagedate = DateTime.UtcNow.ToString()
            };

            MessageService omesssrv = new MessageService(_settings, MBADDatabase, MBADDatabase);
            oms = omesssrv.Create(oms);

            return oms;

        }
    }
}
