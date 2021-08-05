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
    public class CaseTypeService
    {
        private IMongoCollection<CaseType> _casecollection;
        private IMongoDatabase MBADDatabase;
        private IMongoDatabase TenantDatabase;
        ICasesDatabaseSettings _settings;
        private MongoClient _client;
        public CaseTypeService(ICasesDatabaseSettings settings)
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
            try
            {
                TenantDatabase = helperservice.Gettenant(tenantid, _client, MBADDatabase, _settings);
                _casecollection = TenantDatabase.GetCollection<CaseType>("CaseTypes");
            }
            catch { throw; };
        }
        public CaseType Get(string id)
        {
            try { return _casecollection.Find<CaseType>(book => book._id == id).FirstOrDefault(); } catch { throw; };
        }
        public CaseType GetByName(string name)
        {
            try { return _casecollection.Find<CaseType>(book => book.Casetype.ToLower() == name.ToLower()).FirstOrDefault(); } catch { throw; };
        }
        public CaseType Create(CaseType ocase)
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

        public void Update(string id, CaseType CaseTypeIn)
        {
            try
            {
                _casecollection.ReplaceOne(ocase => ocase._id == id, CaseTypeIn);
               
            }
            catch { throw; }
        }
        public Message SetMessage(string casetypeid, string srequest, string srequesttype, string sMessageCode, string sMessagedesc, string userid, Exception ex)
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
                Callerid = casetypeid,
                Callertype = ICallerType.CASETYPE ,
                Messagecode = _MessageCode,
                Messageype = _MessageType,
                MessageDesc = _MessageDesc,
                Callerrequest = srequest,
                Callerrequesttype = srequesttype,
                Userid = userid,
                Messagedate = DateTime.UtcNow.ToString()
            };

            MessageService omesssrv = new MessageService(_settings, TenantDatabase);
            oms = omesssrv.Create(oms);

            return oms;

        }


    }
}
