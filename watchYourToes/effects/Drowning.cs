using System;

namespace watchYourToes
{
    public class Drowning : Effect
    {
        public Drowning(int dmg) : base("Drowning", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Drowning causes the environment to become waterlogged, slowing movement.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Drowning deals {Dmg} damage and stuns the enemy temporarily.");
        }
    }
}