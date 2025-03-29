public class Ninja : Character
{
    public Ninja(string name) : base(name, "Ninja") { }
    // Constructor to load an existing character from DB
    public Ninja(Character character) : base(character.Name, "Ninja")
    {
        this.Id = character.Id;
        this.Level = character.Level;
        this.Exp = character.Exp;
        this.Stats = character.Stats;
        this.Days = character.Days;
    }

    public override int BonusPoints { get; } = 5;


    public override async Task LevelUp()
    {
        Level++;
        Stats.BaseStats.Health += 1;
        Stats.BaseStats.Attack +=3;
        Stats.BaseStats.Defense += 3;
        Stats.BaseStats.MagicAttack += 0; 
        Stats.BaseStats.MagicDefense += 0;
        Stats.BaseStats.Speed += 4;

        Console.WriteLine($"{Name} (Ninja) leveled up!.");

        // let the player add stats
        DistributeExtraPoints();

        // Update the database with new stats
        var db = new dbConnection(); // Ensure you have access to your DB connection
        await db.UpdateCharacter(this);
    }
    
}
