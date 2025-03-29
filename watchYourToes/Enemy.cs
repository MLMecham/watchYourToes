using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization; // Required for OnDeserialized

public class Enemy : Combatant
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonIgnore] // Prevents automatic deserialization of CurrentStat
    public Stat CurrentStat { get; set; } = new Stat();

    [JsonPropertyName("healthpoint")]
    public int Health { get; set; }

    [JsonPropertyName("attackpoint")]
    public int Attack { get; set; }

    [JsonPropertyName("defencepoint")]
    public int Defense { get; set; }

    [JsonPropertyName("magicattackpoint")]
    public int MagicAttack { get; set; }

    [JsonPropertyName("magicdefencepoint")]
    public int MagicDefense { get; set; }

    [JsonPropertyName("speed")]
    public int Speed { get; set; }

    [JsonPropertyName("exp")]
    public int Exp { get; set; }

    [JsonPropertyName("possibilityOfDrop")]
    public float PossibilityOfDrop { get; set; }

    public List<Gear> EnemyGears { get; set; } = new List<Gear>();

    public float[] EnemyStrongness = {
        1.0f, 1.2f, 1.4f, 1.7f, 2.0f, 2.4f, 2.8f, 3.3f, 3.8f,
        4.4f, 5.0f, 5.7f, 6.5f, 7.4f, 8.4f, 9.5f, 10.7f, 12.0f, 13.5f, 15.0f
    };

    // Parameterless constructor for deserialization
    public Enemy() { 

        // InitializeCurrentStat();
    }

    public Enemy(string name, int healthpoint, int attackpoint, int defencepoint,
                int magicattackpoint, int magicdefencepoint, int speed, int exp,
                float possibilityOfDrop, List<Gear> enemyGears)
    {
        Name = name;
        Health = healthpoint;
        Attack = attackpoint;
        Defense = defencepoint;
        MagicAttack = magicattackpoint;
        MagicDefense = magicdefencepoint;
        Speed = speed;
        Exp = exp;
        PossibilityOfDrop = possibilityOfDrop;
        
        EnemyGears = enemyGears ?? new List<Gear>();  // Avoid null reference

        CurrentStat = new Stat(Health, Attack, Defense, MagicAttack, MagicDefense, Speed);

        InitializeCurrentStat();
    }

    // This method runs after JSON deserialization
    [OnDeserialized]
    private void OnDeserializedMethod(StreamingContext context)
    {
        InitializeCurrentStat();
    }

    public void InitializeCurrentStat()
    {
        // CurrentStat.Health = Health;
        // CurrentStat.Attack = Attack;
        // CurrentStat.Defense = Defense;
        // CurrentStat.MagicAttack = MagicAttack;
        // CurrentStat.MagicDefense = MagicDefense;
        // CurrentStat.Speed = Speed;

        CurrentStat = new Stat(Health, Attack, Defense, MagicAttack, MagicDefense, Speed);
    }

    public void TakeDamage(int damage)
    {
        CurrentStat.Health -= damage;
        if (CurrentStat.Health < 0) CurrentStat.Health = 0;
        Console.WriteLine($"{Name} took {damage} damage! Remaining HP: {CurrentStat.Health}");
    }

    public bool IsDefeated()
    {
        return CurrentStat.Health <= 0;
    }

    public override void BasicAttack()
    {
        throw new NotImplementedException();
    }
}
