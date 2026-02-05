using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3.Models
{
    internal class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        
        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
