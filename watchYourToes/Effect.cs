using System;
using System.Collections.Generic;

namespace watchYourToes;
public class Effect
{
    public string Name { get; set; }
    public int Dmg { get ; set; }

    public Effect(string name, int dmg)
    {
        Name = name;
        Dmg = dmg;
    }
    public virtual void FieldEffect()
    {
    }
    public virtual void BattleEffect()
    {
    }
}