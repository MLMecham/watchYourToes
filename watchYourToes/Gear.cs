public class Gear : Item
{
    public string Slot { get; set; }     // The body part the gear is equipped on (head, shoulders, etc.)
    public int HealthChange { get; set; }        // Stat change for Health
    public int AttackChange { get; set; }        // Stat change for Attack
    public int DefenseChange { get; set; }       // Stat change for Defense
    public int MagicAttackChange { get; set; }   // Stat change for Magic Attack
    public int MagicDefenseChange { get; set; }  // Stat change for Magic Defense
    public int SpeedChange { get; set; }         // Stat change for Speed

    public Gear(string name, string description, string slot, int healthChange = 0, int attackChange = 0,
                int defenseChange = 0, int magicAttackChange = 0, int magicDefenseChange = 0, int speedChange = 0)
        : base(name, description)
    {
        Slot = slot;
        HealthChange = healthChange;
        AttackChange = attackChange;
        DefenseChange = defenseChange;
        MagicAttackChange = magicAttackChange;
        MagicDefenseChange = magicDefenseChange;
        SpeedChange = speedChange;
    }
}
