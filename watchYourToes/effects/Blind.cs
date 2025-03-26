using System;

namespace watchYourToes
{
    public class Blind : Effect
    {
        Stats stats = new Stats();
        public Blind(int dmg) : base("Blind", dmg)
        {
            stats.CurrentStats.Health -= Dmg;
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
        public override void SpecialEffect()
        {
            Random random = new Random();
            int blind = random.Next(1, 3);
            if (blind > 2)
            {
                
            }
        }
    }
}