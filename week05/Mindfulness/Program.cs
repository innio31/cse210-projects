using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mindfulness Program";
            Console.ForegroundColor = ConsoleColor.Cyan;

            Dictionary<string, int> activityLog = new Dictionary<string, int>
            {
                { "Breathing", 0 },
                { "Reflection", 0 },
                { "Listing", 0 },
                { "Gratitude", 0 }
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MINDFULNESS PROGRAM ===\n");
                Console.WriteLine("Menu Options:");
                Console.WriteLine("  1. Breathing Activity");
                Console.WriteLine("  2. Reflection Activity");
                Console.WriteLine("  3. Listing Activity");
                Console.WriteLine("  4. Gratitude Activity (NEW!)");
                Console.WriteLine("  5. View Activity Statistics");
                Console.WriteLine("  6. Exit");
                Console.Write("\nSelect an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BreathingActivity breathing = new BreathingActivity();
                        breathing.Run();
                        activityLog["Breathing"]++;
                        break;
                    case "2":
                        ReflectionActivity reflection = new ReflectionActivity();
                        reflection.Run();
                        activityLog["Reflection"]++;
                        break;
                    case "3":
                        ListingActivity listing = new ListingActivity();
                        listing.Run();
                        activityLog["Listing"]++;
                        break;
                    case "4":
                        GratitudeActivity gratitude = new GratitudeActivity();
                        gratitude.Run();
                        activityLog["Gratitude"]++;
                        break;
                    case "5":
                        ShowStatistics(activityLog);
                        break;
                    case "6":
                        Console.WriteLine("\nThank you for practicing mindfulness. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        Thread.Sleep(1500);
                        break;
                }
            }
        }

        static void ShowStatistics(Dictionary<string, int> log)
        {
            Console.Clear();
            Console.WriteLine("=== ACTIVITY STATISTICS ===\n");
            Console.WriteLine($"Breathing Activity: {log["Breathing"]} sessions");
            Console.WriteLine($"Reflection Activity: {log["Reflection"]} sessions");
            Console.WriteLine($"Listing Activity: {log["Listing"]} sessions");
            Console.WriteLine($"Gratitude Activity: {log["Gratitude"]} sessions");
            Console.WriteLine($"\nTotal Sessions: {log["Breathing"] + log["Reflection"] + log["Listing"] + log["Gratitude"]}");

            if (log["Breathing"] + log["Reflection"] + log["Listing"] + log["Gratitude"] > 0)
            {
                Console.WriteLine("\nGreat job! Consistency is key to mindfulness practice.");
                Console.WriteLine("Remember: Even a few minutes of mindfulness each day can make a difference.");
            }

            Console.WriteLine("\nPress any key to return to main menu...");
            Console.ReadKey();
        }
    }
}