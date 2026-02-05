using Labb3.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Labb3.Services
{
    internal class QuestionPackRepository
    {
        private readonly IMongoCollection<QuestionPack> _collection;

        public QuestionPackRepository()
        {
            
            var client = new MongoClient("mongodb://localhost:27017");

            
            var database = client.GetDatabase("KevinKongpachith");

           
            _collection = database.GetCollection<QuestionPack>("QuestionPacks");
        }

        public List<QuestionPack> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }
    }
}
