using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;


class Program
{
    static async Task Main()
    {
        // HttpClient client = new HttpClient();
        // string apiUrl = "http://127.0.0.1:8000/level-up"; // FastAPI URL

        // // Create a new character (not Hero)
        // Character newPlayer = new Character("BuffIvan", 13);

        // // Explicitly set JSON serialization options
        // var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        // // Convert character to JSON
        // string json = JsonSerializer.Serialize(newPlayer, options);
        // HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

        // // Send character to FastAPI
        // HttpResponseMessage response = await client.PostAsync(apiUrl, content);
        // string result = await response.Content.ReadAsStringAsync();

        // if (!response.IsSuccessStatusCode)
        // {
        //     Console.WriteLine($"Error: {response.StatusCode} - {result}");
        //     return;
        // }

        // // Deserialize response (get updated character)
        // Character updatedCharacter = JsonSerializer.Deserialize<Character>(result, options);

        // Console.WriteLine($"Character {updatedCharacter.Name} is now Level {updatedCharacter.Level}!");

        // // 🔹 Insert or Update Character in MongoDB
        // dbConnection db = new dbConnection();
        // var playerCollection = db.GetCollection<Character>("Players");

        // var filter = Builders<Character>.Filter.Eq("Name", updatedCharacter.Name);
        // var update = Builders<Character>.Update.Set("Level", updatedCharacter.Level);
        // var optionsUpdate = new UpdateOptions { IsUpsert = true }; // If character doesn't exist, insert it

        // playerCollection.UpdateOne(filter, update, optionsUpdate);

        // Console.WriteLine($"Character {updatedCharacter.Name} saved to MongoDB!");

        // Create a character
        // Character character = new Character("Warrior");

        // // Create some gear items
        // Gear headGear = new Gear("Headgear", "A helmet that protects the head", "head", 10, 5, 2, 0, 0, 0);  
        // Gear shouldersGear = new Gear("Shoulders Armor", "Armor that protects the shoulders", "shoulders", 5, 3, 1, 0, 0, 0);
        // Gear weaponGear = new Gear("Magic Sword", "A sword that boosts magic power", "weapon", 0, 0, 0, 10, 0, 0); 


        // // Equip items to the character
        // Console.WriteLine("Equipping items...\n");
        // character.Equip(headGear);
        // character.Equip(shouldersGear);
        // character.Equip(weaponGear);

        // // Display stats after equipping
        // Console.WriteLine($"Stats after equipping items:");
        // Console.WriteLine($"Health: {character.Stats.CurrentStats.Health}");
        // Console.WriteLine($"Attack: {character.Stats.CurrentStats.Attack}");
        // Console.WriteLine($"Defense: {character.Stats.CurrentStats.Defense}");
        // Console.WriteLine($"Magic Attack: {character.Stats.CurrentStats.MagicAttack}");
        // Console.WriteLine($"Speed: {character.Stats.CurrentStats.Speed}\n");

        // // Remove the weapon
        // Console.WriteLine("Removing the weapon...\n");
        // character.RemoveItem("weapon");

        // // Display stats after removing the weapon
        // Console.WriteLine($"Stats after removing weapon:");
        // Console.WriteLine($"Health: {character.Stats.CurrentStats.Health}");
        // Console.WriteLine($"Attack: {character.Stats.CurrentStats.Attack}");
        // Console.WriteLine($"Defense: {character.Stats.CurrentStats.Defense}");
        // Console.WriteLine($"Magic Attack: {character.Stats.CurrentStats.MagicAttack}");
        // Console.WriteLine($"Speed: {character.Stats.CurrentStats.Speed}\n");

        // // Remove the shoulders gear
        // Console.WriteLine("Removing the shoulders gear...\n");
        // character.RemoveItem("shoulders");

        // // Display stats after removing shoulders
        // Console.WriteLine($"Stats after removing shoulders:");
        // Console.WriteLine($"Health: {character.Stats.CurrentStats.Health}");
        // Console.WriteLine($"Attack: {character.Stats.CurrentStats.Attack}");
        // Console.WriteLine($"Defense: {character.Stats.CurrentStats.Defense}");
        // Console.WriteLine($"Magic Attack: {character.Stats.CurrentStats.MagicAttack}");
        // Console.WriteLine($"Speed: {character.Stats.CurrentStats.Speed}\n");

        // // Remove the head gear
        // Console.WriteLine("Removing the head gear...\n");
        // character.RemoveItem("head");

        // // Display stats after removing head gear
        // Console.WriteLine($"Stats after removing head gear:");
        // Console.WriteLine($"Health: {character.Stats.CurrentStats.Health}");
        // Console.WriteLine($"Attack: {character.Stats.CurrentStats.Attack}");
        // Console.WriteLine($"Defense: {character.Stats.CurrentStats.Defense}");
        // Console.WriteLine($"Magic Attack: {character.Stats.CurrentStats.MagicAttack}");
        // Console.WriteLine($"Speed: {character.Stats.CurrentStats.Speed}\n");

        // Create a character
Character character = new Character("Hero");

// Create gear items
// Create gear items with appropriate stat changes
Gear sword = new Gear(
    name: "Sword",
    description: "A sharp sword.",
    slot: "weapon",
    healthChange: 1,      // +1 health
    attackChange: 1,      // +1 attack
    defenseChange: 1,     // +1 defense
    magicAttackChange: 1, // +1 magic attack
    magicDefenseChange: 1, // +1 magic defense
    speedChange: 1        // +1 speed
);

Gear shield = new Gear(
    name: "Shield",
    description: "A sturdy shield.",
    slot: "shoulders",
    healthChange: 0,
    attackChange: 2,      // +2 attack
    defenseChange: 3,     // +3 defense
    magicAttackChange: 0,
    magicDefenseChange: 0,
    speedChange: 0
);

// Add items to inventory
character.AddItemToInventory(sword);
character.AddItemToInventory(shield);

// Equip sword (removes it from inventory)
character.Equip(sword);
Console.WriteLine("Sword equipped.");

// Equip shield (removes it from inventory)
character.Equip(shield);
Console.WriteLine("Shield equipped.");


character.PrintEquippedItems();

// Remove sword (returns it to inventory)
character.RemoveItem("weapon");
Console.WriteLine("Sword removed from equipment.");

// Print inventory
character.PrintInventory();

character.PrintEquippedItems();

    }
}
