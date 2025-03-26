using MongoDB.Bson.Serialization;
public class Consumable : Item
{
    public Stat Effects { get; set; }  // The stats this consumable modifies
    public bool Temporary { get; set; } // Determines if effects are temporary
    public Consumable(string name, string description, Stat effects, bool temporary)
        : base(name, description)
    {
        Effects = effects;
        Temporary = temporary;
    }
    public void PrintConsumable()
    {
        Console.WriteLine($"Consumable: {Name}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Temporary: {Temporary}");
        Effects.PrintStats();
    }
    public void ViewEffects()
    {
        Console.WriteLine("Effects of the Consumable:");
        if (Effects.Health != 0) Console.WriteLine($"Health: {Effects.Health}");
        if (Effects.Attack != 0) Console.WriteLine($"Attack: {Effects.Attack}");
        if (Effects.Defense != 0) Console.WriteLine($"Defense: {Effects.Defense}");
        if (Effects.MagicAttack != 0) Console.WriteLine($"Magic Attack: {Effects.MagicAttack}");
        if (Effects.MagicDefense != 0) Console.WriteLine($"Magic Defense: {Effects.MagicDefense}");
        if (Effects.Speed != 0) Console.WriteLine($"Speed: {Effects.Speed}");
    }
}