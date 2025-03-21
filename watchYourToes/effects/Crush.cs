using System;

namespace watchYourToes
{
    public class Crush : Effect
    {
        public Crush(int dmg) : base("Crush", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Crushing debris blocks paths, making movement difficult.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Crushing deals {Dmg} damage and stuns the enemy briefly.");
        }
    }
}