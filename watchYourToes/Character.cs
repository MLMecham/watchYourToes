using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Character
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public Stats Stats { get; set; }
    public Equipment Equipment { get; set; } // The Equipment class that holds all slots
    public bool InDungeon { get; set; }  // Whether the character is in the dungeon
    public int Days { get; set; }  // Number of days the character has spent in the dungeon
    public int LowestFloor { get; set; }  // The lowest floor the character has reached

    // Inventory and storage as lists
    public List<Item> Inventory { get; set; }
    public List<Item> Storage { get; set; }

    // Virtual property that subclasses can override to define bonus points
    public virtual int BonusPoints { get; } = 3; // Default bonus points (e.g., 3)

    

    public Character(string name)
    {
        Name = name;
        Level = 1;
        Exp = 0;
        Stats = new Stats();
        InDungeon = false;  // Initialize as not in the dungeon
        Days = 0;  // Initialize days to 0
        LowestFloor = 1;  // Initialize lowest floor as the first floor
        Equipment = new Equipment();

        // Initialize inventory and storage as empty lists
        Inventory = new List<Item>();
        Storage = new List<Item>();
    }

    // Base LevelUp method with dynamic bonus points
    public virtual void LevelUp()
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

            Stats.CurrentStats.Health = Stats.BaseStats.Health;

            requiredExp = 100 * Math.Pow(Level, 1.5);
        }
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
            Console.WriteLine("1. Health");
            Console.WriteLine("2. Attack");
            Console.WriteLine("3. Defense");
            Console.WriteLine("4. Magic Attack");
            Console.WriteLine("5. Magic Defense");
            Console.WriteLine("6. Speed");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Stats.BaseStats.Health += 5;  // Set health bonus should be more than one
                    pointsToDistribute--;
                    break;
                case 2:
                    Stats.BaseStats.Attack++;
                    pointsToDistribute--;
                    break;
                case 3:
                    Stats.BaseStats.Defense++;
                    pointsToDistribute--;
                    break;
                case 4:
                    Stats.BaseStats.MagicAttack++;
                    pointsToDistribute--;
                    break;
                case 5:
                    Stats.BaseStats.MagicDefense++;
                    pointsToDistribute--;
                    break;
                case 6:
                    Stats.BaseStats.Speed++;
                    pointsToDistribute--;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
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
        Inventory.Add(itemToRemove);
    }

    // Equip the new gear to the corresponding slot
    Equipment.Equip(gear);

    // Remove the gear from inventory after equipping it
    Inventory.Remove(gear);

    // Update stats based on the item being equipped
    ApplyStatChanges(gear, isEquipping: true);
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
            foreach (var item in Storage)
            {
                Console.WriteLine($"Name: {item.Name}, Description: {item.Description}");
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
            foreach (var item in Inventory)
            {
                Console.WriteLine($"Name: {item.Name}, Description: {item.Description}");
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

    

    


    



    
}
