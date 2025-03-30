public class CustomClass : Character
{
    // Constructor to load an existing character from DB
    public CustomClass(Character character) : base(character.Name, character.ClassName )
    {
        this.Id = character.Id;
        this.Level = character.Level;
        this.Exp = character.Exp;
        this.Stats = character.Stats;
        this.Days = character.Days;
        this.Gold = character.Gold;
        
    }

    // Default minimal bonus 3 from Character class


    public override async Task LevelUp()
    {

        Level++;
        Stats.BaseStats.Health += 0;
        Stats.BaseStats.Attack +=0;
        Stats.BaseStats.Defense += 0;
        Stats.BaseStats.MagicAttack += 0; 
        Stats.BaseStats.MagicDefense += 0;
        Stats.BaseStats.Speed += 0;

        Console.WriteLine($"{Name} {ClassName} doesn't level up, sorry.");

        // let the player add stats
        //DistributeExtraPoints();

        // Update the database with new stats
        var db = new dbConnection(); // Ensure you have access to your DB connection
        await db.UpdateCharacter(this);
    }
    
}
