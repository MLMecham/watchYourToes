using System;

namespace watchYourToes
{
    public class Blind : Effect
    {
        public Blind(int dmg) : base("Blind", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Blindness causes reduced visibility, making navigation harder.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Blindness deals {Dmg} damage and reduces the enemy's accuracy.");
        }
    }
}