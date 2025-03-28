using System;
using System.Collections.Generic;

class EmptyRoom : Room
{
    public EmptyRoom(Character character) : base("An empty room to recover in - sidenote, there is a wierd stain in the corner...", character)
    {
    }

    public override void RoomEffect()
    {
        Console.WriteLine($"This is a rest room. Take some time to recover strength.");
    }
}