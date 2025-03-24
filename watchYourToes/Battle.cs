using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Linq;
using System.Text;


public class Battle
{
    private Character character;
    private List<Enemy> enemies;
    public List<Combatant> combatants = new List<Combatant>();
    private Random random = new Random();
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:8000/") };


    public Battle(Character character, List<Enemy> enemies)
    {
        this.character = character;
        this.enemies = enemies;
        
    
        // Add the character to the combatants list
        combatants.Add(character);

        // Add the enemies to the combatants list
        combatants.AddRange(enemies);
        
        combatants = combatants.OrderByDescending(c => 
        {
            if (c is Character charCombatant)
            {
                return charCombatant.Stats.CurrentStats.Speed;  // Use the Speed from the Character class
            }
            else if (c is Enemy enemy)
            {
                return enemy.CurrentStat.Speed;  // Use the Speed from the Enemy class
            }
            return 0;  // Default if neither (just to ensure the code compiles)
        }).ToList();

        // Display the combatants turn order
        
    }
    public async Task StartBattle()
    {
        Console.WriteLine($"Battle starts! {character.Name} vs {enemies.Count} enemie(s)!");
     

        Console.WriteLine("Turn Order:");
        foreach (var combatant in combatants)
        {
            //Console.WriteLine(combatant);
        // Check if the combatant is of type 'Character'
        if (combatant is Character charCombatant)
        {
            // Now you can access Character-specific methods and properties
            Console.WriteLine($"{charCombatant.Name} - Speed: {charCombatant.Stats.CurrentStats.Speed}");
            // Call other methods specific to 'Character'
            charCombatant.Attack();  // Assuming Character has an Attack method
        }
        else if (combatant is Enemy enemy)
        {
            // Handle the case for non-Character combatants (e.g., Enemy)
            Console.WriteLine($"{enemy.Name} - Speed: {enemy.CurrentStat.Speed} is not a character.");
        }

        }

        // Battle loop
        while (character.Stats.BaseStats.Health > 0 && enemies.Count > 0){ // while character is alive and there are enemies left
            
            foreach (var combatant in combatants.ToList()) //copy of combatants list
            {
            if (character.Stats.BaseStats.Health <= 0 || combatants.Count == 1) break;  //stops battle if over

            if (combatant is Character charCombatant)
                {
                    await PlayerTurn(charCombatant);
                }
            else if (combatant is Enemy enemy)
                {
                    await EnemyTurn(enemy);
                }

            //removes dead enemies from combatants & enemies list
            // It's better to remove enemy here instead of in character's turn to avoid modifying the list while iterating
            // it also has the flexibility if the enemy can receive damage from other sources (e.g., effect, etc.)
            combatants = combatants.Where(c => !(c is Enemy e && e.IsDefeated())).ToList();
            enemies = enemies.Where(e => !e.IsDefeated()).ToList();
            
            }
        }

        // Battle result
        if (character.Stats.BaseStats.Health <= 0)
        {
            Console.WriteLine($"{character.Name} has been defeated...");
            // 1. The character will be send back to the village
            // 2. Update the character's inventory to null
            // 3. Update the character's current stats to the base stats
        }
        else
        {
            Console.WriteLine($"Victory! {character.Name} defeated all enemies!");
        }
    }

