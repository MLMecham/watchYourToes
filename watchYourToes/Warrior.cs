public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior")
    {
    }

    // Override the LevelUp method to define Warrior-specific logic
    public override void LevelUp()
    {
        base.LevelUp(); // Call the base LevelUp method
        Stats.BaseStats.Health += 10; // Warrior gains more Health on level-up
        Stats.BaseStats.Attack += 5; // Warrior gains more Attack on level-up
        Console.WriteLine($"{Name} (Warrior) leveled up! Health and Attack increased.");
    }

}
