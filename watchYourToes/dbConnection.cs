using MongoDB.Driver;

public class dbConnection
{
    private readonly IMongoDatabase _database;

    public dbConnection()
    {
        string connectionString = "mongodb+srv://mechamit000:1FhnVwbO6e54fLRa@character.btcp0.mongodb.net/?retryWrites=true&w=majority&appName=character";
        string databaseName = "touchyourtoes";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
