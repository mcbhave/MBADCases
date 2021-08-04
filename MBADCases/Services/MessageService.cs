using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MBADCases.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> _message;
        public  IMongoDatabase _database;
        public MessageService(ICasesDatabaseSettings settings, IMongoDatabase TenantDatabase)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = TenantDatabase;
            _message = TenantDatabase.GetCollection<Message>(settings.MessagesCollectionName);
        }
        public Message Create( Message omess)
        {
            try
            {
                _message.InsertOneAsync(omess);
                return omess;
            }
            catch
            {
                throw;
            }

        }
    }
    
}
