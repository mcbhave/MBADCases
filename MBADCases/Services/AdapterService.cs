using System;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using MBADCases.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
namespace MBADCases.Models
{
    public class AdapterService
    {
        private IMongoCollection<Adapter> _Adapterscollection;
        private IMongoDatabase MBADDatabase;
        private IMongoDatabase TenantDatabase;
        ICasesDatabaseSettings _settings;
        private MongoClient _client;
        public AdapterService(ICasesDatabaseSettings settings)
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
                _Adapterscollection = TenantDatabase.GetCollection<Adapter>(_settings.Adapterscollection);


            }
            catch { throw; };
        }
        public Adapter Create(Adapter oadapter)
        {
            try
            {
                _Adapterscollection.InsertOneAsync(oadapter);
                return oadapter;
            }
            catch
            {
                throw;
            }

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
                Callertype = ICallerType.ADAPTER,
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
