public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior") { }
    
    public override int BonusPoints { get; } = 4;


    public override void LevelUp()
    {
        base.LevelUp();
        Stats.BaseStats.Attack += 3; // Warrior gains more attack
        Stats.BaseStats.MagicAttack += 0;
        Stats.BaseStats.MagicDefense +=0;
        Stats.BaseStats.Speed += 1;
        Stats.BaseStats.Defense += 2;
        Stats.BaseStats.Health += 5;
        Console.WriteLine($"{Name} (Warrior) leveled up!");
    }
}