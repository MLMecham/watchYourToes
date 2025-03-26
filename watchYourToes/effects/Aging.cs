using System;

namespace watchYourToes
{
    public class Aging : Effect
    {
        Stats stats = new Stats();
        public Aging(int dmg) : base("Aging", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
            stats.CurrentStats.Speed -= 20;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}