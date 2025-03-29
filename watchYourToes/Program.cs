using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.IO;

class Program
{
    
    static void RegisterDiscriminators()
    {
        // Register base class 'Item' with a discriminator
        BsonClassMap.RegisterClassMap<Item>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("Item");
        });
        // Register subclass 'Consumable' with its own discriminator
        BsonClassMap.RegisterClassMap<Consumable>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("Consumable");
        });
        // Similarly, register other subclasses like Gear, etc.
        BsonClassMap.RegisterClassMap<Gear>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("Gear");
        });
    }

    static async Task Main()
    {

        
        RegisterDiscriminators();
        dbConnection db = new dbConnection(); // Create an instance of dbConnection
        Character myCharacter = null; // Declaring character  outside the loop
        string characterName = ""; // Declaring character  outside the loop


        
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
                                myCharacter.FullHeal();
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



        

        // Grab the Json data and store them into static variables.
         string filePath = "EnemyTable.json";  // Path to the enemy json file
         string filePathGear = "GearTable.json";
         string filePathConsumable = "ConsumableTable.json";


        // Load Consumables from JSON
        JsonManager.LoadConsumablesFromJson(filePathConsumable);
        Consumable randomConsumable = JsonManager.GetRandomConsumable();
        if (randomConsumable != null)
        {
            Console.WriteLine("Randomly Selected Consumable:");
            randomConsumable.PrintConsumable();
        }
        Console.ReadLine();
        myCharacter.AddItemToInventory(randomConsumable);
        randomConsumable = JsonManager.GetRandomConsumable();
        if (randomConsumable != null)
        {
            Console.WriteLine("Randomly Selected Consumable:");
            randomConsumable.PrintConsumable();
        }
        Console.ReadLine();
        myCharacter.AddItemToInventory(randomConsumable);
        randomConsumable = JsonManager.GetRandomConsumable();
        if (randomConsumable != null)
        {
            Console.WriteLine("Randomly Selected Consumable:");
            randomConsumable.PrintConsumable();
        }
        Console.ReadLine();
        myCharacter.AddItemToInventory(randomConsumable);
        randomConsumable = JsonManager.GetRandomConsumable();
        if (randomConsumable != null)
        {
            Console.WriteLine("Randomly Selected Consumable:");
            randomConsumable.PrintConsumable();
        }
        Console.ReadLine();
        myCharacter.AddItemToInventory(randomConsumable);


        // Load Gear from Specified file
        JsonManager.LoadGearFromJson(filePathGear);
        Gear randomgear = JsonManager.GetRandomGear();
        myCharacter.AddItemToInventory(randomgear);
        myCharacter.PrintInventory();

        // Load enemies from the specified file
        // DO NOT DELETE THIS LINE. THIS IS WHAT LET"S US USE THE STATIC JSON LIST
        JsonManager.LoadEnemiesFromJson(filePath);
        // JsonManager.PrintAllEnemies();
        Enemy randomdude = JsonManager.GetRandomEnemy(5);
        Console.WriteLine(randomdude.Name);
        randomdude.CurrentStat.PrintStats();
        Console.ReadLine();

        


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
            defenseChange: 50,     // +3 defense
            magicAttackChange: 0,
            magicDefenseChange: 50,
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


        // Create some testing items:
        // Strength Potion (Permanent)
        Consumable strengthPotionPermanent = new Consumable(
            "Strength Potion", 
            "A potion that permanently increases attack by 5.", 
            new Stat(attack: 5), // Only set attack to 5, others are 0 by default
            false // Permanent effect
        );

        // Strength Potion (Temporary)
        Consumable strengthPotionTemporary = new Consumable(
            "Strength Potion (Temporary)", 
            "A potion that temporarily increases attack by 50.", 
            new Stat(attack: 300), // Only set attack to 50, others are 0 by default
            true // Temporary effect
        );

        Consumable defensePotionPermanent = new Consumable(
            "Defense Potion", 
            "A potion that permanently increases defense by 5.", 
            new Stat(defense: 5), // Only set defense to 5, others are 0 by default
            false // Permanent effect
        );

        // Defense Potion (Temporary)
        Consumable defensePotionTemporary = new Consumable(
            "Defense Potion (Temporary)", 
            "A potion that temporarily increases defense by 50.", 
            new Stat(defense: 50), // Only set defense to 50, others are 0 by default
            true // Temporary effect
        );

        // Bandage (Healing)
        Consumable bandage = new Consumable(
            "Bandage", 
            "A bandage that heals 50 health.", 
            new Stat(health: 50), // Only set health to 50, others are 0 by default
            true // Temporary healing effect
        );



        
    

        //TEST !!
        // Adding items to inventory test
        Console.WriteLine("\n-- Adding Items to Inventory ... --\n");
        myCharacter.AddItemToInventory(sword);
        myCharacter.AddItemToInventory(shield);
        myCharacter.AddItemToInventory(bandage);
        myCharacter.AddItemToInventory(defensePotionTemporary);
        myCharacter.AddItemToInventory(defensePotionPermanent);
        myCharacter.AddItemToInventory(strengthPotionTemporary);
        myCharacter.AddItemToInventory(strengthPotionPermanent);
        myCharacter.Equip(sword);
        myCharacter.PrintBaseStats();
        myCharacter.PrintCurrentStats();
        Console.ReadLine();
        // await db.UpdateCharacter(myCharacter); //save new info to db
    
        Console.WriteLine("\n-- Level Up Test... --\n");
        //Level Up test
        await myCharacter.LevelUp();
        // Fetch the character again from DB to verify update
        await db.LoadCharacter(myCharacter.Name);
        Console.WriteLine("\nAfter Level Up:");
        myCharacter.PrintBaseStats();


        //BATTLE TEST!!!!!
       
        // Console.WriteLine("\n-- Battle Test... --\n");
        // List<Enemy> enemies = new List<Enemy>
        // {
        //     new Enemy("Goblin", 10, 5, 2, 0, 1, 3, 10, new List<Gear>(), 0.5f, 1),
        //     new Enemy("Orc", 20, 8, 5, 0, 2, 100, 20, new List<Gear>(), 0.5f, 1),
        //     new Enemy("Skeleton", 15, 6, 3, 0, 1, 4, 15, new List<Gear>(), 0.5f, 1)
        // };

        // // Start the battle
        // Battle battle = new Battle(myCharacter, enemies);
        // await battle.StartBattle();



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

        Thread.Sleep(200); 
        Console.Clear();

        


        while (true)
{
    Console.Clear();
    myCharacter.TakeDamage(30);
    Console.WriteLine($"Day {myCharacter.Days}: You wake up to another day in the village.");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. See Stats");
    Console.WriteLine("2. See Inventory");
    Console.WriteLine("3. See Storage");
    Console.WriteLine("4. Visit Store");
    Console.WriteLine("5. See Equipment");
    Console.WriteLine("6. Enter Dungeon");
    Console.WriteLine("7. Rest (Full Heal / End Day)");

    ConsoleKey choice = Console.ReadKey(true).Key;
    Console.Clear();

    switch (choice)
    {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            myCharacter.PrintBaseStats();
            myCharacter.PrintCurrentStats();
            break;

        case ConsoleKey.D2: 
        case ConsoleKey.NumPad2:
            while (true)
    {
        Console.Clear();
        myCharacter.PrintInventory(); // Show inventory

        Console.WriteLine("\nSelect an action:");
        Console.WriteLine("E - Equip Gear");
        Console.WriteLine("S - Send to Storage");
        Console.WriteLine("U - Use Consumable");
        Console.WriteLine("Enter - Exit Inventory");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Enter)
            break;

        Console.Clear();
        myCharacter.PrintInventory(); // Refresh inventory display

        Console.Write("\nEnter item number (1-N) or 0 to cancel: ");
        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= myCharacter.Inventory.Count)
        {
            Item selectedItem = myCharacter.Inventory[itemIndex - 1];

            if (keyInfo.Key == ConsoleKey.E) // Equip Item
            {
                if (selectedItem is Gear gearItem)
                {
                    myCharacter.Equip(gearItem);
                    Console.WriteLine($"{gearItem.Name} equipped!");
                }
                else
                {
                    Console.WriteLine("You can only equip gear items.");
                }
            }
            else if (keyInfo.Key == ConsoleKey.S) // Store Item
            {
                myCharacter.StoreItemInStorage(selectedItem);
            }
            else if (keyInfo.Key == ConsoleKey.U) // Use Consumable
            {
                if (selectedItem is Consumable consumableItem)
                {
                    myCharacter.UseConsumable(consumableItem);
                }
                else
                {
                    Console.WriteLine("You can only use consumable items.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
    break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            while (true)
            {
                Console.Clear();
                myCharacter.PrintStorage(); // Show storage

                if (myCharacter.Storage.Count == 0)
                {
                    Console.WriteLine("\nStorage is empty. Press any key to return...");
                    Console.ReadKey(true);
                    break;
                }
                
            
                Console.Write("\nEnter nothing to Exit.\nEnter item number (1-N) to retrieve or 0 to cancel: ");
                if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= myCharacter.Storage.Count)
                {
                    Item selectedItem = myCharacter.Storage[itemIndex - 1];

                    myCharacter.Storage.Remove(selectedItem);
                    myCharacter.Inventory.Add(selectedItem);

                    Console.WriteLine($"{selectedItem.Name} has been moved to inventory!");
                }
                else if (itemIndex == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
            break;

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            Console.WriteLine("not built yet"); // Placeholder for store functionality
            break;

        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
            while (true)
            {
                Console.Clear();
                myCharacter.PrintEquippedItems(); // Display all equipped items

                // Show total equipment stat changes
                Stat totalChanges = myCharacter.Equipment.GetEquipmentStats();
                Console.WriteLine("\nTotal Equipment Stat Changes:");
                totalChanges.PrintStats();

                Console.WriteLine("\nPress 1-5 to view specific item stats, or press Enter to exit.");
                Console.WriteLine("1 - Head, 2 - Shoulders, 3 - Knees, 4 - Toes, 5 - Weapon");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                    break;

                string slot = keyInfo.Key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => "head",
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => "shoulders",
                    ConsoleKey.D3 or ConsoleKey.NumPad3 => "knees",
                    ConsoleKey.D4 or ConsoleKey.NumPad4 => "toes",
                    ConsoleKey.D5 or ConsoleKey.NumPad5 => "weapon",
                    _ => null
                };

                if (slot != null)
                {
                    Gear item = myCharacter.Equipment.GetItem(slot);
                    if (item != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Item: {item.Name}");
                        Console.WriteLine($"Description: {item.Description}");
                        Console.WriteLine($"Health Change: {item.HealthChange}");
                        Console.WriteLine($"Attack Change: {item.AttackChange}");
                        Console.WriteLine($"Defense Change: {item.DefenseChange}");
                        Console.WriteLine($"Magic Attack Change: {item.MagicAttackChange}");
                        Console.WriteLine($"Magic Defense Change: {item.MagicDefenseChange}");
                        Console.WriteLine($"Speed Change: {item.SpeedChange}");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("No item equipped in this slot.");
                    }
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey(true);
                }
            }
            break;

        case ConsoleKey.D6:
        case ConsoleKey.NumPad6:
            if (ConfirmAction("Are you sure you want to enter the dungeon?"))
            {
                // Prompt the player to select a floor number
                Console.WriteLine($"There are {myCharacter.LowestFloor} floors in the dungeon.");
                Console.WriteLine("Which floor would you like to enter? (1 to " + myCharacter.LowestFloor + ")");
                
                int chosenFloor;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out chosenFloor) && chosenFloor >= 1 && chosenFloor <= myCharacter.LowestFloor)
                    {
                        break; // Exit the loop if a valid floor is selected
                    }
                    else
                    {
                        Console.WriteLine("Invalid floor. Please enter a number between 1 and " + myCharacter.LowestFloor + ".");
                    }
                }

                // Dungeon start_dungeon = new Dungeon(myCharacter, chosenFloor);
                // start_dungeon.FindBossRoom(start_dungeon.startRoom);
                // start_dungeon.SetRooms();
                // Console.WriteLine("The voice of Malgor echoes in your ears as you enter the dungeon,\n 'Find the biggest baddie and bash him in! Only then can you continue into dungeons dim!");
                // while (start_dungeon.currentCoord != start_dungeon.bossRoom || start_dungeon.QuitDungeon == false)
                // {
                //     start_dungeon.Action();
                // }
                
                Dungeon dungeon = new Dungeon(myCharacter, chosenFloor);
                dungeon.FindBossRoom(dungeon.startRoom);
                dungeon.SetRooms();

                while (true)
                {
                    int actionResult =dungeon.Action();
                    if (actionResult == 1) //Deeper in the dungeon
                    {
                        dungeon = new Dungeon(myCharacter, chosenFloor++);
                        dungeon.FindBossRoom(dungeon.startRoom);
                        dungeon.SetRooms();
                    }
                    else if (actionResult == 2) //Back out of the dungeon
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }
                    
                }
            }
            break;

        case ConsoleKey.D7:
        case ConsoleKey.NumPad7:
            Console.WriteLine("You decide to rest and prepare for another loop.");
            myCharacter.FullHeal();
            myCharacter.Days++;


            // For now, you level up when you do this
            await myCharacter.LevelUp();
            break;

        default:
            Console.WriteLine("Invalid choice. Please select a valid option.");
            break;
    }

    Console.WriteLine("\nPress SPACE to continue...");
    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
}

// Function to confirm major actions
bool ConfirmAction(string message)
{
    Console.WriteLine(message + " (Y/N)");
    ConsoleKey response = Console.ReadKey(true).Key;
    return response == ConsoleKey.Y;
}


        
        

       

    }
}



