using MongoDB.Bson;
using MongoDB.Driver;
using Labb3.Models;
using System.Collections.Generic;
using System.Linq;

namespace Labb3.Services
{
    internal class CategoryRepository
    {
        private readonly IMongoCollection<Category> _collection;

        public CategoryRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("KevinKongpachith");

            _collection = database.GetCollection<Category>("Categories");
        }

        public List<Category> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public void Create(Category category)
        {
            _collection.InsertOne(category);
        }

        public void Delete(string id)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, id);
            _collection.DeleteOne(filter);
        }


        public void EnsureDefaultCategories()
        {
            var all = GetAll();
            if (!all.Any())
            {
                Create(new Category("Matte"));
                Create(new Category("Historia"));
                Create(new Category("Sport"));
            }
        }
    }
}
