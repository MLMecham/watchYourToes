using System;

namespace watchYourToes
{
    public class Radiation : Effect
    {
        public Radiation(int dmg) : base("Radiation", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Radiation spreads, causing environmental damage over time.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Radiation deals {Dmg} damage and weakens the enemy's defense.");
        }
    }
}