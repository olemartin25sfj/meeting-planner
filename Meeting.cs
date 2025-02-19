class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime meetingDateTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<string> Participants { get; set; } = new List<string>();

    public void Display()
    {
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Date & Time: {meetingDateTime}");
        Console.WriteLine($"Location: {Location}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Participants: {string.Join(", ", Participants)}");
        Console.WriteLine(new string('-', 30));
    }
}