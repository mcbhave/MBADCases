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
        private IMongoDatabase database;
        public MessageService(ICasesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
            _message = database.GetCollection<Message>(settings.MessagesCollectionName);
        }
        public Message Create(Message omess)
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
