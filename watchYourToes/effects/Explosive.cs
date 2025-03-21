using System;

namespace watchYourToes
{
    public class Explosive : Effect
    {
        public Explosive(int dmg) : base("Explosive", dmg)
        {
        }

        public override void FieldEffect()
        {
            Console.WriteLine("Explosions leave craters, altering the terrain.");
        }

        public override void BattleEffect()
        {
            Console.WriteLine($"Explosions deal {Dmg} damage to all enemies in the area.");
        }
    }
}