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





//     {
//         client.BaseAddress = new Uri("http://127.0.0.1:8000/");
//         BattleMessage battleMessage = new BattleMessage();
//         {
//             battleMessage.name = character.Name;
//             battleMessage.action = "Attack";
//             battleMessage.enemy = "Skeleton";
//             string jsonMessage = JsonSerializer.Serialize(battleMessage);
//             StringContent content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
//             HttpResponseMessage response = await client.PostAsync("battle-ai", content);
//             string result = await response.Content.ReadAsStringAsync();
//             Console.WriteLine("Battle AI Response: " + result);

//         }
//     }