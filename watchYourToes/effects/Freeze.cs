using System;

namespace watchYourToes
{
    public class Freeze : Effect
    {
        public Freeze(int dmg) : base("Freeze", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Freezing temperatures create slippery surfaces and slow movement.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Freezing deals {Dmg} damage and reduces the enemy's speed.");
        }
    }
}