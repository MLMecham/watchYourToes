using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using watchYourToes;

class TrapRoom : Room
{
    public Trap trap;
    private static List<Trap> traps = new List<Trap>();
    private static readonly Random rnd = new Random(); // Reuse Random instance

    static TrapRoom()
    {
        LoadTrapsFromJson("obj/traps.json"); // Load traps **once** for all instances
    }

    public TrapRoom() : base("The room reeks of trouble...")
    {
        trap = GetRandomTrap();
    }

    private static void LoadTrapsFromJson(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                traps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Trap>>(json) ?? new List<Trap>();
            }
            else
            {
                Console.WriteLine("Warning: Trap JSON file not found. Using default traps.");
                traps = new List<Trap>
                {
                    new Trap("Spikes", "Spikes shoot from the floor!", "pierce"),
                    new Trap("Poison Gas", "A cloud of poison fills the room!", "poison")
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading traps: {ex.Message}");
            traps = new List<Trap> { new Trap("Spikes", "Spikes shoot from the floor!", "pierce") };
        }
    }

    private Trap GetRandomTrap()
    {
        return traps.Count > 0 ? traps[rnd.Next(traps.Count)] : new Trap("No Trap", "Nothing happens.", "none");
    }

    public void TriggerTrap(Character character)
    {
        Trap trap = GetRandomTrap();
        Console.WriteLine($"⚠️ {trap.Name}: {trap.Description}");

        Effect effect = trap.EffectType.ToLower() switch
        {
            "poison" => new Poison(10),
            "burn" => new Burn(10),
            "freeze" => new Freeze(10),
            "pierce" => new Pierce(10),
            "crush" => new Crush(10),
            "mental" => new Mental(10),
            "magicdrain" => new MagicDrain(10),
            "teleport" => new Teleport(10),
            "radiation" => new Radiation(10),
            "blind" => new Blind(10),
            "bleed" => new Bleed(10),
            "drowning" => new Drowning(10),
            "aging" => new Aging(10),
            "explosive" => new Explosive(10),
            "fall" => new Fall(10),

            _ => throw new ArgumentException($"Unknown effect type: {trap.EffectType}")
        };

        character.ActiveEffects.Add(effect);
    }
}


// Effects:
// Poison
// Burn
// Explosive
// Freeze
// Pierce
// Crush
// Mental
// Magic Drain
// Teleport
// Radiation
// Blind
// Bleed
// Drowning
// Aging


// When die: pause and tell why died, save, return to main menu, initate long rest to reset.