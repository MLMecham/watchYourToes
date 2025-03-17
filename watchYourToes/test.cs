// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
// using System.Text.Json;

// public class EnemyRoom
// {
//     private int floorNumber = 1;
//     private List<Enemy> enemies;

//     // Class to match the JSON structure
//     public class MonsterData
//     {
//         public Dictionary<string, EnemyEntry> monsters { get; set; }
//     }

//     public class EnemyEntry
//     {
//         public int healthpoint { get; set; }
//         public int attackpoint { get; set; }
//         public int defencepoint { get; set; }
//         public int magicattackpoint { get; set; }
//         public int magicdefencepoint { get; set; }
//         public int speed { get; set; }
//         public int exp { get; set; }
//         public float possibilityOfDrop { get; set; }
//     }

//     public EnemyRoom(int floorNumber)
//     {
//         this.floorNumber = floorNumber;
//         enemies = new List<Enemy>();

//         Random random = new Random();
//         int enemyNumber = random.Next(1, floorNumber + 1);

//         try
//         {
//             string jsonString = File.ReadAllText("EnemyTable.json");
//             MonsterData monsterData = JsonSerializer.Deserialize<MonsterData>(jsonString);

//             // Convert dictionary entries to a list of key-value pairs
//             var monsterList = monsterData.monsters
//                 .Select(kvp => new { Name = kvp.Key, Stats = kvp.Value })
//                 .OrderBy(x => random.Next())
//                 .Take(enemyNumber)
//                 .ToList();

//             foreach (var monster in monsterList)
//             {
//                 enemies.Add(new Enemy(
//                     monster.Name,  // Pass the monster name
//                     monster.Stats.healthpoint,
//                     monster.Stats.attackpoint,
//                     monster.Stats.defencepoint,
//                     monster.Stats.magicattackpoint,
//                     monster.Stats.magicdefencepoint,
//                     monster.Stats.speed,
//                     monster.Stats.exp,
//                     monster.Stats.possibilityOfDrop
//                 ));
//             }
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine($"Error loading enemy