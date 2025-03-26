using System;

namespace watchYourToes
{
    public class Freeze : Effect
    {
        Stats stats = new Stats();
        public Freeze(int dmg) : base("Freeze", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
            stats.CurrentStats.Speed -= 30;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}