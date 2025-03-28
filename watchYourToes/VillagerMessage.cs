public class VillagerMessage
{
    public string User_query { get; set; }
    public int Days { get; set; }  // Number of days the character has spent in the dungeon

    public VillagerMessage(string user_query, int days)
    {
       User_query = user_query;
       Days = days;
    }
}