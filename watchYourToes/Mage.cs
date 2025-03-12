public class Mage : Character
{
    // Calls the base class constructor with the name and class name
    public Mage(string name) : base(name, "Mage")
    {
    }
    
    public override int BonusPoints { get; } = 5; // Mages get 5 bonus points


    public override void LevelUp()
    {
        base.LevelUp();
        Stats.BaseStats.MagicAttack += 3; // Mage gains more Magic Attack
        Stats.BaseStats.MagicDefense += 3;
        Stats.BaseStats.Speed += 1;
        Stats.BaseStats.Defense += 1;
        Stats.BaseStats.Health += 1;
        Stats.BaseStats.Attack +=0;
        Console.WriteLine($"{Name} (Mage) leveled up!");
    }
}