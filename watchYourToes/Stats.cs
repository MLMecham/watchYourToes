public class Stats
{
    public Stat BaseStats { get; set; }
    public Stat CurrentStats { get; set; }

    public Stats()
    {
        BaseStats = new Stat();
        CurrentStats = new Stat();
    }

    public void PrintBaseStats()
    {
        Console.WriteLine("Base Stats:");
        BaseStats.PrintStats();
    }

    public void PrintCurrentStats()
    {
        Console.WriteLine("Current Stats:");
        CurrentStats.PrintStats();
    }
}



public class Stat
{
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefense { get; set; }
    public int Speed { get; set; }


    

    public Stat()
    {
        Health = 100;
        Attack = 10;
        Defense = 5;
        MagicAttack = 5;
        MagicDefense = 5;
        Speed = 5;
    }

    public void PrintStats()
    {
        Console.WriteLine($"Health: {Health}");
        Console.WriteLine($"Attack: {Attack}");
        Console.WriteLine($"Defense: {Defense}");
        Console.WriteLine($"Magic Attack: {MagicAttack}");
        Console.WriteLine($"Magic Defense: {MagicDefense}");
        Console.WriteLine($"Speed: {Speed}");
    }

    
}
