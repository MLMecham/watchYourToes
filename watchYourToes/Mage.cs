public class Mage : Character
{
    // Calls the base class constructor with the name and class name
    public Mage(string name) : base(name, "Mage")
    {
    }
    // Constructor to load an existing character from DB
    public Mage(Character character) : base(character.Name, "Mage")
    {
        this.Id = character.Id;
        this.Level = character.Level;
        this.Exp = character.Exp;
        this.Stats = character.Stats;
    }

    public override int BonusPoints { get; } = 5; // Mages get 5 bonus points


    public override async Task LevelUp()
    {
        // base.LevelUp(); /// I believe that this base part doesn't do anything

        Level++; //We forgot to add this so the characters were gaining stats, but staying at level one. Infinite stat glitch.



        // Let's always have the stats increase in this order. Health, Atk, Def, Matk, Mdef, Spd. This will make it easier for us later.
        Stats.BaseStats.Health += 1;
        Stats.BaseStats.Attack +=0;
        Stats.BaseStats.Defense += 1;
        Stats.BaseStats.MagicAttack += 3; // Mage gains more Magic Attack
        Stats.BaseStats.MagicDefense += 3;
        Stats.BaseStats.Speed += 1;
        
        Console.WriteLine($"{Name} (Mage) leveled up!");

        // let the player add stats
        DistributeExtraPoints();


        // Update the database with new stats
        var db = new dbConnection(); // Ensure you have access to your DB connection
        await db.UpdateCharacter(this);
    }

    public override void BasicAttack()
    {
        // Console.WriteLine("The mage flips a table.");
    }
}