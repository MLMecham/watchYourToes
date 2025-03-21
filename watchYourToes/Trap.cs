using System;
using System.Collections.Generic;

class Trap
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string EffectType { get; set; }

    public Trap(string name, string description, string effectType)
    {
        Name = name;
        Description = description;
        EffectType = effectType;
    }
}