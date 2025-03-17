using System;
using ZstdSharp.Unsafe;

public class Enemy{
    // for boss: it creates 5 different types of enemies and append them into the array
    public string name { get; set; } = "Goblin";
    public Stat stat = new Stat();
    public int exp { get; set; }
    public List<Gear> enemyGears { get; set; } = new List<Gear>();
    public float possibilityOfDrop { get; set; }
    public float[] enemyStrongness = {
    1.0f, 1.2f, 1.4f, 1.7f, 2.0f, 2.4f, 2.8f, 3.3f, 3.8f,
    4.4f, 5.0f, 5.7f, 6.5f, 7.4f, 8.4f, 9.5f, 10.7f, 12.0f, 13.5f, 15.0f
    };

    public Enemy(List<Gear> enemyGears){
        this.exp = 3;
        this.enemyGears = enemyGears;
        this.possibilityOfDrop = 0.9f;
    }
    
    public Enemy(string name, int hp, int atk, int def, int matk, int mdef, int spd, int exp, List<Gear> enemyGears, float possibilityOfDrop, int floorNumber){
        this.name = name;
        stat.Health = (int)Math.Floor(hp*enemyStrongness[floorNumber-1]); // adjust the stats of the enemy based on the floor number
        stat.Attack = (int)Math.Floor(atk*enemyStrongness[floorNumber-1]);
        stat.Defense = (int)Math.Floor(def*enemyStrongness[floorNumber-1]);
        stat.MagicAttack = (int)Math.Floor(matk*enemyStrongness[floorNumber-1]);
        stat.MagicDefense = (int)Math.Floor(mdef*enemyStrongness[floorNumber-1]);
        stat.Speed= (int)Math.Floor(spd*enemyStrongness[floorNumber-1]);
        
        this.exp = exp*floorNumber;
        this.enemyGears = enemyGears;
        this.possibilityOfDrop = possibilityOfDrop;
    }

    public List<Gear> DropLoot(){
        Random random = new Random();
        List<Gear> droppedGears = new List<Gear>();
        foreach (Gear gear in enemyGears){
            if (random.Next() <= possibilityOfDrop){
                droppedGears.Add(gear);
            }
        } 
        return droppedGears;
    }

}