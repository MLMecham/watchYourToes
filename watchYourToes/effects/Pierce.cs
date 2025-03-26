using System;

namespace watchYourToes
{
    public class Pierce : Effect
    {
        Stats stats = new Stats();
        public Pierce(int dmg) : base("Pierce", dmg)
        {
            Console.WriteLine("The moment you move, sharp, unseen needles pierce your skin, embedding deep within. The pain is unbearable, and though you manage to escape the trap's immediate reach, the agony doesn't cease. Every step you take, even as you move through the dungeon, causes the needles to shift and dig deeper, inflicting more damage with each movement. It feels as if the trap's curse is following you, making every step a painful reminder of its sting.");
            stats.CurrentStats.Health -= Dmg;
            stats.CurrentStats.Defense -= 30;
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