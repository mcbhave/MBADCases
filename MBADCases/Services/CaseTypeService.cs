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
        private IMongoCollection<CaseType> _casetypecollection;
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
        public CaseTypeService(IMongoCollection<CaseType> casetypecollection)
        {
            try
            {
                _casetypecollection = casetypecollection;
            }
            catch { throw; }
        }
        public void Gettenant(string tenantid)
        {
            try
            {
                TenantDatabase = helperservice.Gettenant(tenantid, _client, MBADDatabase, _settings);
                _casetypecollection = TenantDatabase.GetCollection<CaseType>("CaseTypes");
            }
            catch { throw; };
        }
        public CaseType Get(string id)
        {
            try { return _casetypecollection.Find<CaseType>(book => book._id == id).FirstOrDefault(); } catch { throw; };
        }
        public CaseType GetByName(string name)
        {
            try{ return _casetypecollection.Find<CaseType>(book => book.Casetype.ToLower() == name.ToLower()).FirstOrDefault(); } catch { throw; };
        }
        public CaseType Create(string CaseTypeName,CaseType ocasetype)
        {
            try
            {
                if (ocasetype.Casetype != CaseTypeName) { ocasetype.Casetype = CaseTypeName; }
                //if (ocasetype.Updateuser == null) { ocasetype.Updateuser = createuserid; }
                if (ocasetype.Createdate == null) { ocasetype.Createdate = DateTime.UtcNow.ToString(); }
                if (ocasetype.Updatedate == null) { ocasetype.Updatedate = DateTime.UtcNow.ToString(); }

                _casetypecollection.InsertOneAsync(ocasetype);
                return ocasetype;
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
                _casetypecollection.ReplaceOne(ocase => ocase._id == id, CaseTypeIn);
               
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
