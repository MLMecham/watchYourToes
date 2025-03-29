public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior") { }

    // Constructor to load an existing character from DB
    public Warrior(Character character) : base(character.Name, "Warrior")
    {
        this.Id = character.Id;
        this.Level = character.Level;
        this.Exp = character.Exp;
        this.Stats = character.Stats;
        this.Days = character.Days;
    }

    public override int BonusPoints { get; } = 6;

    public override async Task LevelUp()
    {
        Level++;

        Stats.BaseStats.Health += 5;
        Stats.BaseStats.Attack +=3;
        Stats.BaseStats.Defense += 2;
        Stats.BaseStats.MagicAttack += 0; 
        Stats.BaseStats.MagicDefense += 0;
        Stats.BaseStats.Speed += 1;
        
        Console.WriteLine($"{Name} (Warrior) leveled up!");
        // let the player add stats
        DistributeExtraPoints();
        // Save the updated character to the database
        var db = new dbConnection();
        await db.UpdateCharacter(this);
    }
}