    //player's turn - they choose who to attack
    public async Task PlayerTurn(Character charCombatant)
    {
        if (enemies.Count == 0) return;  //if no enemies left

        //asks player which enemy to attack
        Console.WriteLine($"It's {charCombatant.Name}'s turn! Choose an enemy to attack:");

        for (int i = 0; i < enemies.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {enemies[i].Name} (HP: {enemies[i].CurrentStat.Health})");
        }

        int choice;
        int rawDamage; // the raw attack value before defense is applied
        int attackChoice; // 1 for normal attack, 2 for magic attack
        while (true)
        {
            Console.Write("Enter the number of the enemy you want to attack: ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= enemies.Count)
            {   
                while (true)
                {
                    Console.WriteLine($"Which attack you want to use on {enemies[choice - 1].Name} ?\nNormal Attack -> 1\nMagic Attack -> 2");
                    if (int.TryParse(Console.ReadLine(), out attackChoice) && (attackChoice == 1 || attackChoice == 2))
                    {
                        // Use the chosen attack type
                        rawDamage = (attackChoice == 1) ? charCombatant.Stats.BaseStats.Attack : charCombatant.Stats.BaseStats.MagicAttack;
                        break; // Exit the loop if valid choice is made
                    }
                    Console.WriteLine("Invalid attack choice. Try again.");
                }
                break;

            }
            Console.WriteLine("Invalid choice. Try again.");
        }

        Enemy target = enemies[choice - 1]; 
        String attackType = (attackChoice == 1) ? "Normal" : "Magic";

        int damage = rawDamage - target.CurrentStat.Defense;
        damage = Math.Max(damage, 1); // apply target's defense to the damage and ensure at least 1 damage is dealt
        //Console.WriteLine($"{charCombatant.Name} uses {attackType} attack on {target.Name} for {damage} damage!");
        await PrintBattleMessage(charCombatant.Name, charCombatant.ClassName, attackType, target.Name, target.Name);
        await Task.Delay(1000); 
        target.TakeDamage(damage); 
        

        if (target.IsDefeated())
        {
            Console.WriteLine($"{target.Name} has been defeated!");
            Console.WriteLine($"Remaining enemies: {enemies.Count}");
        }

        await Task.Delay(1000);
    }

    //enemy just attacks character, no choice
    public async Task EnemyTurn(Enemy enemy)
    {
        if (character.Stats.BaseStats.Health <= 0) {
            // the dead emeny already gets removed in the Battle loop
            return;
        }
        int damage = enemy.CurrentStat.Attack - character.Stats.BaseStats.Defense;
        damage = Math.Max(damage, 1); // Ensures at least 1 damage is dealt
        
        await PrintBattleMessage(enemy.Name,enemy.Name, "Attack", character.Name, character.ClassName);
        await Task.Delay(1000);
        character.TakeDamage(damage);
    }
        
    public async Task PrintBattleMessage(string name, string name_class, string action, string target, string target_class)
    {
        BattleMessage battleMessage = new BattleMessage(name, name_class,action, target, target_class);
        
        string jsonMessage = JsonSerializer.Serialize(battleMessage);
        StringContent content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync("battle-ai", content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Battle AI Response: " + result);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Error communicating with Battle AI: " + e.Message);
        }
    }









    // private async Task PlayerTurn()
    // {
    //     Console.WriteLine($"{character.Name}'s turn!");

    //     Enemy target = enemies[0]; // Attack the first enemy in the list
    //     int damage = character.Stats.BaseStats.Attack;
    //     target.TakeDamage(damage);

    //     if (target.IsDefeated())
    //     {
    //         Console.WriteLine($"{target.Name} has been defeated!");
    //         enemies.Remove(target);
    //     }

    //     await Task.Delay(1000);
    // }

    // private async Task EnemiesTurn()
    // {
    //     Console.WriteLine("Enemies' turn!");

    //     foreach (Enemy enemy in enemies)
    //     {
    //         if (enemy.IsDefeated()) continue; // Skip defeated enemies

    //         int damage = enemy.stat.Attack - character.Stats.BaseStats.Defense;
    //         if (damage < 1) damage = 1; // Ensure at least 1 damage is dealt

    //         character.Stats.BaseStats.Health -= damage;
    //         Console.WriteLine($"{enemy.name} attacks {character.Name} for {damage} damage! Remaining HP: {character.Stats.BaseStats.Health}");

    //         if (character.Stats.BaseStats.Health <= 0) break; // Stop if player is defeated
    //     }

    //     await Task.Delay(1000);
    // }
}