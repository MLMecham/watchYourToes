using System;

namespace watchYourToes
{
    public class Mental : Effect
    {
        Stats stats = new Stats();
        public Mental(int dmg) : base("Mental", dmg)
        {
            Console.WriteLine("A heavy, unnatural silence surrounds you, and then your mind is flooded with whispers—dark, twisted thoughts clawing at your consciousness. They cloud your judgment, filling you with uncertainty and fear. Images of failure distort your thoughts, making it harder to focus. As the whispers fade and the trap releases its hold, you feel a lingering weakness in your magic, as if the connection to your arcane power has been disturbed, leaving you more vulnerable than before.");
            stats.CurrentStats.Health -= Dmg;
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