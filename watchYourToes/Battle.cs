using System;
using System.Collections.Generic;
using System.Threading.Tasks;


public class Battle
{
    private Character character;
    private List<Enemy> enemies;
    private Random random = new Random();

    public Battle(Character character, List<Enemy> enemies)
    {
        this.character = character;
        this.enemies = enemies;
    }

    public async Task StartBattle()
    {
        Console.WriteLine($"Battle starts! {character.Name} vs {enemies.Count} enemie(s)!");

        while (character.Stats.BaseStats.Health > 0 && enemies.Count > 0)
        {
            await PlayerTurn();

            if (enemies.Count > 0)
            {
                await EnemiesTurn();
            }
        }

        if (character.Stats.BaseStats.Health <= 0)
        {
            Console.WriteLine($"{character.Name} has been defeated...");
        }
        else
        {
            Console.WriteLine($"Victory! {character.Name} defeated all enemies!");
        }
    }

    private async Task PlayerTurn()
    {
        Console.WriteLine($"{character.Name}'s turn!");

        Enemy target = enemies[0]; // Attack the first enemy in the list
        int damage = character.Stats.BaseStats.Attack;
        target.TakeDamage(damage);

        if (target.IsDefeated())
        {
            Console.WriteLine($"{target.name} has been defeated!");
            enemies.Remove(target);
        }

        await Task.Delay(1000);
    }

    private async Task EnemiesTurn()
    {
        Console.WriteLine("Enemies' turn!");

        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsDefeated()) continue; // Skip defeated enemies

            int damage = enemy.stat.Attack - character.Stats.BaseStats.Defense;
            if (damage < 1) damage = 1; // Ensure at least 1 damage is dealt

            character.Stats.BaseStats.Health -= damage;
            Console.WriteLine($"{enemy.name} attacks {character.Name} for {damage} damage! Remaining HP: {character.Stats.BaseStats.Health}");

            if (character.Stats.BaseStats.Health <= 0) break; // Stop if player is defeated
        }

        await Task.Delay(1000);
    }
}
