using System;
using watchYourToes;

namespace watchYourToes
{
    public class Poison : Effect
    {
        Stats stats = new Stats();
        public Poison(int dmg) : base("Poison", dmg)
        {
            Console.WriteLine("The moment the trap activates, a thick, acrid cloud of poison envelops you. You cough violently as the toxic fumes invade your lungs, clouding your senses and draining your energy. Though you manage to move away, the poison lingers in your system, slowly sapping your strength with every step you take, making each movement feel heavier and more taxing as you press on.");
            stats.CurrentStats.Health -= Dmg;
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