public class Archer : Character
{
    public Archer(string name) : base(name, "Archer") { }
    // Constructor to load an existing character from DB
    public Archer(Character character) : base(character.Name, "Archer")
    {
        this.Id = character.Id;
        this.Level = character.Level;
        this.Exp = character.Exp;
        this.Stats = character.Stats;
        this.Gold = character.Gold;
    }

    public override int BonusPoints { get; } = 4;


    public override async Task LevelUp()
    {
        Level++;
        Stats.BaseStats.Health += 2;
        Stats.BaseStats.Attack +=2;
        Stats.BaseStats.Defense += 1;
        Stats.BaseStats.MagicAttack += 0; 
        Stats.BaseStats.MagicDefense += 0;
        Stats.BaseStats.Speed += 3;

        Console.WriteLine($"{Name} (Archer) leveled up!.");

        // let the player add stats
        DistributeExtraPoints();

        // Update the database with new stats
        var db = new dbConnection(); // Ensure you have access to your DB connection
        await db.UpdateCharacter(this);
    }
    
}
