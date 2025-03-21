using System;

namespace watchYourToes
{
    public class Pierce : Effect
    {
        public Pierce(int dmg) : base("Pierce", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Piercing traps litter the area, creating hazards.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Piercing deals {Dmg} damage and ignores a portion of the enemy's defense.");
        }
    }
}