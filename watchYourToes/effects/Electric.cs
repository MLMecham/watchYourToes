using System;

namespace watchYourToes
{
    public class Electric : Effect
    {
        Stats stats = new Stats();
        public Electric(int dmg) : base("Explosive", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
            stats.CurrentStats.MagicAttack -= 10;
            stats.CurrentStats.MagicDefense -= 10;
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