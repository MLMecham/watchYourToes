public class Archer : Character
{
    public Archer(string name) : base(name, "Archer") { }

    public override int BonusPoints { get; } = 4;


    public override void LevelUp()
    {
        base.LevelUp();
        Stats.BaseStats.Speed += 3; // Archer gains more Agility
        Stats.BaseStats.MagicAttack += 0;
        Stats.BaseStats.MagicDefense+=0;
        Stats.BaseStats.Attack += 2;
        Stats.BaseStats.Defense += 1;
        Stats.BaseStats.Health += 2;
        Console.WriteLine($"{Name} (Archer) leveled up!.");
    }
}
