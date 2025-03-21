using System;

namespace watchYourToes
{
    public class Mental : Effect
    {
        public Mental(int dmg) : base("Mental", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Mental strain causes confusion, making decisions harder.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Mental strain deals {Dmg} damage and reduces the enemy's focus.");
        }
    }
}