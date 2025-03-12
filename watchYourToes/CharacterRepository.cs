using MongoDB.Driver;
using System.Threading.Tasks;

public class CharacterRepository
{
    private readonly IMongoCollection<Character> _characterCollection;

    public CharacterRepository()
    {
        dbConnection db = new dbConnection();
        _characterCollection = db.GetCollection<Character>("characters"); // Accesses "characters" collection
    }

    //Method to insert (push) a new characters
    public async Task SaveCharacter(Character character)
    {
        await _characterCollection.InsertOneAsync(character);
    }

    //Method to retrieve a character by name
    public async Task<Character> GetCharacterByName(string name)
    {
        return await _characterCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
    }
}
