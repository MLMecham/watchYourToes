using System;

namespace watchYourToes
{
    public class Bleed : Effect
    {
        Stats stats = new Stats();
        public Bleed(int dmg) : base("Bleed", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void FieldEffect()
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void BattleEffect()
        {
        }
    }
}