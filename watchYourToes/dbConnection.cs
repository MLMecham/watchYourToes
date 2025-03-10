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

    public async Task<bool> CreateUser(string username, string password)
    {
        username = username.ToLower();

        if (await GetUser(username) != null)
        {
            return false; // Username already exists
        }

        User newUser = new User { Username = username, Password = password };

        await _userCollection.InsertOneAsync(newUser);
        return true; // User created successfully
    }

    public async Task<User> GetUser(string username)
    {

        username = username.ToLower();
        return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        return enteredPassword == storedPassword;
    }




    public async Task<bool> AssignCharacterToUser(string username, string characterName)
{
    try
    {
        // Filter to find the user by username
        var filter = Builders<User>.Filter.Eq("Username", username);

        // Update the ActiveCharacter field to the specified character name
        var update = Builders<User>.Update.Set("ActiveCharacter", characterName);

        // Execute the update operation
        var result = await _userCollection.UpdateOneAsync(filter, update);

        // Return true if a document was modified (i.e., the update was successful)
        return result.ModifiedCount > 0;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error assigning character to user: {ex.Message}");
        return false;
    }
}

public async Task<bool> CheckIfUsernameExists(string username)
    {
        var user = await _userCollection.Find(Builders<User>.Filter.Eq("Username", username)).FirstOrDefaultAsync();
        return user != null; // Return true if the username exists
    }


    public async Task<bool> CheckIfCharacterExistsForUser(string username, string characterName)
    {
        try
        {
            var user = await _userCollection.Find(Builders<User>.Filter.Eq("Username", username)).FirstOrDefaultAsync();

            if (user != null)
            {
                // Assuming user["Characters"] is a list of character names (or a related class)
                var existingCharacter = user.Characters.FirstOrDefault(c => c.Name == characterName); // Or adjust to how your structure looks
                return existingCharacter != null; // Return true if character name exists
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking if character exists for user: {ex.Message}");
            return false;
        }
    }

    // Other methods like CreateUser, GetUser, etc.
}






public class User
{
    [BsonId] // Set username as the primary key
    [BsonRepresentation(BsonType.String)]
    public string Username { get; set; }
    public string Password { get; set; }

    public List<Character> Characters { get; set; } = new List<Character>();
}
