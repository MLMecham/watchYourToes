public class Stats
{
    public Stat BaseStats { get; set; }
    public Stat CurrentStats { get; set; }

    public Stats()
    {
        BaseStats = new Stat();
        CurrentStats = new Stat();
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
}
