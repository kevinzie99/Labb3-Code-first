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
            try
            {
                return _collection.Find(Builders<QuestionPack>.Filter.Empty).ToList();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching QuestionPacks: {ex.Message}");
                return new List<QuestionPack>();
            }
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

       
        public void Delete(string id)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, id);
            _collection.DeleteOne(filter);
        }


        public QuestionPack GetById(string id)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
