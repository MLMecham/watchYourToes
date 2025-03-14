using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
class Dungeon
{
    public Dictionary<(int,int),Room> grid = new Dictionary<(int, int), Room>();
    Random rand = new Random();

    public (int, int) startRoom;

    public (int,int) bossRoom;
    public int randomNumber;

    public (int,int) currentCoord;

    public bool QuitDungeon = false;


    public int Length; //Length of the grid
    public int Width; //Width of the grid
    public int RoomCount; //Amount of rooms to be made in the grid
    public int Floor; //The floor that the player is on.

    public Dungeon(int floor = 1, int roomCount = 10, int length = 10, int width = 10)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Length = length;
        Width = width;
        RoomCount = roomCount;
        Floor = floor;
        int i = Floor;
        for (; i > 1; i-=2)
        {
            randomNumber = rand.Next(1, 11);
            if (randomNumber > 2)
            {
                RoomCount++;
            }
        }
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        int x = rand.Next(1, Width + 1);
        int y = rand.Next(1, Length + 1);


        grid[(x,y)] = new StartRoom();
        startRoom = (x, y);
        currentCoord = startRoom;

        
        while (grid.Count < RoomCount)
        {
            var randomKey = grid.Keys.ElementAt(rand.Next(grid.Count));

            // Console.WriteLine("passing checkpoint");
            // Console.WriteLine(grid.Count);
            // Console.WriteLine(randomKey);
            AddRoomToGrid(randomKey.Item1,randomKey.Item2);
        
        }
        // After the loop ends
        // if (grid.Count == RoomCount)
        // {
        //     Console.WriteLine("Dungeon completed!");
        // }
        // else
        // {
        //     Console.WriteLine($"Dungeon generation failed with {grid.Count} rooms.");
        // }
    }
    private void AddRoomToGrid(int x, int y)
    {
        //Base Case
        if (grid.Count >= RoomCount) return;
        if (!InGrid(x, y, Length, Width)) return;

        int randDir = rand.Next(1, 5);
        int newX = x;
        int newY = y;

        if (randDir == 1) newX = x + 1;
        
        else if (randDir == 2) newX = x - 1;

        else if (randDir == 3) newY = y + 1;

        else newY = y - 1;
        
        // Console.WriteLine($"{newX}, {newY}");


        if (InGrid(newX, newY, Length, Width) && !grid.ContainsKey((newX, newY)))
        {
            grid[(newX, newY)] = new Room("Magar doesn't want you to see this");

            var randomRoom = grid.Keys.ElementAt(rand.Next(grid.Count));

            // Console.WriteLine($"{newX}, {newY}");
            AddRoomToGrid(randomRoom.Item1,randomRoom.Item2);
            // return;
        }
        else
        {
            return;
        }
    }
    
    public bool InGrid (int x, int y, int length, int height)
    {
        if (x <= Length && x >= 1 && y <= Width && y >= 1)
            return true;
        return false;
    }
    public void FindBossRoom((int, int) startRoom)
    {
        int highest = 0;
        foreach (var entry in grid)
        {
            (int x, int y) = entry.Key;  // Get coordinates
            int distance = Math.Abs(startRoom.Item1 - entry.Key.Item1) + Math.Abs(startRoom.Item2 - entry.Key.Item2);
            if (distance > highest)
            {
                highest = distance;
                bossRoom = (entry.Key.Item1, entry.Key.Item2);
            }
        }
        grid[bossRoom] = new BossRoom();
    }
    public void SetRooms()
    {
        foreach (var entry in grid)
        {
            if (entry.Value is not StartRoom && entry.Value is not BossRoom)
            {
                double chance = rand.NextDouble();
                
                if (chance < 0.55)
                {
                    grid[entry.Key] = new EnemyRoom();
                }
                else if (chance < 0.75)
                {
                    grid[entry.Key] = new TrapRoom();
                }
                else if (chance < 0.95)
                {
                    grid[entry.Key] = new EmptyRoom();
                }
                else
                {
                    grid[entry.Key] = new TreasureRoom();
                }
            }
        }
    }
    public void DisplayMap((int, int) currentCoord)
    {
        
        string[,] matrix = new string[10, 10]
        {
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"},
            {"\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B", "\u2B1B"}
        };
        for (int y = 1; y < Width + 1; y++)
        {
            for (int x = 1; x < Length + 1; x++)
            {
                if (grid.ContainsKey((x, y)))
                {
                    if ((x,y) == currentCoord)
                    {
                        matrix[y - 1, x - 1] = "\u2592\u2592";
                    }
                    else if (grid[(x, y)] is BossRoom)
                    {
                        matrix[y - 1, x - 1] = "\u2592\u2592";
                    }
                    else if (grid[(x,y)] is StartRoom)
                    {
                        matrix[y - 1, x - 1] = "\u2592\u2592";
                    }
                    else
                    {
                        matrix[y - 1, x - 1] = "\u2B1C";
                    }
                }
                
                // matrix[y - 1, x - 1] = "\u2592\u2592";
            }
        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if ((j + 1, i + 1) == currentCoord) // Correcting coordinates
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(matrix[i, j]); // Print current location purple
                    Console.ResetColor();
                }
                else if ((j + 1, i + 1) == bossRoom) // Correcting coordinates
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(matrix[i, j]); // Print boss room in red
                    Console.ResetColor();
                }
                else if ((j + 1, i + 1) == startRoom) // Correcting coordinates
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(matrix[i, j]); // Print start room blue
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(matrix[i, j]);
                }
            }
            Console.WriteLine();
        }
    }
    public void Movement()
    {
        Console.Clear();
        
        List<string> directions = new List<string>();
        // List<string> dirAb = new List<string>();


        // Checks if movement is possible in each direction
        if (InGrid(currentCoord.Item1, currentCoord.Item2 - 1, Length, Width) && grid.ContainsKey((currentCoord.Item1, currentCoord.Item2 - 1)))
            directions.Add("north");
            // dirAb.Add("n");

        if (InGrid(currentCoord.Item1, currentCoord.Item2 + 1, Length, Width) && grid.ContainsKey((currentCoord.Item1, currentCoord.Item2 + 1)))
            directions.Add("south");
            // dirAb.Add("s");

        if (InGrid(currentCoord.Item1 + 1, currentCoord.Item2, Length, Width) && grid.ContainsKey((currentCoord.Item1 + 1, currentCoord.Item2)))
            directions.Add("east");
            // dirAb.Add("e");
        
        if (InGrid(currentCoord.Item1 - 1, currentCoord.Item2, Length, Width) && grid.ContainsKey((currentCoord.Item1 - 1, currentCoord.Item2)))
            directions.Add("west");
            // dirAb.Add("w");


        DisplayMap(currentCoord);
        // Display available directions
        Console.WriteLine( grid[currentCoord].Description);
        Console.Write("You can go ");
        if (directions.Count > 0)
        {
            Console.WriteLine(string.Join(", ", directions) + ".");
        }
        else
        {
            Console.WriteLine("nowhere.");
        }


        // foreach (string dir in directions)
        // {
        //     Console.WriteLine(dir);
        // }
        // foreach (string dir in dirAb)
        // {
        //     Console.WriteLine(dir);
        // }
        

        Console.Write("Enter a direction: ");
        string input = Console.ReadLine()?.ToLower();

        // Validate input
        //  || dirAb.Contains(input)
        if (directions.Contains(input))
        {
            switch (input)
            {
                case "north":
                // case "n":
                    currentCoord = (currentCoord.Item1, currentCoord.Item2 - 1);
                    break;
                case "south":
                // case "s":
                    currentCoord = (currentCoord.Item1, currentCoord.Item2 + 1);
                    break;
                case "east":
                // case "e":
                    currentCoord = (currentCoord.Item1 + 1, currentCoord.Item2);
                    break;
                case "west":
                // case "w":
                    currentCoord = (currentCoord.Item1 - 1, currentCoord.Item2);
                    break;
                
            }
        }
        else
        {
            Console.WriteLine("Invalid direction! Try again.");
        }
        // Movement(currentCoord);

    }

    public void Action()
    {
        Console.Clear();
        Console.WriteLine( grid[currentCoord].Description);
        Console.WriteLine("");
        Console.WriteLine("What will you do:");
        Console.WriteLine("1. Move");
        Console.WriteLine("2. Inventory");
        Console.WriteLine("3. Exit Dungeon");
        Console.Write("Enter a number: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.Clear();
                Movement();
                break;
            case "2":
                Console.Clear();
                // inventory.DisplayInventory();
                break;
            case "3":
                Console.Clear();
                Console.WriteLine("Goodbye!");
                QuitDungeon = true;

                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
}
