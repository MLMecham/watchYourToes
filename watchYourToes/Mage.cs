public class Mage : Character
{
    // Calls the base class constructor with the name and class name
    public Mage(string name) : base(name, "Mage")
    {
    }

    public override void LevelUp()
    {
        base.LevelUp(); // Call the base LevelUp method
        Stats.BaseStats.MagicAttack += 3; // Mage gains more Magic Attack on level-up
        Console.WriteLine($"{Name} (Mage) leveled up! Magic Attack increased.");
    }
}
