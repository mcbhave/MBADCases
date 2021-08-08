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
        private readonly IMongoCollection<Message> _messagemaster;
        public  IMongoDatabase _database;
       
        public MessageService(ICasesDatabaseSettings settings, IMongoDatabase TenantDatabase, IMongoDatabase MBADDatabase)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = TenantDatabase;
            _message = TenantDatabase.GetCollection<Message>(settings.MessagesCollectionName);
            _messagemaster = MBADDatabase.GetCollection<Message>("Logs");
        }
        public Message Create( Message omess)
        {
            try
            {
                if(omess.Messageype == "ERROR")
                {
                    _messagemaster.InsertOneAsync(omess);
                }

                omess.MessageDesc = "";
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
