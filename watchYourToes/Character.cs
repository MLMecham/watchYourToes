using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Threading.Tasks;
using watchYourToes;

public class Character : Combatant

{
    [BsonId] 
    public string Id { get; set; } // _id field as a string
    public string Name { get; set; }
    public string ClassName { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public Stats Stats { get; set; }
    public Equipment Equipment { get; set; } // The Equipment class that holds all slots
    public bool InDungeon { get; set; }  // Whether the character is in the dungeon
    public int Days { get; set; }  // Number of days the character has spent in the dungeon
    public int LowestFloor { get; set; }  // The lowest floor the character has reached
    public int Gold { get; set; }

    // Inventory and storage as lists
    public List<Item> Inventory { get; set; }
    public List<Item> Storage { get; set; }
    public HashSet<Effect> ActiveEffects { get; set; }


    /// MongoDB connection and database
    private static readonly string connectionString = "mongodb+srv://mechamit000:1FhnVwbO6e54fLRa@character.btcp0.mongodb.net/?retryWrites=true&w=majority&appName=character";
    private static readonly MongoClient client = new MongoClient(connectionString);
    private static readonly IMongoDatabase database = client.GetDatabase("touchyourtoes");
    private static readonly IMongoCollection<BsonDocument> charactersCollection = database.GetCollection<BsonDocument>("characters");




    // Virtual property that subclasses can override to define bonus points
    public virtual int BonusPoints { get; } = 3; // Default bonus points (e.g., 3)

    

    public Character(string name, string className )
    {
        Name = name;
        Level = 1;
        Exp = 0;
        Stats = new Stats();
        InDungeon = false;  // Initialize as not in the dungeon
        ClassName = className;
        Days = 0;  // Initialize days to 0
        LowestFloor = 1;  // Initialize lowest floor as the first floor
        Equipment = new Equipment();

        // Initialize inventory and storage as empty lists
        Inventory = new List<Item>();
        Storage = new List<Item>();
        Gold = 0;
        ActiveEffects = new HashSet<Effect>();
    }

      public async Task<bool> SaveCharacter()
{
    try
    {
        // Convert the Character object to a BsonDocument for MongoDB storage
        BsonDocument characterDoc = new BsonDocument
        {
            { "_id", Name },  // Set the _id field to the Name of the character
            { "Name", Name },
            { "ClassName", ClassName },
            { "Level", Level },
            { "Exp", Exp },
            { "Stats", BsonSerializer.Deserialize<BsonDocument>(Stats.ToJson()) },
            { "Equipment", BsonSerializer.Deserialize<BsonDocument>(Equipment.ToJson()) },
            { "InDungeon", InDungeon },
            { "Days", Days },
            { "LowestFloor", LowestFloor },
            { "Gold", Gold },
            { "Inventory", new BsonArray(Inventory.Select(item => BsonSerializer.Deserialize<BsonDocument>(item.ToJson()))) },
            { "Storage", new BsonArray(Storage.Select(item => BsonSerializer.Deserialize<BsonDocument>(item.ToJson()))) }
        };

        // Insert or replace the character in the MongoDB collection
        var filter = Builders<BsonDocument>.Filter.Eq("_id", Name); // Use _id as the unique field now
        var updateOptions = new UpdateOptions { IsUpsert = true }; // This will insert a new document if none exists
        var update = new BsonDocument("$set", characterDoc);

        var result = await charactersCollection.UpdateOneAsync(filter, update, updateOptions); // Use UpdateOneAsync for async operation

        if (result.ModifiedCount > 0 || result.UpsertedId != null)
        {
            Console.WriteLine($"Character {Name} saved successfully.");
            return true;
        }
        else
        {
            Console.WriteLine($"Character {Name} was not saved.");
            return false;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving character {Name}: {ex.Message}");
        return false;
    }
}

   // Take damage method
    public void TakeDamage(int damage)
    {
        Stats.CurrentStats.Health -= damage;
        if (Stats.CurrentStats.Health < 0) Stats.CurrentStats.Health = 0;
        Console.WriteLine($"Character {Name} took {damage} damage! Remaining HP: {Stats.CurrentStats.Health}");
    }
   
    // Base LevelUp method with dynamic bonus points
    public virtual async Task LevelUp()
    {
        double requiredExp = 100 * Math.Pow(Level, 1.5);
        while (Exp >= requiredExp)
        {
            Exp -= (int)requiredExp;
            Level++;

            // Default stat growth
            Stats.BaseStats.Health += 10;
            Stats.BaseStats.Attack += 3;
            Stats.BaseStats.Defense += 2;

            // Allow the player to distribute extra points
            DistributeExtraPoints();

            Stats.CurrentStats.Health  = Stats.BaseStats.Health;
            Stats.CurrentStats.Attack = Stats.BaseStats.Attack;
            Stats.CurrentStats.Defense = Stats.BaseStats.Defense;
            Stats.CurrentStats.MagicAttack = Stats.BaseStats.MagicAttack;
            Stats.CurrentStats.MagicDefense = Stats.BaseStats.MagicDefense;
            Stats.CurrentStats.Speed = Stats.BaseStats.Speed;

            // Get total equipment buffs
            Stat equipmentBonus = Equipment.GetEquipmentStats();
            equipmentBonus.PrintStats();
            Console.ReadLine();

            // Apply equipment buffs after leveling up
            Stats.CurrentStats.Health += equipmentBonus.Health;
            Stats.CurrentStats.Attack += equipmentBonus.Attack;
            Stats.CurrentStats.Defense += equipmentBonus.Defense;
            Stats.CurrentStats.MagicAttack += equipmentBonus.MagicAttack;
            Stats.CurrentStats.MagicDefense += equipmentBonus.MagicDefense;
            Stats.CurrentStats.Speed += equipmentBonus.Speed;

            requiredExp = 100 * Math.Pow(Level, 1.5);
        }
        // Save changes to the database
        dbConnection db = new dbConnection();
        await db.UpdateCharacter(this);
        }

    // Allow user to distribute extra points (using the inherited BonusPoints)
    public void DistributeExtraPoints()
    {
        int pointsToDistribute = BonusPoints; // Get the bonus points from the subclass
        Console.WriteLine($"You have {pointsToDistribute} stat points to distribute.");

        // Example of how to allocate points: Let the user input values for each stat
        while (pointsToDistribute > 0)
{
    Console.WriteLine("Which stat would you like to increase?");
    Console.WriteLine($"1. Health:        {Stats.BaseStats.Health}");
    Console.WriteLine($"2. Attack:        {Stats.BaseStats.Attack}");
    Console.WriteLine($"3. Defense:       {Stats.BaseStats.Defense}");
    Console.WriteLine($"4. Magic Attack:  {Stats.BaseStats.MagicAttack}");
    Console.WriteLine($"5. Magic Defense: {Stats.BaseStats.MagicDefense}");
    Console.WriteLine($"6. Speed:         {Stats.BaseStats.Speed}");

    ConsoleKey choice = Console.ReadKey(true).Key;
    Console.Clear();

    switch (choice)
    {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            Stats.BaseStats.Health += 5; // More than 1 for health
            pointsToDistribute--;
            break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            Stats.BaseStats.Attack++;
            pointsToDistribute--;
            break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            Stats.BaseStats.Defense++;
            pointsToDistribute--;
            break;

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:
            Stats.BaseStats.MagicAttack++;
            pointsToDistribute--;
            break;

        case ConsoleKey.D5:
        case ConsoleKey.NumPad5:
            Stats.BaseStats.MagicDefense++;
            pointsToDistribute--;
            break;

        case ConsoleKey.D6:
        case ConsoleKey.NumPad6:
            Stats.BaseStats.Speed++;
            pointsToDistribute--;
            break;

        default:
            Console.WriteLine("Invalid choice, please select a valid stat.");
            break;
    }
}

            // Stats.CurrentStats.Health  = Stats.BaseStats.Health;
            // Stats.CurrentStats.Attack = Stats.BaseStats.Attack;
            // Stats.CurrentStats.Defense = Stats.BaseStats.Defense;
            // Stats.CurrentStats.MagicAttack = Stats.BaseStats.MagicAttack;
            // Stats.CurrentStats.MagicDefense = Stats.BaseStats.MagicDefense;
            // Stats.CurrentStats.Speed = Stats.BaseStats.Speed;
    }

    public void FullHeal()
{
            Stats.CurrentStats.Health  = Stats.BaseStats.Health;
            Stats.CurrentStats.Attack = Stats.BaseStats.Attack;
            Stats.CurrentStats.Defense = Stats.BaseStats.Defense;
            Stats.CurrentStats.MagicAttack = Stats.BaseStats.MagicAttack;
            Stats.CurrentStats.MagicDefense = Stats.BaseStats.MagicDefense;
            Stats.CurrentStats.Speed = Stats.BaseStats.Speed;

            ReaddEquipmentBonuses();

            ResetActiveEffects();
}

    public void ResetActiveEffects()
    {
        ActiveEffects.Clear();
    }

    public void UseConsumable(Consumable consumable)
    {
        // Check if the consumable is in the player's inventory
        if (Inventory.Contains(consumable))
        {
            // If the consumable has health effects (e.g., healing item)
            if (consumable.Effects.Health > 0 && consumable.Temporary) 
            {
                int maxHealth = Stats.BaseStats.Health; // Maximum possible health
                if (Stats.CurrentStats.Health >= maxHealth)
                {
                    Console.WriteLine("Your health is already full. You cannot use this item.");
                    return;
                }

                // Calculate how much can actually be healed
                int healAmount = Math.Min(consumable.Effects.Health, maxHealth - Stats.CurrentStats.Health);
                Stats.CurrentStats.Health += healAmount;

                Console.WriteLine($"{consumable.Name} healed you for {healAmount} health. You now have {Stats.CurrentStats.Health} HP");
            }
            else if (consumable.Temporary)
            {
                // Apply temporary effects to CurrentStats only
                ApplyTemporaryEffects(consumable);
            }
            else
            {
                // Apply permanent effects to both BaseStats and CurrentStats
                ApplyPermanentEffects(consumable);
            }

            // Remove the consumable from inventory after use
            RemoveItemFromInventory(consumable);

            Console.WriteLine($"{consumable.Name} has been used.");
        }
        else
        {
            Console.WriteLine("You do not have this consumable in your inventory.");
        }
    }

    // Method to apply temporary effects to current stats
    private void ApplyTemporaryEffects(Consumable consumable)
    {
        // Only update CurrentStats, do not modify BaseStats
        Stats.CurrentStats.Attack += consumable.Effects.Attack;
        Stats.CurrentStats.Defense += consumable.Effects.Defense;
        Stats.CurrentStats.MagicAttack += consumable.Effects.MagicAttack;
        Stats.CurrentStats.MagicDefense += consumable.Effects.MagicDefense;
        Stats.CurrentStats.Speed += consumable.Effects.Speed;

        Console.WriteLine($"{consumable.Name} has temporarily boosted your stats.");
    }

    // Method to apply permanent effects to base stats and current stats
    private void ApplyPermanentEffects(Consumable consumable)
    {
        // Update BaseStats with permanent changes
        Stats.BaseStats.Health += consumable.Effects.Health;
        Stats.BaseStats.Attack += consumable.Effects.Attack;
        Stats.BaseStats.Defense += consumable.Effects.Defense;
        Stats.BaseStats.MagicAttack += consumable.Effects.MagicAttack;
        Stats.BaseStats.MagicDefense += consumable.Effects.MagicDefense;
        Stats.BaseStats.Speed += consumable.Effects.Speed;

        // Apply the same changes to CurrentStats
        UpdateCurrentStats();
        
        Console.WriteLine($"{consumable.Name} has permanently boosted your stats.");
    }

    // Method to apply changes to CurrentStats (reflecting BaseStats after permanent changes)
    private void UpdateCurrentStats()
    {
        Stats.CurrentStats.Health = Stats.BaseStats.Health;
        Stats.CurrentStats.Attack = Stats.BaseStats.Attack;
        Stats.CurrentStats.Defense = Stats.BaseStats.Defense;
        Stats.CurrentStats.MagicAttack = Stats.BaseStats.MagicAttack;
        Stats.CurrentStats.MagicDefense = Stats.BaseStats.MagicDefense;
        Stats.CurrentStats.Speed = Stats.BaseStats.Speed;

        ReaddEquipmentBonuses();
    }

    // Equip the item and update stats accordingly
// Equip the item and update stats accordingly
public void Equip(Gear gear)
{
    // Check if the gear exists in inventory before equipping it
    if (!Inventory.Contains(gear))
    {
        Console.WriteLine("This item is not in your inventory.");
        return; // If it's not in the inventory, don't equip it
    }

    // Check if an item is already equipped in the given slot, and if so, remove it first
    if (Equipment.IsSlotOccupied(gear.Slot))
    {
        Gear itemToRemove = Equipment.GetItem(gear.Slot);
        RemoveItem(gear.Slot); // Remove the currently equipped item from equipment
        // Add the removed item back to inventory
        // Inventory.Add(itemToRemove);
    }

    // Equip the new gear to the corresponding slot
    Equipment.Equip(gear);

    // Remove the gear from inventory after equipping it
    Inventory.Remove(gear);

    // Update stats based on the item being equipped
    ApplyStatChanges(gear, isEquipping: true);
}

public void ReaddEquipmentBonuses()
{
    // Get total equipment buffs
    Stat equipmentBonus = Equipment.GetEquipmentStats();

    // Apply equipment buffs
    Stats.CurrentStats.Health += equipmentBonus.Health;
    Stats.CurrentStats.Attack += equipmentBonus.Attack;
    Stats.CurrentStats.Defense += equipmentBonus.Defense;
    Stats.CurrentStats.MagicAttack += equipmentBonus.MagicAttack;
    Stats.CurrentStats.MagicDefense += equipmentBonus.MagicDefense;
    Stats.CurrentStats.Speed += equipmentBonus.Speed;
}

public void RemoveItem(string slot)
{
    // Retrieve the item from the Equipment class (if any)
    Gear itemToRemove = Equipment.GetItem(slot);

    if (itemToRemove != null)
    {
        // Remove the item from the Equipment class
        Equipment.RemoveItem(slot);

        // Revert stats based on the item being removed
        ApplyStatChanges(itemToRemove, isEquipping: false);

        // Add the removed item back to inventory
        Inventory.Add(itemToRemove);

        Console.WriteLine($"{itemToRemove.Name} has been removed from equipment and added back to inventory.");
    }
    else
    {
        Console.WriteLine("No item is equipped in this slot.");
    }
}


    // Method to apply or revert stat changes based on whether the item is being equipped or removed
    private void ApplyStatChanges(Gear gear, bool isEquipping)
    {
        // Determine whether to add or subtract stats based on equipping/removing the gear
        int multiplier = isEquipping ? 1 : -1;

        Stats.CurrentStats.Health += gear.HealthChange * multiplier;
        Stats.CurrentStats.Attack += gear.AttackChange * multiplier;
        Stats.CurrentStats.Defense += gear.DefenseChange * multiplier;
        Stats.CurrentStats.MagicAttack += gear.MagicAttackChange * multiplier;
        Stats.CurrentStats.MagicDefense += gear.MagicDefenseChange * multiplier;
        Stats.CurrentStats.Speed += gear.SpeedChange * multiplier;
    }

    // Add item to inventory
    public void AddItemToInventory(Item item)
    {
        Inventory.Add(item);
        Console.WriteLine($"{item.Name} has been added to your inventory.");
    }

    // Remove item from inventory
    public void RemoveItemFromInventory(Item item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
            Console.WriteLine($"{item.Name} has been removed from your inventory.");
        }
        else
        {
            Console.WriteLine("Item not found in inventory.");
        }
    }

    // Store item in storage (and remove from inventory)
    public void StoreItemInStorage(Item item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
            Storage.Add(item);
            Console.WriteLine($"{item.Name} has been moved to storage.");
        }
        else
        {
            Console.WriteLine("Item not found in inventory.");
        }
    }

