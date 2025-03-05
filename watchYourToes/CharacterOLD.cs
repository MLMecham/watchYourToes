// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;
// using System.Text.Json.Serialization;

// public class Character
// {
//     [BsonId]
//     public ObjectId Id { get; set; } // Unique ID for MongoDB

//     [JsonPropertyName("name")]  // Ensures correct JSON property name
//     public string Name { get; set; }

//     [JsonPropertyName("level")]  // Ensures correct JSON property name
//     public int Level { get; set; }

//     public Character() { }

//     public Character(string name, int level)
//     {
//         Name = name;
//         Level = level;
//     }
// }
