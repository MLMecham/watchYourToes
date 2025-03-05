public class Item
{
    public string Name { get; set; }         // The name of the item
    public string Description { get; set; }  // Description of the item

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
