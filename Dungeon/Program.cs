class Program
{
    static void Main()
    {
        Dungeon dungeon = new Dungeon(14);
        dungeon.FindBossRoom(dungeon.startRoom);
        dungeon.SetRooms();
        Console.WriteLine("The voice of Magar echoes in your ears as you enter the dungeon,\n 'Find the biggest baddie and bash him in! Only then can you continue into dungeons dim!");
        while (dungeon.currentCoord != dungeon.bossRoom || dungeon.QuitDungeon == false)
        {
            dungeon.Action();
        }

    }
}
        // Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Console.Clear(); // Clears console for fresh start
        // int mapX = 30; // Map starts at column 30

        // // Draw menu (left side)
        // Console.SetCursorPosition(0, 1);
        // Console.WriteLine("MENU:");
        // Console.SetCursorPosition(0, 2);
        // Console.WriteLine("1. Move");
        // Console.SetCursorPosition(0, 3);
        // Console.WriteLine("2. Inventory");
        // Console.SetCursorPosition(0, 4);
        // Console.WriteLine("3. Exit");

        // // Draw map (right side)
        // Console.SetCursorPosition(mapX, 1);
        // Console.WriteLine("MAP:");
        // for (int i = 0; i < 10; i++)
        // {
        //     Console.SetCursorPosition(mapX, i + 2);
        //     Console.WriteLine("\u2B1C \u2B1C \u2B1C \u2B1C");
        // }

        // Console.SetCursorPosition(0, 15); // Reset cursor so input isn't messy
        // Console.ReadKey();


        // foreach (var entry in dungeon.grid)
        // {
        //     (int x, int y) = entry.Key;  // Get coordinates
        //     Room room = entry.Value;      // Get the room object
        //     Console.WriteLine($"Room at ({x}, {y}): {room.Description}");
        // }
        // dungeon.DisplayMap();

        // // Initialize the matrix with the box character
        // char[,] matrix = new char[10, 10]
        // {
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'},
        //     {'\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C', '\u2B1C'}
        // };

        // // First way: Print matrix with separators
        // for (int i = 0; i < matrix.GetLength(0); i++)
        // {
        //     for (int j = 0; j < matrix.GetLength(1); j++)
        //     {
        //         if (j != 0)
        //         {
        //             Console.Write(" | ");
        //         }
        //         Console.Write(matrix[i, j]);
        //     }
        //     Console.WriteLine();
        // }

        // Console.WriteLine("\n\n");

        // // Second way: Print without separators before first element
        // for (int i = 0; i < matrix.GetLength(0); i++)
        // {
        //     for (int j = 0; j < matrix.GetLength(1); j++)
        //     {
        //         Console.Write(matrix[i, j]);
        //     }
        //     Console.WriteLine();
        // }

        // Console.WriteLine("\u25A0 \n\u2B1C\u2B1C \n\u25A1\n\u2B1B");
        // Console.WriteLine("\u2B1C\u2B1C\u2B1C\n\u2B1C\u2B1B\u2B1C\n\u2B1C\u2B1C\u2B1C");