using System;

namespace watchYourToes
{
    public class Radiation : Effect
    {
        Stats stats = new Stats();
        public Radiation(int dmg) : base("Radiation", dmg)
        {
            Random random = new Random();
            int time = random.Next(1, 6);
            stats.CurrentStats.Health -= Dmg * time;
            Console.WriteLine("A faint, unsettling hum vibrates through the air as an invisible wave of radiation begins to seep into your body. At first, it’s barely noticeable, but with each passing second, the warmth intensifies, draining your energy and clouding your mind. For what feels like {0} seconds, the radiation continues to sap your strength, leaving you feeling weak and disoriented as its effects linger in your body.", time);
        }

        public override void FieldEffect()
        {
        }

        public override void BattleEffect()
        {
        }
    }
}