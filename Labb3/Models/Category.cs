using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3.Models
{
    internal class Category
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; }

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
