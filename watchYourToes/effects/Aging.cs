using System;

namespace watchYourToes
{
    public class Aging : Effect
    {
        public Aging(int dmg) : base("Aging", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Aging slows down movement and reduces effectiveness over time.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Aging deals {Dmg} damage and reduces the enemy's attack speed.");
        }
    }
}