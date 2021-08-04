using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MBADCases.Services
{
    public class CaseService
    {
        private readonly IMongoCollection<Case> _case;
        private IMongoDatabase database;
        public CaseService(ICasesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
              database = client.GetDatabase(settings.DatabaseName);

            _case = database.GetCollection<Case>(settings.CasesCollectionName);
        }

        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public Case Get(string id)=> 
            _case.Find<Case>(book => book._id == id).FirstOrDefault();
         
        public Case Create(Case ocase)
        {
            _case.InsertOneAsync(ocase);
            return ocase;
        }

        public void Update(string id,List<Caseattribute> caseAttrIn) 
        {
            //var filter = Builders<Case>.Filter.Eq("_id", id);
           
            foreach (Caseattribute csat in caseAttrIn)
            {
                var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id))
                        & Builders<BsonDocument>.Filter.Eq("Caseattributes._id", csat.Id);
                var arrayUpdate = Builders<BsonDocument>.Update.Set("Caseattributes.$.Value", csat.Value);

                var casecoll = database.GetCollection<BsonDocument>("Cases");
                casecoll.UpdateOne(arrayFilter, arrayUpdate);

                //var filter = Builders<Case>.Filter.Eq("_id", id)
                //      & Builders<Case>.Filter.Eq("Caseattributes.Id", csat.Id);
                //var update = Builders<Case>.Update.Set("Caseattributes.$.Id", csat.Value);
                //_case.UpdateOneAsync(filter, update);
            }
           
        }
        public void Update(string id, Case caseIn) =>
            _case.ReplaceOne(ocase => ocase._id == id, caseIn);

        public void Remove(Case caseIn) =>
            _case.DeleteOne(ocase => ocase._id == caseIn._id);

        public void Remove(string id) =>
            _case.DeleteOne(book => book._id == id);
    }
}
