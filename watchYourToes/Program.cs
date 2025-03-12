using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;

class Program
{
    // static void ShowMenu()
    // {
    //     Console.Clear();
    //     Console.WriteLine("Welcome to the Main Menu!");
    //     Console.WriteLine("1. Option 1");
    //     Console.WriteLine("2. Option 2");
    //     Console.WriteLine("3. Logout");
        
    //     Console.Write("Choose an option: ");
    //     Console.ReadLine();
    // }

    // This method checks if the username exists
    // public static async Task<bool> CheckIfUsernameExists(string username, dbConnection db)
    // {
    //     var user = await db.GetUser(username);
    //     return user != null; // Returns true if the username already exists
    // }

    static async Task Main()
    {
        dbConnection db = new dbConnection(); // Create an instance of dbConnection
        Character myCharacter = null; // Declaring character  outside the loop


        // bool isLoggedIn = false;
        // User currentUser = null; // Store the current logged-in user

        // while (!isLoggedIn)
        // {
        //     Console.WriteLine("1. Create Account");
        //     Console.WriteLine("2. Login");
        //     Console.WriteLine("3. Exit");
        //     Console.Write("Choose an option: ");

        //     string choice = Console.ReadLine();

        //     switch (choice)
        //     {
        //         case "1":
        //             // Create Account
        //             string createUsername;
        //             bool usernameExists;
        //             do
        //             {
        //                 Console.Write("Enter username: ");
        //                 createUsername = Console.ReadLine().ToLower(); // Convert to lowercase

        //                 // Check if the username already exists
        //                 usernameExists = await CheckIfUsernameExists(createUsername, db);
        //                 if (usernameExists)
        //                 {
        //                     Console.WriteLine("Username already exists. Please choose another one.");
        //                 }
        //             } while (usernameExists);

        //             Console.Write("Enter password: ");
        //             string createPassword = Console.ReadLine();

        //             bool userCreated = await db.CreateUser(createUsername, createPassword);
        //             if (userCreated)
        //             {
        //                 Console.WriteLine("Account created successfully!");
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Error creating account.");
        //             }
        //             break;

        //         case "2":
        //             // Login
        //             Console.Write("Enter username: ");
        //             string loginUsername = Console.ReadLine().ToLower(); // Convert to lowercase
        //             Console.Write("Enter password: ");
        //             string loginPassword = Console.ReadLine();

        //             User user = await db.GetUser(loginUsername);

        //             if (user != null && db.VerifyPassword(loginPassword, user.Password))
        //             {
        //                 Console.WriteLine("Login successful!");
        //                 currentUser = user;
        //                 isLoggedIn = true;
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Invalid username or password.");
        //             }
        //             break;

        //         case "3":
        //             // Exit
        //             Console.WriteLine("Exiting...");
        //             return;

        //         default:
        //             Console.WriteLine("Invalid option. Please try again.");
        //             break;
        //     }
        // }

        // Character creation or selection
        bool isCharacterCreatedOrSelected = false;
        while (!isCharacterCreatedOrSelected)
        {
            Console.WriteLine("----WatchYourToes----");
            Console.WriteLine("1. Create New Character");
            Console.WriteLine("2. Select Existing Character");
            Console.WriteLine("3. Exit to Main Menu");
            Console.Write("Choose an option: ");
            string characterChoice = Console.ReadLine();

            switch (characterChoice)
            {
                case "1":
                    // Create a new character
                    string characterName;
                    bool characterExists;
                    do
                    {
                        Console.Write("Enter character name: ");
                        characterName = Console.ReadLine();

                        // Check if the character name already exists in the collection
                        characterExists = await db.CheckIfCharacterExists( characterName);
                        if (characterExists)
                        {
                            Console.WriteLine("Character name already exists. Please choose another name.");
                        }
                    } while (characterExists);

                    Console.Write("Enter character class: ");
                    string characterClass = Console.ReadLine();

                    // Create the character object with default Level and Stats set in the constructor
                    Character newCharacter = new Character(characterName, characterClass);

                    // Save the character to the database
                    bool isSaved = await newCharacter.SaveCharacter();

                    if (isSaved)
                    {
                        Console.WriteLine("Character created and saved successfully!");

                        // Add the character name to the user's list of characters in the database
                        //bool isCharacterAssigned = await db.AssignCharacterToUser(currentUser.Username, newCharacter.Name);
                        // if (isCharacterAssigned)
                        // {
                        //     Console.WriteLine("Character added to user successfully!");
                        // }
                        
                    }
                    else
                    {
                        Console.WriteLine("Failed to save character.");
                        
                    }
                    break;

                case "2":
                    // Show the list of existing characters from the database
                    List<Character> allCharacters = await db.GetAllCharacters();
                    if (allCharacters.Count > 0)
                    {
                        Console.WriteLine("\nSelect a character:");

                        for (int i = 0; i < allCharacters.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {allCharacters[i].Name}");
                        }

                        Console.Write("Choose a character by number: ");
                        string selection = Console.ReadLine();
                        if (int.TryParse(selection, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= allCharacters.Count)
                        {
                            string selectedCharacterName = allCharacters[selectedIndex - 1].Name;
                            Console.Clear();
                            Console.WriteLine($"Character '{selectedCharacterName}' selected.");

                            myCharacter = await db.LoadCharacter(selectedCharacterName); // Load and assing the selected character
                            if (myCharacter != null)
                            {
                                Console.WriteLine($"Character loaded: {myCharacter.Name}, Level: {myCharacter.Level}");
                            }
                            else
                            {
                                Console.WriteLine("Character not found.");
                            }
                    
                            isCharacterCreatedOrSelected = true;

                            
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You don't have any characters. Please create one first.");
                    }
                    break;

                case "3":
                    // Exit to Main Menu
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        //=== ITEMS Creation ===

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


        //=== Character Stats  ===
        Console.WriteLine();
        myCharacter.PrintBaseStats();
        Console.WriteLine();
        myCharacter.PrintEquippedItems();
        Console.WriteLine("\nPress SPACE to continue...");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed

        Console.Clear(); // Clears the screen before the next scene


        //=== Intro Sequence after Character Selection ===
        Console.Write("Loading");
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(1000);
            Console.Write(".");
        }

        Thread.Sleep(500);
        Console.Clear();

        Console.WriteLine("Welcome, traveler. A peaceful village lies nestled between towering mountains and endless forests, a sanctuary for adventurers seeking respite from the perils of the world. Yet, beyond the village walls, whispers speak of a dark dungeon, an ancient ruin filled with treasures, mysteries, and unspeakable dangers.");
        
        Thread.Sleep(3000); 
        Console.Clear();

        

        //TEST 

        // Add items to inventory
        myCharacter.AddItemToInventory(sword);
        myCharacter.AddItemToInventory(shield);
        myCharacter.AddItemToInventory(BigSword);

        myCharacter.MoveAllInventoryToStorage();
        await db.UpdateCharacter(myCharacter);
        Console.WriteLine("\n-- Inventory Updated and Saved --\n");


    }
}




















// // Add items to inventory
// character.AddItemToInventory(sword);
// character.AddItemToInventory(shield);
// character.AddItemToInventory(BigSword);

// character.MoveAllInventoryToStorage();

// Console.WriteLine("\n-- Inventory Updated --\n");

// // Equip sword (removes it from inventory)
// character.Equip(sword);
// Console.WriteLine("\nSword equipped.\n");

// // Equip shield (removes it from inventory)
// character.Equip(shield);
// Console.WriteLine("\nShield equipped.\n");

// character.PrintEquippedItems();
// Console.WriteLine();
// character.PrintBaseStats();
// character.PrintCurrentStats();
// Console.WriteLine();

// // Remove sword (returns it to inventory)
// character.RemoveItem("weapon");
// Console.WriteLine("\nSword removed from equipment.\n");

// character.PrintInventory();
// Console.WriteLine();
// character.PrintEquippedItems();
// Console.WriteLine();
// character.PrintBaseStats();
// character.PrintCurrentStats();
// Console.WriteLine();