    // Retrieve item from storage (and add to inventory)
    public void RetrieveItemFromStorage(Item item)
    {
        if (Storage.Contains(item))
        {
            Storage.Remove(item);
            Inventory.Add(item);
            Console.WriteLine($"{item.Name} has been retrieved from storage.");
        }
        else
        {
            Console.WriteLine("Item not found in storage.");
        }
    }


    public void PrintStorage()
    {
        if (Storage.Count == 0)
        {
            Console.WriteLine("Your storage is empty.");
        }
        else
        {
            Console.WriteLine("Storage Items:");
            for (int i = 0; i < Storage.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Name: {Storage[i].Name}, Description: {Storage[i].Description}");
            }
        }
    }


    public void PrintInventory()
    {
        if (Inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
        }
        else
        {
            Console.WriteLine("Inventory Items:");
            for (int i = 0; i < Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Name: {Inventory[i].Name}, Description: {Inventory[i].Description}");
            }
        }
    }

    // Method to print the equipped items for the character
    public void PrintEquippedItems()
    {
        Console.WriteLine($"{Name}'s Equipped Items:");
        Equipment.PrintEquippedItems();
    }

    public void MoveAllInventoryToStorage()
{
    if (Inventory.Count == 0)
    {
        Console.WriteLine("Your inventory is empty. Nothing to move.");
        return;
    }

    // Move all inventory items to storage
    Storage.AddRange(Inventory);
    Inventory.Clear();
    Console.WriteLine("All items have been moved from inventory to storage.");
}

public void UnequipAllItems()
{
    var equippedItems = Equipment.GetAllEquippedItems(); // Assume this method returns a list of equipped gear

    if (equippedItems.Count == 0)
    {
        Console.WriteLine("No items are equipped.");
        return;
    }

    foreach (var item in equippedItems)
    {
        RemoveItem(item.Slot); // Unequip item and put it back into inventory
    }

    Console.WriteLine("All equipped items have been unequipped and returned to inventory.");
}


public void PrintBaseStats()
{
    Stats.PrintBaseStats();
}

public void PrintCurrentStats()
{
    Stats.PrintCurrentStats();
}

public override void BasicAttack()
{
    // Console.WriteLine("the character doesn't know how to fight!");
}

}
