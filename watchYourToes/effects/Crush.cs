using System;

namespace watchYourToes
{
    public class Crush : Effect
    {
        Stats stats = new Stats();
        public Crush(int dmg) : base("Crush", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}