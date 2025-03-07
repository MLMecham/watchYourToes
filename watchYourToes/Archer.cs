public class Archer : Character
{
    public Archer(string name) : base(name, "Archer")
    {
        // Archer-specific initialization?
    }

    // Overrides the LevelUp method to define Archer-specific logic
    public override void LevelUp()
    {
        base.LevelUp(); // Call the base LevelUp method

        Stats.BaseStats.Speed += 2; // Archer gains more Speed on level-up???
        Console.WriteLine($"{Name} (Archer) leveled up! Speed increased.");
    }

}
