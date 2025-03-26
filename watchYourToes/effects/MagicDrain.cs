using System;

namespace watchYourToes
{
    public class MagicDrain : Effect
    {
        Stats stats = new Stats();
        public MagicDrain(int dmg) : base("Magic Drain", dmg)
        {
            stats.CurrentStats.MagicAttack -= Dmg;
            stats.CurrentStats.MagicDefense -= Dmg;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}