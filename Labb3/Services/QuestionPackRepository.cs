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


        public void Create(QuestionPack pack)
        {
            _collection.InsertOne(pack);
        }

   
        public void Update(QuestionPack pack)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, pack.Id);
            _collection.ReplaceOne(filter, pack);
        }

   
        public void Delete(ObjectId id)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, id);
            _collection.DeleteOne(filter);
        }

     
        public QuestionPack GetById(ObjectId id)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
