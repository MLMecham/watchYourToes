using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public class Battle
{
    private Character character;
    private List<Enemy> enemies;
    public List<Combatant> combatants = new List<Combatant>();
    private Random random = new Random();

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

        while (character.Stats.BaseStats.Health > 0 && enemies.Count > 0){
            
            foreach (var combatant in combatants.ToList())//copy of combatants list
            {
            if (character.Stats.BaseStats.Health <= 0 || enemies.Count == 0) break;  //stops battle if over

            if (combatant is Character charCombatant)
                {
                    await PlayerTurn(charCombatant);
                }
            else if (combatant is Enemy enemy)
                {
                    await EnemyTurn(enemy);
                }

            //removes enemies from the turns list
            combatants = combatants.Where(c => !(c is Enemy e && e.IsDefeated())).ToList();
            }
        }

        // Battle result
        if (character.Stats.BaseStats.Health <= 0)
        {
            Console.WriteLine($"{character.Name} has been defeated...");
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
        while (true)
        {
            Console.Write("Enter the number of the enemy you want to attack: ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= enemies.Count)
            {
                break;
            }
            Console.WriteLine("Invalid choice. Try again.");
        }

        Enemy target = enemies[choice - 1];
        int damage = charCombatant.Stats.BaseStats.Attack;
        

        Console.WriteLine($"{charCombatant.Name} attacks {target.Name} for {damage} damage!");
        target.TakeDamage(damage);

        if (target.IsDefeated())
        {
            Console.WriteLine($"{target.Name} has been defeated!");
            enemies.Remove(target);
            Console.WriteLine($"Remaining enemies: {enemies.Count}");
        }

        await Task.Delay(1000);
    }

    //enemy just attacks character, no choice
    public async Task EnemyTurn(Enemy enemy)
    {
        if (character.Stats.BaseStats.Health <= 0) return;  //if they are dead already

        int damage = enemy.CurrentStat.Attack - character.Stats.BaseStats.Defense;
        damage = Math.Max(damage, 1); // Ensures at least 1 damage is dealt

        character.Stats.BaseStats.Health -= damage;
        Console.WriteLine($"{enemy.Name} attacks {character.Name} for {damage} damage! Remaining HP: {character.Stats.BaseStats.Health}");

        await Task.Delay(1000);
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
