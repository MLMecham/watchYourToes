// using System;

// class Player
// {
//     public Room CurrentRoom { get; private set; }

//     public Player(Room startRoom)
//     {
//         CurrentRoom = startRoom;
//     }

//     public void Move(string direction)
//     {
//         Room? nextRoom = CurrentRoom.GetConnectedRoom(direction);
//         if (nextRoom != null)
//         {
//             CurrentRoom = nextRoom;
//             Console.WriteLine($"You moved {direction}. {CurrentRoom.Description}");
//         }
//         else
//         {
//             Console.WriteLine("You can't go that way!");
//         }

//         if ()
//         {
//             newX = x + 1;
//         }
//         else if (randDir == 2)
//         {
//             newX = x - 1;
//         }
//         else if (randDir == 3)
//         {
//             newY = y + 1;
//         }
//         else if (randDir == 4)
//         {
//             newY = y - 1;
//         }
//     }
// }