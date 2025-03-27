using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Threading.Tasks;


public class dbConnection
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<User> _userCollection;
    private IMongoCollection<Character> _characterCollection;

    public dbConnection()
    {
        string connectionString = "mongodb+srv://mechamit000:1FhnVwbO6e54fLRa@character.btcp0.mongodb.net/?retryWrites=true&w=majority&appName=character";
        string databaseName = "touchyourtoes";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        _userCollection = _database.GetCollection<User>("Users");
        _characterCollection = _database.GetCollection<Character>("characters"); // Assuming Character is your class

    }

    
    public async Task<bool> CheckIfCharacterExists(string characterName)
    {
        var character = await _characterCollection.Find(c => c.Name == characterName).FirstOrDefaultAsync();
        return character != null;
    }
    // Retrieves all characters from the database
    public async Task<List<Character>> GetAllCharacters()
    {
        return await _characterCollection.Find(_ => true).ToListAsync();
    }
    
    public async Task<Character> LoadCharacter(string name)
{
    var filter = Builders<Character>.Filter.Eq("_id", name);
    var character = await _characterCollection.Find(filter).FirstOrDefaultAsync();

    if (character == null)
    {
        return null; // Character not found
    }

    // Your polymorphic character loading logic
    return character.ClassName.ToLowerInvariant() switch
    {
        "warrior" => new Warrior(character),
        "mage" => new Mage(character),
        "archer" => new Archer(character),
        "ninja" => new Ninja(character),
        _ => new CustomClass(character) // Handle custom character class
    };
}


   public async Task UpdateCharacter(Character updatedCharacter)
{
    

    Console.WriteLine($"🔄 Updating character with ID: {updatedCharacter.Id}");

    var filter = Builders<Character>.Filter.Eq("_id", updatedCharacter.Id);
    
    // Debug: Check if character exists in the database before updating
    var existingCharacter = await _characterCollection.Find(filter).FirstOrDefaultAsync();
    if (existingCharacter == null)
    {
        Console.WriteLine($"❌ No character found with ID: {updatedCharacter.Id}. Update aborted.");
        return;
    }

    var result = await _characterCollection.ReplaceOneAsync(filter, updatedCharacter);

    Console.WriteLine($"✅ Update Result - Matched Count: {result.MatchedCount}, Modified Count: {result.ModifiedCount}");
}






}






public class User
{
    [BsonId] // Set username as the primary key
    [BsonRepresentation(BsonType.String)]
    public string Username { get; set; }
    public string Password { get; set; }

    public List<Character> Characters { get; set; } = new List<Character>();
}