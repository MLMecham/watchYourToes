using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Welcome. Please choose one of the following options:\n 1) Create a character\n 2) Retrieve a character\n ");
        string input = Console.ReadLine();
        CharacterRepository characterRepo = new CharacterRepository();

        if (input == "1")
        {
            Console.Write("Please enter a name for your character: ");
            string create_character_name = Console.ReadLine();
            Console.Write("Please enter a class for your character (Mage, Archer, or Warrior):\n");
            string create_character_class = Console.ReadLine();

            Character character = new Character(create_character_name,create_character_class);
          

            // Save the character to MongoDB
            await characterRepo.SaveCharacter(character);
            Console.WriteLine("Character created and saved to the database!");
        }
        else if (input == "2")
        {
            Console.Write("Enter the character's name to retrieve: ");
            string retrieve_character_name = Console.ReadLine();
            Character retrievedCharacter = await characterRepo.GetCharacterByName(retrieve_character_name);

            if (retrievedCharacter != null)
            {
                Console.WriteLine("Character found!");
                Console.WriteLine($"You are a {retrievedCharacter.ClassName} named {retrievedCharacter.Name}. You are level {retrievedCharacter.Level} and have {retrievedCharacter.Exp} XP.");
            }
            else
            {
                Console.WriteLine("Character not found.");
            }
        }
    }
}


//     static async Task Main()
//     {
//     // Create a character
//     Character character = new Mage("Harold"); 

//     using (HttpClient client = new HttpClient())
//     {
//         client.BaseAddress = new Uri("http://127.0.0.1:8000/");
//         BattleMessage battleMessage = new BattleMessage();
//         {
//             battleMessage.name = character.Name;
//             battleMessage.action = "Attack";
//             battleMessage.enemy = "Skeleton";
//             string jsonMessage = JsonSerializer.Serialize(battleMessage);
//             StringContent content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
//             HttpResponseMessage response = await client.PostAsync("battle-ai", content);
//             string result = await response.Content.ReadAsStringAsync();
//             Console.WriteLine("Battle AI Response: " + result);

//         }
//     }


//     // Create gear items
//     Gear sword = new Gear(
//         name: "Sword",
//         description: "A sharp sword.",
//         slot: "weapon",
//         healthChange: 1,      // +1 health
//         attackChange: 1,      // +1 attack
//         defenseChange: 1,     // +1 defense
//         magicAttackChange: 1, // +1 magic attack
//         magicDefenseChange: 1, // +1 magic defense
//         speedChange: 100        // +1 speed
//     );
//     Gear BigSword = new Gear(
//         name: "Giant Sword",
//         description: "A sharp sword.",
//         slot: "weapon",
//         healthChange: 30,      // +1 health
//         attackChange: 1,      // +1 attack
//         defenseChange: 40,     // +1 defense
//         magicAttackChange: 1, // +1 magic attack
//         magicDefenseChange: 1, // +1 magic defense
//         speedChange: 100        // +1 speed
//     );

//     Gear shield = new Gear(
//         name: "Shield",
//         description: "A sturdy shield.",
//         slot: "shoulders",
//         healthChange: 0,
//         attackChange: 2,      // +2 attack
//         defenseChange: 3,     // +3 defense
//         magicAttackChange: 0,
//         magicDefenseChange: 0,
//         speedChange: 0
//     );


//     character.LevelUp();

//     // Add items to inventory
//     character.AddItemToInventory(sword);
//     character.AddItemToInventory(shield);
//     character.AddItemToInventory(BigSword);

//     character.MoveAllInventoryToStorage();

//     Console.WriteLine("\n-- Inventory Updated --\n");

//     // Equip sword (removes it from inventory)
//     character.Equip(sword);
//     Console.WriteLine("\nSword equipped.\n");

//     // Equip shield (removes it from inventory)
//     character.Equip(shield);
//     Console.WriteLine("\nShield equipped.\n");

//     character.PrintEquippedItems();
//     Console.WriteLine();
//     character.PrintBaseStats();
//     character.PrintCurrentStats();
//     Console.WriteLine();

//     // Remove sword (returns it to inventory)
//     character.RemoveItem("weapon");
//     Console.WriteLine("\nSword removed from equipment.\n");

//     character.PrintInventory();
//     Console.WriteLine();
//     character.PrintEquippedItems();
//     Console.WriteLine();
//     character.PrintBaseStats();
//     character.PrintCurrentStats();
//     Console.WriteLine();
//     }
// }
