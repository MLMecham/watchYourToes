using System;

namespace watchYourToes
{
    public class Drowning : Effect
    {
        Stats stats = new Stats();
        public Drowning(int dmg) : base("Drowning", dmg)
        {
            Random random = new Random();
            int min = random.Next(1, 5);
            stats.CurrentStats.Health -= Dmg * min;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}