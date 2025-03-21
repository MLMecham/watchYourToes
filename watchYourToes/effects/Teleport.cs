using System;

namespace watchYourToes
{
    public class Teleport : Effect
    {
        public Teleport(int dmg) : base("Teleport", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Teleportation creates unpredictable movement patterns.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Teleportation deals {Dmg} damage and disorients the enemy.");
        }
    }
}