using System;

namespace watchYourToes
{
    public class Bleed : Effect
    {
        public Bleed(int dmg) : base("Bleed", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Bleeding leaves a trail, attracting enemies.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Bleeding deals {Dmg} damage over time.");
        }
    }
}