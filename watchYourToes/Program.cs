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
        string characterName = ""; // Declaring character  outside the loop


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
                    // string characterName;
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
                                Console.WriteLine($"Character loaded: {myCharacter.Name},Id: {myCharacter.Id}, Class Name: {myCharacter.ClassName}, Level: {myCharacter.Level}");
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
        Console.WriteLine();
        // Console.WriteLine("\nPress SPACE to continue...");
        // while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed

        // Console.Clear(); // Clears the screen before the next scene


        //TEST !!
        // Adding items to inventory test
        Console.WriteLine("\n-- Adding Items to Inventory ... --\n");
        myCharacter.AddItemToInventory(sword);
        myCharacter.AddItemToInventory(shield);
        myCharacter.AddItemToInventory(BigSword);
        await db.UpdateCharacter(myCharacter); //save new info to db
    
        Console.WriteLine("\n-- Level Up Test... --\n");
        //Level Up test
        await myCharacter.LevelUp();
        // Fetch the character again from DB to verify update
        await db.LoadCharacter(myCharacter.Name);
        Console.WriteLine("\nAfter Level Up:");
        myCharacter.PrintBaseStats();
        

        


        Console.WriteLine("\nPress SPACE to continue...");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        

        //=== Intro Sequence after Character Selection ===
        Console.Write("Loading");
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(1000);
            Console.Write(".");
        }

        Thread.Sleep(500);
        Console.Clear();

        Console.WriteLine("Welcome, traveler. A peaceful village lies nestled between towering mountains and endless forests, a sanctuary for adventurers seeking respite from the perils of the world. Yet, under the village walls, whispers speak of a dark dungeon, an ancient ruin filled with treasures, mysteries, and unspeakable dangers.");
        
        
        Console.WriteLine("You, having lived in this village your whole life, never felt the urge to delve into that wretched pit.");  
        Console.WriteLine("Adventurers spoke of its horrors over tankards of ale,");  
        Console.WriteLine("spinning tales of valiant warriors who ventured in, only to return as twisted husks of their former selves.");  
        Console.WriteLine("The dungeon did not just kill—it corrupted, warping even the bravest into mindless horrors.");  
        Console.WriteLine("The village residents knew better than to set foot near its cursed entrance.");  
        Console.WriteLine("And so, you lived in peace, content to let the dungeon remain a nightmare for a future generation."); 
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Until the sky split open.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("A voice, deep and resonant with malice, thundered from the heavens, shaking the very earth beneath your feet.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("\"Foolish mortals! I am Malgor, and your time is mine to command!");  
        Console.WriteLine("In the depths of the dungeon lies that which I seek.");  
        Console.WriteLine("You will retrieve it today… or suffer eternity within my grasp!\"");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed  
        Console.Clear();  
        Console.WriteLine("Darkness swallowed the sky, and then—just as suddenly—it was gone.");  
        Console.WriteLine("The village stood still, breathless, gripped by an invisible force.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed  
        Console.Clear();  
        Console.WriteLine("That night, terror fell upon you all.");  
        Console.WriteLine("Some tried to flee the village, but the roads were blocked, as if an unseen wall pressed down upon them.");  
        Console.WriteLine("The gates of the village, once open and welcoming, now stood firmly closed, no matter how hard the villagers tried to open them.");  
        Console.WriteLine("No one could escape. The air itself felt thick, stifling, as though the world held its breath.");  
        Console.WriteLine("Some locked their doors in a futile attempt to keep the evil at bay.");  
        Console.WriteLine("Others huddled together in the tavern, whispering frantic prayers, their eyes darting nervously toward the door.");  
        Console.WriteLine("It didn’t matter. Death came for everyone. The flames. The shadows. The unearthly wails.");  
        Console.WriteLine("And as midnight approached, the village grew eerily quiet, save for the muffled cries of those who knew the end was near.");  
        Console.WriteLine("At the stroke of midnight, the village began to burn. Not from flames—but from the very air itself, turning to ash.");  
        Console.WriteLine("The shadows moved with a life of their own, creeping into homes, dragging those inside into the darkness.");  
        Console.WriteLine("One by one, the villagers fell to the curse. And then, as if it had all been a dream.");  
  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("You woke up.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("In your bed.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Again.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("You ran into the streets. Others had already gathered, their faces pale, eyes wide with disbelief.");  
        Console.WriteLine("They remembered. Every single person.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.WriteLine("You all waited for the sky to open.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("A voice, deep and resonant with malice, thundered from the heavens, shaking the very earth beneath your feet.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("\"Foolish mortals! I am Malgor, and your time is mine to command!");  
        Console.WriteLine("In the depths of the dungeon lies that which I seek.");  
        Console.WriteLine("You will retrieve it today… or suffer eternity within my grasp!\"");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed  
        Console.Clear();  
        Console.WriteLine("Darkness swallowed the sky, and then—just as suddenly—it was gone.");  
        Console.WriteLine("The village stood still, breathless, gripped by an invisible force.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed  
        Console.Clear();  

// Malgor's speech with added emphasis on stopping the loop
        Console.WriteLine("\"The curse is simple. Retrieve what I seek from the dungeon, and the cycle will end.\"");
        Console.WriteLine("\"Fail, and you will relive this moment, again and again, until the end of time.\"");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed  
        Console.Clear();  

        Console.Clear();  
        Console.WriteLine("The town square became a place of madness.");  
        Console.WriteLine("Some fell to their knees in despair.");  
        Console.WriteLine("Some laughed hysterically, unable to comprehend the horror of it.");  
        Console.WriteLine("Others raged, screaming into the sky, cursing Malgor’s name.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Then night fell.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("And you all died.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Again.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("And again.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("And again.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("The blacksmith, once a steady and calm man, smashed apart his own forge,");  
        Console.WriteLine("declaring he would build a weapon mighty enough to kill a god.");  
        Console.WriteLine("The baker stopped making bread, convinced there was no point in feeding the doomed.");  
        Console.WriteLine("The children no longer played. The elders wept openly.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Some people threw themselves into the dungeon in desperation, hoping to break the cycle.");  
        Console.WriteLine("They never returned. But when the day reset, they were back in the village, their eyes hollow, their bodies shaking.");  
        Console.WriteLine("They would not speak of what they had seen.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("It became clear: there was no escape. Not from the loop. Not from the dungeon.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("Malgor had bound you all to this fate.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear();  
        Console.WriteLine("You had to enter the ruin. You had to retrieve whatever it was he wanted.");  
        Console.WriteLine("No matter how long it took. No matter how many times you perished.");  
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { } // Wait until SPACE is pressed
        Console.Clear(); 
        Console.WriteLine("But how many cycles could you endure before losing yourself completely?");  

        Thread.Sleep(3000); 
        Console.Clear();

        // Start village loop

        // Let player go to shops / save / change class / go to storage / heal
        // Let players go into the dungeon

            // dungeon stuff
            // dungeon stuff
            // dungeon stuff
            // dungeon stuff


        // When the player dies or finishes the dungeon, they wake up at the start of the village loop.

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