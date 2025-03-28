using System;
using System.Collections.Generic;

public class Room
{
    public string Description {get; protected set;}
    public bool roomCompleted;
    public Character character;

    public Room(string description, Character myCharacter)
    {
        character = myCharacter;
        Description = description;
        roomCompleted = false;
    }
}