using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.IO;
using System.Text.Json;

// 3.14 notes: Next thing, make the deserialization work and make sure that you can spond the enemies
// It seems like there are 2 instance of EnemyRoom.Enemy here somewhere, fix it.

public class EnemyRoom{
    public int floorNumber = 1;
    public List<Enemy> enemiesList {get; set;} = new List<Enemy>();


    // Class to match the JSON structure
    public class EnemyData
    {
        public Dictionary<string, EnemyEntry> EnemyDict { get; set; } = new Dictionary<string, EnemyEntry>();
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
            string jsonString = File.ReadAllText("EnemyData.json"); // get all of the enemy json
            EnemyData enemyData = JsonSerializer.Deserialize<EnemyData>(jsonString) ?? new EnemyData(); // handle null list

            if (enemyData.EnemyDict == null || !enemyData.EnemyDict.Any())
            {
                Console.WriteLine("Warning: No enemy data fonud in Json file");
            }

            // Convert dictionary entries to a list, shuffle them, and take a random number
            var enemyEntries = enemyData.EnemyDict.Values.ToList();
            var selectedEnemies = enemyEntries
                    .OrderBy(x => random.Next()) // change the amount of the enemy according to the floor number later
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
                    new List<Gear>(), // for now, no gear 
                    enemyEntry.possibilityOfDrop,
                    this.floorNumber));
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Cannot read enemy data");
        }

        foreach (Enemy enemy in enemies)
        {
            Console.WriteLine($"You are fighting an emeny--{enemy.Name} with " + enemy.hp + " HP, " + enemy.atk + " ATK, " + enemy.def + " DEF, " + enemy.matk + " MATK, " + enemy.mdef + " MDEF, " + enemy.spd + " SPD");
        }
    }
}