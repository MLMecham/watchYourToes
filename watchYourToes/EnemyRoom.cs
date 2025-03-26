using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.IO;
using System.Text.Json;

// 3.17 Notes: Make battle work. 
// Structure:
// Combatant -- Character -- Different Characters 
//  |--> Enemy 
// In Combatant, there's a virtual attack() method. Override it and do the calculation of fighting

// 3.14 notes: Next thing, make the deserialization work and make sure that you can spond the enemies
// It seems like there are 2 instance of EnemyRoom.Enemy here somewhere, fix it.

public class EnemyRoom : Room{
    public int floorNumber;
    public List<Enemy> enemiesList {get; set;} = new List<Enemy>();

    public EnemyRoom(int floorNumber = 1) : base("A dark and eerie room filled with enemies.") // Call the base class constructor with a default description
    {
        getEnemyList(floorNumber);   // Initialize the enemy list for the room
    }

    // Class to match the JSON structure
    public class EnemyData
    {
        public Dictionary<string, EnemyEntry> EnemyDict { get; set; } = new Dictionary<string, EnemyEntry>();
    }

    public class LootData
    {
        public Dictionary<string, Gear> LootDict { get; set; } = new Dictionary<string, Gear>();
    }

    public class EnemyEntry // for deserializing JSON data
    {   
        public string name { get; set; } = "";
        public int healthpoint { get; set; }
        public int attackpoint { get; set; }
        public int defencepoint { get; set; }
        public int magicattackpoint { get; set; }
        public int magicdefencepoint { get; set; }
        public int speed { get; set; }
        public int exp { get; set; }
        public float possibilityOfDrop { get; set; }
    }

    public void getEnemyList(int floorNumber)
    {
        this.floorNumber = floorNumber;
        Random random = new Random();
        int enemyNumber = random.Next(1, floorNumber + 1);

        try
        {   
            // string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "EnemyTable.json");
            // Console.WriteLine($"Looking for file at: {jsonFilePath}");

            string enemyString = File.ReadAllText("EnemyTable.json"); // get all of the enemy json
            EnemyData enemyData = JsonSerializer.Deserialize<EnemyData>(enemyString); // handle null list

            string gearString = File.ReadAllText("NormalLootTable.json"); // get all of the loot json
            LootData lootData = JsonSerializer.Deserialize<LootData>(gearString);

            if (enemyData.EnemyDict == null || !enemyData.EnemyDict.Any())
            {
                Console.WriteLine("Warning: No enemy data fonud in Json file");
            }

            if (lootData.LootDict == null || !lootData.LootDict.Any())
            {
                Console.WriteLine("Warning: No loot data found in Json file");
            }

            // Convert dictionary entries to a list, shuffle them, and take a random number
            var enemyEntries = (enemyData.EnemyDict ?? new Dictionary<string, EnemyEntry>()).Values.ToList();
            var selectedEnemies = enemyEntries
                    .OrderBy(x => random.Next()) // get random enemyNumber amount of enemies from the reshuffled List. Each enemy is unique
                    .Take(enemyNumber)
                    .ToList(); // provide an empty list if enemyTable is null

            var lootEntries = (lootData.LootDict ?? new Dictionary<string, Gear>()).Values.ToList();
            var selectedLoots = lootEntries
                    .OrderBy(x => random.Next())
                    .Take(enemyNumber)
                    .ToList(); // provide an empty list if enemyTable is null

            foreach(EnemyEntry enemyEntry in selectedEnemies)
            {
                enemiesList.Add(new Enemy(
                    enemyEntry.name,
                    enemyEntry.healthpoint, 
                    enemyEntry.attackpoint, 
                    enemyEntry.defencepoint, 
                    enemyEntry.magicattackpoint, 
                    enemyEntry.magicdefencepoint, 
                    enemyEntry.speed, 
                    enemyEntry.exp, 
                    new List<Gear> {selectedLoots[random.Next(0, enemyNumber)]}, // add a random loot to the enemy
                    enemyEntry.possibilityOfDrop,
                    this.floorNumber));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "!!!!");
        }

        // Debugging: Print out the enemiesList to see if it was populated correctly
        // foreach (Enemy enemy in enemiesList)
        // {
        //     Console.WriteLine($"You are fighting an emeny--{enemy.Name} with " + enemy.CurrentStat.Health + " HP, " + enemy.CurrentStat.Attack + " ATK, " + enemy.CurrentStat.Defense + " DEF, " + enemy.CurrentStat.MagicAttack + " MATK, " + enemy.CurrentStat.MagicDefense + " MDEF, " + enemy.CurrentStat.Speed + " SPD");
        // }
    }
}