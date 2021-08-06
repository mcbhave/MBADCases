using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MBADCases.Services
{
    public class CaseService  
    {
        private  IMongoCollection<Case> _casecollection;
        private IMongoCollection<CaseDB> _casedbcollection;
        private IMongoCollection<CaseType> _casetypecollection;
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
                _casetypecollection = TenantDatabase.GetCollection<CaseType>(_settings.CaseTypesCollectionName);
                _casedbcollection= TenantDatabase.GetCollection<CaseDB>(_settings.CasesCollectionName);
            }
            catch { throw; };
        }
        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public Case Get(string id) {
            try { 
                Case ocase= _casecollection.Find<Case>(book => book._id == id).FirstOrDefault();
                //get case type
                CaseTypeService octsr = new CaseTypeService(_casetypecollection);

                //get case type definations
                CaseType oct = octsr.GetByName(ocase.Casetype);
                if (oct == null)
                {
                    oct = new CaseType();
                    oct.Casetype = ocase.Casetype;
                    //Register new case type
                    oct = octsr.Create(ocase.Casetype, oct);
                }

                //Get current activity details
                var scurrentact = ocase.Currentactivityid;
                Activity odact = new Activity();
                if (scurrentact == null || scurrentact == "")
                {
                    //get the first activity from case type defination
                    oct.Activities.Sort();
                    odact = oct.Activities.FirstOrDefault();
                }
                else
                {
                    odact = oct.Activities.Where(ct => ct.Activityid.ToLower() == scurrentact.ToLower()).FirstOrDefault();
                }

               
               // ocase.Activities.Add(odact);

                //Get all field definations and values
                //fields are set and created within an activity
                foreach(Activity octypeact in oct.Activities)
                {
                   foreach(Models.Action oa in octypeact.Actions)
                    {
                         
                    }
                }


                return ocase;
            } catch { throw; };
        }
         
        public CaseDB Create(Case ocase)
        {
            try
            {
                //first get the case type
                //get case type details
                CaseTypeService octsr = new CaseTypeService(_casetypecollection);
                CaseType oct = octsr.GetByName(ocase.Casetype);
                if (oct == null) {
                    oct = new CaseType();
                    oct.Casetype = ocase.Casetype;
                    //Register new case type
                    oct= octsr.Create(ocase.Casetype, oct);
                }
                //var sdefaultact = oct.Defaultactivityid;
                //Activity odact;
                //if (sdefaultact==null || sdefaultact == "")
                //{
                //    oct.Activities.Sort();
                //    //get the first one if any
                //    odact = oct.Activities.Where(a=>a.Activitydisabled==false).FirstOrDefault();
                //}
                //else
                //{
                //   odact = oct.Activities.Where(ct => ct.Activityid.ToLower() == sdefaultact.ToLower()).FirstOrDefault();
                //}
                
                foreach(Activity odact in oct.Activities)
                {

                    //set this as current activity
                    CaseActivity icaseActivity = new CaseActivity();
                    icaseActivity.Activityid = odact.Activityid;
                    icaseActivity.Activityseq = odact.Activityseq;
                    icaseActivity.Activitydisabled = odact.Activitydisabled;
                    int totalactionsfin = 0;
                    if (odact.Activitydisabled == false)
                    {
                        Models.Action oaction;
                      
                        if (odact.Actions.Count > 0) {
                        odact.Actions.Sort(); 
                        oaction = odact.Actions.FirstOrDefault();
                        if (oaction != null)
                        {
                            List<Casetypefield> oflds = oaction.Fields.Where(o => o.Required == true).ToList();

                                foreach (Casetypefield ofl in oflds)
                                {
                                    Casefield casefield;
                                    if ((casefield = ocase.Fields.Where(o => o.Fieldid.ToUpper() == ofl.Fieldid.ToUpper()).FirstOrDefault()) != null)
                                    {
                                        if(casefield.Value ==null || casefield.Value == "")
                                        {
                                            throw new Exception("Field is reqired, Fieldid:" + ofl.Fieldid);
                                        } 
                                    }
                                    else
                                    {
                                        throw new Exception("Field and its value is missing, Fieldid:" + ofl.Fieldid);
                                    }
                                }
                            }
                        }

                        //if reaches this point then check if the first activity action is an EVENT OR TASK
                        //execute EVENT TYPE actions until it reaches a task
                        odact.Actions.Sort();

                        //collect all Events until first TASK is found
                   
                        foreach (Models.Action iAct in odact.Actions)
                        {
                            CaseAction iCaseActn = new CaseAction();
                            iCaseActn.Actionid = iAct.Actionid;
                            iCaseActn.Actiontype = iAct.Actiontype;
                            if (iAct.Actiontype.ToUpper() == "EVENT")
                            {
                                Adapterresponse oAdpResp = new Adapterresponse();

                                if (iAct.Adapterid != null)
                                {
                                    //execute adapter
                                    oAdpResp = iAct.Adapterresponse.Where(a => a.Actionresponse.ToUpper() == "TRUE").FirstOrDefault();
                                }
                                else
                                {
                                    //Adapterresponseattr is also null in this case
                                    //use TRUE
                                    oAdpResp = iAct.Adapterresponse.Where(a => a.Actionresponse.ToUpper() == "TRUE").FirstOrDefault();
                                }

                                if (oAdpResp != null) {
                                    iCaseActn.Adapterresponse = oAdpResp.Actionresponse;
                                //no adapter associated
                                //Translate and assign fields
                                    foreach (Models.SetCasetypefield ofl in oAdpResp.Fields)
                                        {
                                            Casefield ocasesetfld;
                                            if ((ocasesetfld = ocase.Fields.Where(F => F.Fieldid.ToLower() == ofl.Fieldid.ToLower()).FirstOrDefault()) != null)
                                            {
                                                ocasesetfld.Value = ofl.Value;
                                            }
                                            else
                                            {
                                            //add field
                                            ocasesetfld = new Casefield();
                                                ocasesetfld.Fieldid = ofl.Fieldid;
                                                ocasesetfld.Value = ofl.Value;
                                                ocase.Fields.Add(ocasesetfld);
                                            }
                                        }

                                    //at this point activity action is complete
                                    //set Actioncomplete = true and  Actionstatus = "COMPLETE"
                               
                                    iCaseActn.Actioncomplete = true;
                                    iCaseActn.Actionstatus = "SUCCESS";
                                    icaseActivity.Actions.Add(iCaseActn);
                                }
                            }
                            else
                            {
                                //stop
                                break;
                            }
                            totalactionsfin =+ 1;
                        }
                        if (totalactionsfin == odact.Actions.Count)
                        {
                            icaseActivity.Activitycomplete = true;
                        }
                        ocase.Currentactivityid = odact.Activityid;
                        ocase.Activities.Add(icaseActivity);
                        break;
                    }
                    else
                    {
                        icaseActivity.Activitycomplete = true;
                        ocase.Currentactivityid = odact.Activityid;
                        ocase.Activities.Add(icaseActivity);
                    }
                   
              
                }
                //convert case to caseDB
                var oc= ocase.ToJson();
                //BsonDocument ob= BsonDocument.Parse(oc);
                CaseDB ocasedb = Newtonsoft.Json.JsonConvert.DeserializeObject<CaseDB>(oc);

                _casedbcollection.InsertOneAsync(ocasedb);
                return ocasedb;
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
