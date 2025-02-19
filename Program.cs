using System.Data;
using System;


class Program
{
    static void Main()
    {
        MeetingController controller = new MeetingController();

        controller.LoadMeetingsFromFile();

        while (true)
        {
            Console.WriteLine("1. Legg til møte");
            Console.WriteLine("2. Vis møter");
            Console.WriteLine("3. Endre møte");
            Console.WriteLine("4. Slett møte");
            Console.WriteLine("5. Slett alle møter");
            Console.WriteLine("6. Avslutt");
            Console.Write("Velg et alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    controller.AddMeeting();
                    break;
                case "2":
                    controller.ShowMeetings();
                    break;
                case "3":
                    controller.EditMeeting();
                    break;
                case "4":
                    controller.DeleteMeeting();
                    break;
                case "5":
                    controller.DeleteAllMeetings();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Ugyldig alternativ, prøv igjen.");
                    break;
            }
        }
    }
}