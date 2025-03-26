using System;

namespace watchYourToes
{
    public class Burn : Effect
    {
        Stats stats = new Stats();
        public Burn(int dmg) : base("Burn", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
            stats.CurrentStats.Health -= Dmg;
        }
    }
}