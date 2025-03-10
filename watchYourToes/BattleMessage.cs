using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class BattleMessage{
    public string name { get; set; } // character's name
    public string action { get; set; } // character's action
    public string enemy { get; set; } // enemy's name
}