using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class BattleMessage
{
    public string Name { get; set; }  // Attacker's name (Character or Enemy)
    public string Name_Class { get; set; }
    public string Action { get; set; }  // Attack type
    public string Target { get; set; }  // Target's name
    public string Target_Class { get; set; }

    public BattleMessage(string name, string name_class, string action, string target, string target_class)
    {
        Name = name; //for both character and enemy
        Name_Class = name_class;
        Action = action;
        Target = target;
        Target_Class = target_class;
    }
}



