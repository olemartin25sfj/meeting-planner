using System.Runtime.CompilerServices;
using System.Text.Json;
using System.IO;
using System.Linq.Expressions;

class MeetingController
{
    private List<Meeting> meetings = new List<Meeting>();
    private MeetingView view = new MeetingView();

    public void AddMeeting()
    {
        try
        {
            Console.Write("Skriv inn møtetittel: ");
            string title = Console.ReadLine() ?? string.Empty;

            DateTime? dateTime = null;
            bool isValidDate = false;
            while (!isValidDate)
            {
                Console.Write("Skriv inn dato og tid (dd-MM-yyyy HH:mm): ");
                string? inputDate = Console.ReadLine();

                if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    dateTime = parsedDate;
                    isValidDate = true;
                }
                else
                {
                    Console.WriteLine("Ugyldig input, bruk dd-MM-yyyy HH:mm");
                }
            }

            Console.Write("Skriv inn sted: ");
            string location = Console.ReadLine() ?? string.Empty;

            Console.Write("Skriv inn beskrivelse for møtet: ");
            string description = Console.ReadLine() ?? string.Empty;

            Console.Write("Skriv inn deltakere (kommaseparert): ");
            List<string> participants = (Console.ReadLine() ?? "").Split(',').Select(p => p.Trim()).ToList();

            int newId = meetings.Count > 0 ? meetings.Max(m => m.Id) + 1 : 1;

            var newMeeting = new Meeting
            {
                Id = newId,
                Title = title,
                meetingDateTime = dateTime ?? DateTime.MinValue,
                Location = location,
                Description = description,
                Participants = participants
            };

            meetings.Add(newMeeting);
            SaveMeetingsToFile();

            Console.WriteLine("Møte lagt til!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under opprettelsen av møte: {ex.Message}");
        }
    }

    public void ShowMeetings()
    {
        try
        {
            view.ShowMeetings(meetings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under visning av møter: {ex.Message}");
        }
    }

    public void EditMeeting()
    {
        try
        {
            Console.Write("Skriv inn ID for møte du vil endre: ");
            if (int.TryParse(Console.ReadLine(), out int meetingId))
            {
                var meetingToEdit = meetings.FirstOrDefault(m => m.Id == meetingId);

                if (meetingToEdit != null)
                {
                    Console.Write($"Endre tittel (nåværende: {meetingToEdit.Title}): ");
                    string newTitle = Console.ReadLine() ?? meetingToEdit.Title;
                    if (!string.IsNullOrEmpty(newTitle))
                        meetingToEdit.Title = newTitle;

                    Console.Write($"Endre dato og tid (nåværende: {meetingToEdit.meetingDateTime}) (dd-MM-yyyy HH:mm): ");
                    string? newDate = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDate))
                        meetingToEdit.meetingDateTime = DateTime.TryParse(newDate, out DateTime parsedDate) ? parsedDate : meetingToEdit.meetingDateTime;

                    Console.Write($"Endre sted (nåværende: {meetingToEdit.Location}): ");
                    string newLocation = Console.ReadLine() ?? meetingToEdit.Location;
                    if (!string.IsNullOrEmpty(newLocation))
                        meetingToEdit.Location = newLocation;

                    Console.Write($"Endre beskrivelse (nåværende: {meetingToEdit.Description}): ");
                    string newDescription = Console.ReadLine() ?? meetingToEdit.Description;
                    if (!string.IsNullOrEmpty(newDescription))
                        meetingToEdit.Description = newDescription;

                    Console.Write($"Endre deltakere (nåværende: {string.Join(", ", meetingToEdit.Participants)}): ");
                    string newParticipants = Console.ReadLine() ?? string.Join(", ", meetingToEdit.Participants);
                    if (!string.IsNullOrEmpty(newParticipants))
                        meetingToEdit.Participants = newParticipants.Split(',').Select(p => p.Trim()).ToList();

                    Console.WriteLine("Møte oppdatert!");
                }
                else
                {
                    Console.WriteLine("Fant ikke møte med gitt ID.");
                }
            }
            else
            {
                Console.WriteLine("Ugyldig ID.");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Feil i formatet for dato/tid: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under oppdatering av møte: {ex.Message}");
        }
    }

    public void DeleteMeeting()
    {
        try
        {
            Console.Write("Skriv inn ID for møtet du vil slette: ");
            if (int.TryParse(Console.ReadLine(), out int meetingId))
            {
                var meetingToDelete = meetings.FirstOrDefault(m => m.Id == meetingId);

                if (meetingToDelete != null)
                {
                    meetings.Remove(meetingToDelete);
                    SaveMeetingsToFile();
                    Console.WriteLine("Møtet er slettet.");
                }
                else
                {
                    Console.WriteLine("Fant ikke møte med gitt ID.");
                }
            }
            else
            {
                Console.WriteLine("Ugyldig ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under sletting av møte: {ex.Message}");
        }
    }

    public void DeleteAllMeetings()
    {
        try
        {
            Console.Write("Er du sikker på at du vil slette alle møter? (ja/nei): ");
            string response = Console.ReadLine()?.ToLower() ?? string.Empty;

            if (response == "ja")
            {
                meetings.Clear();
                SaveMeetingsToFile();
                Console.WriteLine("Alle møter er slettet.");
            }
            else
            {
                Console.WriteLine("Ingen møter ble slettet.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under sletting av alle møter: {ex.Message}");
        }
    }

    private string filePath = "meetings.json";

    public void SaveMeetingsToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("Møter lagret til fil.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Feil under filoperasjon: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunne ikke lagre møter: {ex.Message}");
        }
    }

    public void LoadMeetingsFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                meetings = JsonSerializer.Deserialize<List<Meeting>>(json) ?? new List<Meeting>();
                Console.WriteLine("Møter lastet inn fra fil.");
            }
            else
            {
                Console.WriteLine("Fant ikke møtefil.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Feil under filoperasjon: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Feil under parsing av JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunne ikke laste møter fra fil: {ex.Message}");
        }
    }
}
