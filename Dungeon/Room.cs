using System;
using System.Collections.Generic;

class Room
{
    public string Description {get; protected set;}

    public Room(string description)
    {
        Description = description;
    }
}