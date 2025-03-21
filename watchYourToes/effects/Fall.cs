using System;

namespace watchYourToes
{
    public class Fall : Effect
    {
        public Fall(int dmg) : base("Fall", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Falling creates dangerous pits, making traversal risky.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Falling deals {Dmg} damage and knocks the enemy down.");
        }
    }
}