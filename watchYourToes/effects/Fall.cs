using System;

namespace watchYourToes
{
    public class Fall : Effect
    {
        Stats stats = new Stats();
        public Fall(int dmg) : base("Fall", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }

        public int SpecialEffect(int Floor)
        {
            Floor = Floor + 10;
            return Floor;
        }
    }
}