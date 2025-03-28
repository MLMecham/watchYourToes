using System;
using System.Collections.Generic;

class StartRoom : Room
{
    public StartRoom(Character character) : base("The voice of Magar echoes in the room,\n'Find the biggest baddie and bash him in! Only then can you continue into dungeons dim!", character)
    {
    }

    public override void RoomEffect()
    {
        Console.WriteLine($"This is the starting room. Enjoy the dungeon");
    }
}
