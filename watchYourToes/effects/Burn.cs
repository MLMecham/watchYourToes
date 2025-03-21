using System;

namespace watchYourToes
{
    public class Burn : Effect
    {
        public Burn(int dmg) : base("Burn", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Burning causes the environment to smolder, creating hazards.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Burning deals {Dmg} damage over time and reduces the enemy's attack power.");
        }
    }
}