using System;

namespace watchYourToes
{
    public class MagicDrain : Effect
    {
        public MagicDrain(int dmg) : base("Magic Drain", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Magic Drain weakens magical energy in the area.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Magic Drain deals {Dmg} damage and reduces the enemy's magic power.");
        }
    }
}