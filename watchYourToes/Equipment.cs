public class Equipment
{
    public Gear Head { get; set; }
    public Gear Shoulders { get; set; }
    public Gear Knees { get; set; }
    public Gear Toes { get; set; }
    public Gear Weapon { get; set; }

    public Equipment()
    {
        Head = null;
        Shoulders = null;
        Knees = null;
        Toes = null;
        Weapon = null;
    }

    // Method to equip an item to a specific slot
    public void Equip(Gear item)
    {
        switch (item.Slot.ToLower())
        {
            case "head":
                Head = item;
                break;
            case "shoulders":
                Shoulders = item;
                break;
            case "knees":
                Knees = item;
                break;
            case "toes":
                Toes = item;
                break;
            case "weapon":
                Weapon = item;
                break;
            default:
                Console.WriteLine("Invalid slot.");
                break;
        }
    }

    // Method to remove an item from a specific slot
    public void RemoveItem(string slot)
    {
        switch (slot.ToLower())
        {
            case "head":
                Head = null;
                break;
            case "shoulders":
                Shoulders = null;
                break;
            case "knees":
                Knees = null;
                break;
            case "toes":
                Toes = null;
                break;
            case "weapon":
                Weapon = null;
                break;
            default:
                Console.WriteLine("Invalid slot.");
                break;
        }
    }

    // Check if a slot is occupied
    public bool IsSlotOccupied(string slot)
    {
        switch (slot.ToLower())
        {
            case "head":
                return Head != null;
            case "shoulders":
                return Shoulders != null;
            case "knees":
                return Knees != null;
            case "toes":
                return Toes != null;
            case "weapon":
                return Weapon != null;
            default:
                return false;
        }
    }

    // Get the item from a specific slot
    public Gear GetItem(string slot)
    {
        switch (slot.ToLower())
        {
            case "head":
                return Head;
            case "shoulders":
                return Shoulders;
            case "knees":
                return Knees;
            case "toes":
                return Toes;
            case "weapon":
                return Weapon;
            default:
                return null;
        }
    }

    public void PrintEquippedItems()
{
    // Print each equipped item, or indicate if the slot is empty
    Console.WriteLine("Equipped Items:");

    Console.WriteLine($"Head: {(Head != null ? Head.Name : "Empty")}");
    Console.WriteLine($"Shoulders: {(Shoulders != null ? Shoulders.Name : "Empty")}");
    Console.WriteLine($"Knees: {(Knees != null ? Knees.Name : "Empty")}");
    Console.WriteLine($"Toes: {(Toes != null ? Toes.Name : "Empty")}");
    Console.WriteLine($"Weapon: {(Weapon != null ? Weapon.Name : "Empty")}");
}
}
