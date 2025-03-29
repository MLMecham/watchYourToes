using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class JsonManager
{
    // Static list of enemies, accessible globally
    public static List<Enemy> JsonEnemyList { get; set; } = new List<Enemy>();
    public static List<Gear> JsonGearList { get; set; } = new List<Gear>();
    private static Random random = new Random();

    // Method to load the enemies from a JSON file
    public static void LoadEnemiesFromJson(string filePath)
    {
        try
        {
            // Check if the file exists
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                
                // Deserialize the JSON string into the list of enemies
                JsonEnemyList = JsonSerializer.Deserialize<List<Enemy>>(jsonString);
                
                Console.WriteLine("Enemies loaded successfully!");

                // Initialize the stats for each enemy
                foreach (var enemy in JsonEnemyList)
                {
                    // Initialize the CurrentStat based on enemy's data
                    enemy.InitializeCurrentStat();

                    // Print the enemy's information
                    // Console.WriteLine($"Name: {enemy.Name} | Health: {enemy.Health} | Attack: {enemy.Attack} | Defense: {enemy.Defense} | Magic Attack: {enemy.MagicAttack} | Magic Defense: {enemy.MagicDefense} | Speed: {enemy.Speed} | Exp: {enemy.Exp} | Possibility of Drop: {enemy.PossibilityOfDrop}");
                    
                    // Print the stats of the current stat
                    // enemy.CurrentStat.PrintStats();

                    // Console.WriteLine("----------------");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading enemies from file: {ex.Message}");
        }
    }

    public static void PrintAllEnemies()
    {
        if (JsonEnemyList.Count == 0)
        {
            Console.WriteLine("No enemies to display.");
            return;
        }

        foreach (var enemy in JsonEnemyList)
        {
            // Print the enemy's basic information
            Console.WriteLine($"Name: {enemy.Name} | Health: {enemy.Health} | Attack: {enemy.Attack} | Defense: {enemy.Defense} | Magic Attack: {enemy.MagicAttack} | Magic Defense: {enemy.MagicDefense} | Speed: {enemy.Speed} | Exp: {enemy.Exp} | Possibility of Drop: {enemy.PossibilityOfDrop}");
            
            // Print the stats of the current stat
            enemy.CurrentStat.PrintStats();

            Console.WriteLine("----------------");
        }
    }

    // Method to return a new enemy object (deep copy) chosen randomly
    public static Enemy GetRandomEnemy(int floor)
    {
        if (JsonEnemyList.Count == 0)
        {
            Console.WriteLine("Enemy list is empty. Cannot generate a random enemy.");
            return null;
        }

        // Select a random enemy from the list
        int randomIndex = random.Next(JsonEnemyList.Count);
        // Console.WriteLine(JsonEnemyList.Count);
        Enemy originalEnemy = JsonEnemyList[randomIndex];

        // Create a new Enemy object with the same attributes (Deep Copy)
        Enemy newEnemy = new Enemy(
            originalEnemy.Name,
            originalEnemy.Health,
            originalEnemy.Attack,
            originalEnemy.Defense,
            originalEnemy.MagicAttack,
            originalEnemy.MagicDefense,
            originalEnemy.Speed,
            originalEnemy.Exp,
            originalEnemy.PossibilityOfDrop,
            new List<Gear>()
        );

        // Initialize the new enemy's stats separately
        newEnemy.InitializeCurrentStat();

        return newEnemy;
    }

    public static void LoadGearFromJson(string filePathGear)
    {
        try
        {
            if (File.Exists(filePathGear))
            {
                string jsonString = File.ReadAllText(filePathGear);
                
                // Deserialize the gear list
                JsonGearList = JsonSerializer.Deserialize<List<Gear>>(jsonString);
                
                Console.WriteLine("Gear loaded successfully!");

                // Print all loaded gear
                foreach (var gear in JsonGearList)
                {
                    Console.WriteLine($"Name: {gear.Name} | Slot: {gear.Slot} | Health: {gear.HealthChange} | Attack: {gear.AttackChange} | Defense: {gear.DefenseChange} | Magic Attack: {gear.MagicAttackChange} | Magic Defense: {gear.MagicDefenseChange} | Speed: {gear.SpeedChange}");
                    Console.WriteLine("----------------");
                }
            }
            else
            {
                Console.WriteLine("Gear file does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading gear from file: {ex.Message}");
        }
    }

    public static Gear GetRandomGear()
    {
        if (JsonGearList.Count == 0)
        {
            Console.WriteLine("Gear list is empty. Cannot generate a random gear.");
            return null;
        }

        int randomIndex = random.Next(JsonGearList.Count);
        return JsonGearList[randomIndex];  // Return a randomly selected gear
    }
}
