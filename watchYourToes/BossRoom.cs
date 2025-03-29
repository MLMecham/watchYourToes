using System;
using System.Collections.Generic;

class BossRoom : Room
{   

    public List<Enemy> enemiesList {get; set;} = new List<Enemy>();
    public int bossEnemyNumber = 5;
    public BossRoom(Character character) : base("A MASSIVE Spider stares at you with eight eyes, poison oozing from it's mouth", character)
    {
    }

    public override void RoomEffect()
    {
        Console.WriteLine($"This is the boss room.");
        enemiesList.Add(JsonManager.GetRandomEnemy(bossEnemyNumber));
        Battle newBattle = new Battle(character, enemiesList);
        newBattle.StartBattle();
    }
}