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
        private readonly IMongoCollection<MongoDB.Bson.BsonDocument> _case;

        public CaseService(ICasesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _case = database.GetCollection<MongoDB.Bson.BsonDocument>(settings.CasesCollectionName);
        }

        //public List<Case> Get() =>
        //    _case.Find(book => true).ToList();

        public BsonDocument Get(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            return _case.Find(filter).FirstOrDefault();
            //_case.Find<Case>(book => book._id == id).FirstOrDefault();
        }
        public BsonDocument Create(MongoDB.Bson.BsonDocument ocase)
        {
            _case.InsertOne(ocase);
            return ocase;
        }

        //public void Update(string id, Case caseIn) =>
        //    _case.ReplaceOne(ocase => ocase._id == id, caseIn);

        //public void Remove(Case caseIn) =>
        //    _case.DeleteOne(ocase => ocase._id == caseIn._id);

        //public void Remove(string id) =>
        //    _case.DeleteOne(book => book._id == id);
    }
}
