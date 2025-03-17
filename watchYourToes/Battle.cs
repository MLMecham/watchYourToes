public class Battle
{
    private List<Enemy> enemies; 
    private Character character; 
    private int currentEnemyIndex = 0; // Tracks which enemy is attacking

    public Battle(Character character, List<string> enemies)
    {
        this.character = character;
        this.enemies = new List<Enemy>(enemies); // Copy enemy list
    }

    public async Task StartBattle()
    {
        Console.WriteLine($"⚔️ {character.Name} has entered battle!");
        
        while (character.Stats.CurrentStats.Health> 0 && enemies.Count > 0)
        {
            Console.WriteLine("\n--- New Turn ---");

            // Character's turn
            CharacterAttack();

            // Check if enemies are still alive before their turn
            if (enemies.Count == 0)
            {
                Console.WriteLine($"🏆 {character.Name} has won the battle!");
                return;
            }

            // Enemies' turn (each enemy gets a turn)
            EnemyTurn();

            // Display battle status
            DisplayStatus();
            await Task.Delay(1000); // Add delay for readability
        }

        // Check if the character lost
        if (character.Stats.CurrentHealth <= 0)
        {
            Console.WriteLine($"💀 {character.Name} was defeated...");
        }
    }

    private void CharacterAttack()
    {
        if (enemies.Count == 0) return;

        Enemy target = enemies[0]; // Attack the first enemy in the list
        int damage = character.Stats.BaseStats.Attack;
        target.TakeDamage(damage); //take damage should be a method in Enemy class
        Console.WriteLine($"{character.Name} attacks {target.Name} for {damage} damage!");

        // Remove enemy if defeated
        if (target.IsDefeated)
        {
            Console.WriteLine($"{target.Name} has been defeated!");
            enemies.Remove(target);
        }
    }

    private void EnemyTurn()
    {
        foreach (var enemy in enemies)
        {
            int damage = enemy.Attack();
            character.TakeDamage(damage);
            Console.WriteLine($"{enemy.Name} attacks {character.Name} for {damage} damage!");

            // Stop if the character is defeated
            if (character.Stats.CurrentHealth <= 0)
                return;
        }
    }

    private void DisplayStatus()
    {
        Console.WriteLine($"\n{character.Name} - HP: {character.Stats.CurrentHealth}");
        foreach (var enemy in enemies)
        {
            Console.WriteLine($"{enemy.Name} - HP: {enemy.CurrentHealth}");
        }
    }
}
