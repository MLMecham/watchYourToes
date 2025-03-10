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
    // Create a character
    Character character = new Mage("Harold"); 

    using (HttpClient client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://127.0.0.1:8000/");
        BattleMessage battleMessage = new BattleMessage();
        {
            battleMessage.name = character.Name;
            battleMessage.action = "Attack";
            battleMessage.enemy = "Skeleton";
            string jsonMessage = JsonSerializer.Serialize(battleMessage);
            StringContent content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("battle-ai", content);
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Battle AI Response: " + result);

        }
    }


    // Create gear items
    Gear sword = new Gear(
        name: "Sword",
        description: "A sharp sword.",
        slot: "weapon",
        healthChange: 1,      // +1 health
        attackChange: 1,      // +1 attack
        defenseChange: 1,     // +1 defense
        magicAttackChange: 1, // +1 magic attack
        magicDefenseChange: 1, // +1 magic defense
        speedChange: 100        // +1 speed
    );
    Gear BigSword = new Gear(
        name: "Giant Sword",
        description: "A sharp sword.",
        slot: "weapon",
        healthChange: 30,      // +1 health
        attackChange: 1,      // +1 attack
        defenseChange: 40,     // +1 defense
        magicAttackChange: 1, // +1 magic attack
        magicDefenseChange: 1, // +1 magic defense
        speedChange: 100        // +1 speed
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


    character.LevelUp();

    // Add items to inventory
    character.AddItemToInventory(sword);
    character.AddItemToInventory(shield);
    character.AddItemToInventory(BigSword);

    character.MoveAllInventoryToStorage();

    Console.WriteLine("\n-- Inventory Updated --\n");

    // Equip sword (removes it from inventory)
    character.Equip(sword);
    Console.WriteLine("\nSword equipped.\n");

    // Equip shield (removes it from inventory)
    character.Equip(shield);
    Console.WriteLine("\nShield equipped.\n");

    character.PrintEquippedItems();
    Console.WriteLine();
    character.PrintBaseStats();
    character.PrintCurrentStats();
    Console.WriteLine();

    // Remove sword (returns it to inventory)
    character.RemoveItem("weapon");
    Console.WriteLine("\nSword removed from equipment.\n");

    character.PrintInventory();
    Console.WriteLine();
    character.PrintEquippedItems();
    Console.WriteLine();
    character.PrintBaseStats();
    character.PrintCurrentStats();
    Console.WriteLine();
    }
}
