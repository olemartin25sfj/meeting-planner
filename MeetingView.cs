class MeetingView
{
    public void ShowMeetings(List<Meeting> meetings)
    {
        if (meetings.Count == 0)
        {
            Console.WriteLine("Ingen møter er planlagt.");
            return;
        }

        foreach (var meeting in meetings)
        {
            // Console.WriteLine($"ID: {meeting.Id} - {meeting.Title} - {meeting.Description} - {meeting.meetingDateTime}");
            meeting.Display();
        }
    }
}
