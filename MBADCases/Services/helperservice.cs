using MBADCases.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBADCases.Services
{
    public static class helperservice
    {
        public static IMongoDatabase Gettenant(string tenantid, MongoClient Client, IMongoDatabase MBADDatabase, ICasesDatabaseSettings settings)
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
                IMongoDatabase TenantDatabase = Client.GetDatabase(oten._id); ;
                                
                SetMBADMessage(settings,   MBADDatabase,ICallerType.TENANT, oten._id, tenantid, "TENANT", "Success", "Tenant login", tenantid, null);

                return TenantDatabase;
            }
            catch { throw; };
        }
        public static  Message SetMBADMessage(ICasesDatabaseSettings settings, IMongoDatabase MBADDatabase, string callrtype, string caseid, string srequest, string srequesttype, string sMessageCode, string sMessagedesc, string userid, Exception ex)
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

            MessageService omesssrv = new MessageService(settings, MBADDatabase);
            oms = omesssrv.Create(oms);

            return oms;

        }
    }
    
}
