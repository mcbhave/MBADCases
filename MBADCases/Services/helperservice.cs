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
        public static string GetFieldValueByFieldID(Case fcase , string Fieldid)
        {
            if (fcase != null)
            {
                Casefield ocf;
                if (fcase.Fields != null)
                {
                   if((ocf = fcase.Fields.Where(f => f.Fieldid.ToLower() == Fieldid.ToLower()).FirstOrDefault()) != null){
                        return ocf.Value;
                    }
                }
            }
            return "";
        }
        public static bool GetCompareResults( Case ocase, Models.Action iAct , IMongoCollection<ActionAuthLogs> Logcollection)
        {
            if (iAct  == null) { return true;  }
            if (iAct.Actionauth == null) { return WriteCompareLog(Logcollection, iAct, "Action Auth configuration is null", true); }
             
                if (iAct.Actionauth.Fieldid != null || iAct.Actionauth.Fieldid == "")
                {
                    iAct.Actionauth.ValueX = helperservice.GetFieldValueByFieldID(ocase, iAct.Actionauth.Fieldid);
                }
             
            var sactionconfig = Newtonsoft.Json.JsonConvert.SerializeObject(iAct.Actionauth);
            var defaultret = iAct.Actionauth.Defaultreturn;
            var bretiftrue = iAct.Actionauth.Returniftrue;
            var bretiffalse= iAct.Actionauth.Returniffalse;
            var FieldValue = iAct.Actionauth.ValueX;
            if (iAct.Actionauth.Oprator==null || iAct.Actionauth.Oprator == "") { iAct.Actionauth.Oprator = "="; }
            switch (iAct.Actionauth.Type.ToUpper())
            {
                case "STRING":
                    switch (iAct.Actionauth.Oprator.ToUpper())
                    {
                        case "=":
                            if (FieldValue.ToLower() == iAct.Actionauth.ValueY.ToLower())
                            { return WriteCompareLog(Logcollection, iAct, sactionconfig, bretiftrue);}
                            else { return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiffalse);}
                        case "CONTAINS":
                            if (FieldValue.ToLower().Contains(iAct.Actionauth.ValueY.ToLower()))
                            { return bretiftrue; }
                            else { return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiffalse); }
                        case "STARTSWITH":
                            if (FieldValue.ToLower().StartsWith(iAct.Actionauth.ValueY.ToLower()))
                            { return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiftrue); }
                            else {   return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiffalse);  }
                        case "ENDSWITH":
                            if (FieldValue.ToLower().EndsWith(iAct.Actionauth.ValueY.ToLower()))
                            { return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiftrue); }
                            else {   return WriteCompareLog(Logcollection,  iAct, sactionconfig, bretiffalse);  }
                        default:
                            return defaultret;
                    }
                default:
                    return defaultret;
            }
        }
        public static bool WriteCompareLog(IMongoCollection<ActionAuthLogs> _logs,  Models.Action iAct, string Logdesc, bool Returnbool)
        {
            

            ActionAuthLogs olog = new ActionAuthLogs() { Activityid = iAct.Activityid, Caseid = iAct.Caseid, Logdesc = Logdesc, Actionid = iAct.Actionid, Actionauthresult = Returnbool,Actionseq=iAct.Actionseq, Activityseq=iAct.Activityseq  };

                 
                _logs.InsertOneAsync(olog);
            
           
            return Returnbool;
        }
    }
   
    public class MyActivityOrder : IComparer<Activity>
    {
        public int Compare(Activity x, Activity y)
        {
            int compareDate = x.Activityseq.CompareTo(y.Activityseq);
            if (compareDate == 0)
            {
                return x.Activityseq.CompareTo(y.Activityseq);
            }
            return compareDate;
        }


    }
    public class MyActionOrder : IComparer<Models.Action>
    {
        public int Compare(Models.Action x, Models.Action y)
        {
            int compareDate = x.Actionseq.CompareTo(y.Actionseq);
            if (compareDate == 0)
            {
                return x.Actionseq.CompareTo(y.Actionseq);
            }
            return compareDate;
        }


    }
}
