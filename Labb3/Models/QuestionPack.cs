using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Labb3.Models;

internal enum Difficulty { Easy, Medium, Hard }

internal class QuestionPack
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Name { get; set; }
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }

    // Relation till Category
    public ObjectId CategoryId { get; set; }

    public List<Question> Questions { get; set; }

    // Krävs av MongoDB
    public QuestionPack()
    {
        Questions = new List<Question>();
    }

    public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
    {
        Name = name;
        Difficulty = difficulty;
        TimeLimitInSeconds = timeLimitInSeconds;
        Questions = new List<Question>();
    }
}
