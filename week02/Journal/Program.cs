using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static Journal myJournal = new Journal("My Daily Journal", Environment.UserName);
    static PromptGenerator promptGenerator = new PromptGenerator();

    static void Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        catch
        {
            // Ignore encoding errors
        }

        ShowWelcomeScreen();

        string userChoice = "";

        // Main menu loop
        while (userChoice != "0")
        {
            DisplayMainMenu();
            Console.Write("\nSelect an option (0-12): ");
            userChoice = Console.ReadLine();

            if (userChoice == null) // Handle Ctrl+Z or input redirection
            {
                Console.WriteLine("\nExiting program...");
                break;
            }

            Console.WriteLine();

            ProcessMenuChoice(userChoice);
        }
    }

    static void ShowWelcomeScreen()
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("                 PERSONAL JOURNAL APP                 ");
        Console.WriteLine("               Your Digital Reflection Space          ");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine();
        Console.WriteLine($"👤 Welcome, {myJournal._author}!");
        Console.WriteLine($"📅 Today is: {DateTime.Now:dddd, MMMM dd, yyyy}");
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("                      MAIN MENU                       ");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine();
        Console.WriteLine("📝 WRITE & READ");
        Console.WriteLine("─────────────────────────────────────────────────────");
        Console.WriteLine("  1. ✍️  Write a new journal entry");
        Console.WriteLine("  2. 📖 Display all entries");
        Console.WriteLine("  3. 📋 Display compact view");
        Console.WriteLine();
        Console.WriteLine("🔍 SEARCH & MANAGE");
        Console.WriteLine("─────────────────────────────────────────────────────");
        Console.WriteLine("  4. 🔎 Search entries");
        Console.WriteLine("  5. 📅 Search by date range");
        Console.WriteLine("  6. ✏️  Edit an entry");
        Console.WriteLine("  7. 🗑️  Delete an entry");
        Console.WriteLine();
        Console.WriteLine("💾 FILE OPERATIONS");
        Console.WriteLine("─────────────────────────────────────────────────────");
        Console.WriteLine("  8. 💾 Save journal to file");
        Console.WriteLine("  9. 📂 Load journal from file");
        Console.WriteLine("  10. 📤 Export to text file");
        Console.WriteLine("  11. 💾 Create backup");
        Console.WriteLine();
        Console.WriteLine("📊 ANALYTICS & TOOLS");
        Console.WriteLine("─────────────────────────────────────────────────────");
        Console.WriteLine("  12. 📈 View detailed statistics");
        Console.WriteLine("  13. ⚙️  Change journal settings");
        Console.WriteLine("  14. 🧹 Clean up empty entries");
        Console.WriteLine();
        Console.WriteLine("  0. 🚪 Exit");
        Console.WriteLine("═══════════════════════════════════════════════════════");

        // Show quick stats if we have entries
        if (myJournal._entries.Count > 0)
        {
            Console.WriteLine("\n📊 JOURNAL SNAPSHOT:");
            Console.WriteLine($"   • 📝 Entries: {myJournal._entries.Count}");

            try
            {
                // Get most common mood
                var moodGroups = myJournal._entries
                    .GroupBy(e => e._mood)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                if (moodGroups != null)
                {
                    Console.WriteLine($"   • 😊 Most common mood: {moodGroups.Key}");
                }

                // Get last entry date
                var lastEntry = myJournal._entries
                    .OrderByDescending(e => e._date)
                    .FirstOrDefault();

                if (lastEntry != null && DateTime.TryParse(lastEntry._date, out DateTime lastDate))
                {
                    Console.WriteLine($"   • 📅 Last entry: {lastDate:yyyy-MM-dd}");
                }
            }
            catch
            {
                // Ignore errors in stats display
            }
        }
    }

    static void ProcessMenuChoice(string choice)
    {
        switch (choice)
        {
            case "0":
                HandleExit();
                break;

            case "1":
                WriteNewEntry();
                break;

            case "2":
                myJournal.DisplayAll();
                WaitForContinue();
                break;

            case "3":
                myJournal.DisplayCompact();
                WaitForContinue();
                break;

            case "4":
                myJournal.SearchEntries();
                WaitForContinue();
                break;

            case "5":
                myJournal.SearchByDateRange();
                WaitForContinue();
                break;

            case "6":
                EditEntry();
                break;

            case "7":
                DeleteEntry();
                break;

            case "8":
                SaveJournal();
                break;

            case "9":
                LoadJournal();
                break;

            case "10":
                ExportToText();
                break;

            case "11":
                myJournal.CreateBackup();
                WaitForContinue();
                break;

            case "12":
                myJournal.ShowStatistics();
                WaitForContinue();
                break;

            case "13":
                myJournal.ChangeSettings();
                WaitForContinue();
                break;

            case "14":
                myJournal.Cleanup();
                WaitForContinue();
                break;

            default:
                Console.WriteLine("❌ Invalid option. Please try again.");
                WaitForContinue();
                break;
        }
    }

    static void WriteNewEntry()
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("                  WRITE NEW ENTRY                     ");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine();

        // Get random prompt
        string prompt = promptGenerator.GetRandomPrompt();
        Console.WriteLine($"📝 Prompt: {prompt}");
        Console.WriteLine();

        // Get entry text
        Console.WriteLine("💭 Write your thoughts (press Enter twice when finished):");
        Console.WriteLine("─────────────────────────────────────────────────────");

        List<string> lines = new List<string>();
        string line;
        int emptyLines = 0;

        do
        {
            line = Console.ReadLine();

            if (line == null) // Handle input redirection or Ctrl+Z
            {
                Console.WriteLine("\nInput cancelled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                emptyLines++;
            }
            else
            {
                lines.Add(line);
                emptyLines = 0;
            }

            // Stop after two consecutive empty lines
        } while (emptyLines < 2);

        string entryText = string.Join("\n", lines);

        if (string.IsNullOrWhiteSpace(entryText))
        {
            Console.WriteLine("\n❌ Entry was empty. Not saved.");
            WaitForContinue();
            return;
        }

        // Get additional metadata
        Console.WriteLine("\n📊 Additional Information (optional):");
        Console.WriteLine("─────────────────────────────────────────────────────");

        Console.Write("😊 How are you feeling? (e.g., Happy, Tired, Excited): ");
        string mood = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(mood)) mood = "Neutral";

        Console.Write("⚡ Energy level (1-10, Enter for 5): ");
        string energyInput = Console.ReadLine();
        int energy = 5;
        if (!string.IsNullOrWhiteSpace(energyInput) && int.TryParse(energyInput, out int e))
        {
            energy = Math.Clamp(e, 1, 10);
        }

        Console.Write("📍 Where are you writing from? (e.g., Home, Office, Cafe): ");
        string location = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(location)) location = "Unknown";

        Console.Write("🏷️  Tags (comma-separated, e.g., Work, Family, Reflection): ");
        string tagsInput = Console.ReadLine();
        List<string> tags = new List<string>();
        if (!string.IsNullOrWhiteSpace(tagsInput))
        {
            tags = tagsInput.Split(',')
                           .Select(t => t.Trim())
                           .Where(t => !string.IsNullOrEmpty(t))
                           .ToList();
        }

        // Create and add entry
        Entry newEntry = new Entry(
            prompt: prompt,
            response: entryText,
            mood: mood,
            energy: energy,
            location: location,
            tags: tags
        );

        myJournal.AddEntry(newEntry);

        // Show entry preview
        Console.WriteLine("\n✅ ENTRY SAVED SUCCESSFULLY!");
        Console.WriteLine("─────────────────────────────────────────────────────");
        newEntry.DisplayCompact();

        WaitForContinue();
    }

    static void EditEntry()
    {
        if (myJournal._entries.Count == 0)
        {
            Console.WriteLine("❌ No entries to edit.");
            WaitForContinue();
            return;
        }

        Console.WriteLine("\n✏️  EDIT ENTRY");
        Console.WriteLine("─────────────────────────────────────────────────────");
        myJournal.DisplayCompact();

        Console.Write($"\nEnter entry number to edit (1-{myJournal._entries.Count}): ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            myJournal.EditEntry(index);
        }
        else
        {
            Console.WriteLine("❌ Invalid entry number.");
        }

        WaitForContinue();
    }

    static void DeleteEntry()
    {
        if (myJournal._entries.Count == 0)
        {
            Console.WriteLine("❌ No entries to delete.");
            WaitForContinue();
            return;
        }

        Console.WriteLine("\n🗑️  DELETE ENTRY");
        Console.WriteLine("─────────────────────────────────────────────────────");
        myJournal.DisplayCompact();

        Console.Write($"\nEnter entry number to delete (1-{myJournal._entries.Count}): ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            myJournal.DeleteEntry(index);
        }
        else
        {
            Console.WriteLine("❌ Invalid entry number.");
        }

        WaitForContinue();
    }

    static void SaveJournal()
    {
        Console.WriteLine("\n💾 SAVE JOURNAL");
        Console.WriteLine("─────────────────────────────────────────────────────");

        Console.WriteLine("\nChoose format:");
        Console.WriteLine("1. CSV (Recommended - Excel compatible)");
        Console.WriteLine("2. JSON (Modern - preserves all data)");
        Console.WriteLine("3. Legacy format (Simple text)");
        Console.Write("Choice (1-3): ");

        string formatChoice = Console.ReadLine();
        string format = formatChoice switch
        {
            "1" => "csv",
            "2" => "json",
            "3" => "txt",
            _ => "csv"
        };

        Console.Write($"\nEnter filename (e.g., myjournal.{format}): ");
        string filename = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filename))
        {
            filename = $"journal_{DateTime.Now:yyyyMMdd}.{format}";
        }

        myJournal.SaveToFile(filename, format);

        WaitForContinue();
    }

    static void LoadJournal()
    {
        Console.WriteLine("\n📂 LOAD JOURNAL");
        Console.WriteLine("─────────────────────────────────────────────────────");

        // Show available files in current directory
        var files = Directory.GetFiles(".", "*.*")
                           .Where(f => f.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ||
                                       f.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ||
                                       f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                           .Select(f => Path.GetFileName(f))
                           .ToList();

        if (files.Count > 0)
        {
            Console.WriteLine("\n📁 Available files:");
            foreach (string file in files)
            {
                Console.WriteLine($"   • {file}");
            }
        }

        Console.Write("\nEnter filename to load (or Enter to browse): ");
        string filename = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.Write("Enter full path to journal file: ");
            filename = Console.ReadLine();
        }

        if (!string.IsNullOrWhiteSpace(filename))
        {
            myJournal.LoadFromFile(filename);
        }
        else
        {
            Console.WriteLine("❌ No filename provided.");
        }

        WaitForContinue();
    }

    static void ExportToText()
    {
        if (myJournal._entries.Count == 0)
        {
            Console.WriteLine("❌ No entries to export.");
            WaitForContinue();
            return;
        }

        Console.WriteLine("\n📤 EXPORT TO TEXT FILE");
        Console.WriteLine("─────────────────────────────────────────────────────");

        Console.Write("Enter filename (e.g., journal_export.txt): ");
        string filename = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filename))
        {
            filename = $"journal_export_{DateTime.Now:yyyyMMdd}.txt";
        }

        myJournal.ExportToText(filename);
        WaitForContinue();
    }

    static void HandleExit()
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════");
        Console.WriteLine("                     EXIT JOURNAL                     ");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine();

        if (myJournal._entries.Count > 0)
        {
            Console.WriteLine($"📊 Journal Summary:");
            Console.WriteLine($"   • Total entries: {myJournal._entries.Count}");

            // Calculate date range manually
            var dates = myJournal._entries
                .Select(e =>
                {
                    DateTime.TryParse(e._date, out DateTime dt);
                    return dt;
                })
                .Where(dt => dt != DateTime.MinValue)
                .ToList();

            if (dates.Count > 0)
            {
                DateTime minDate = dates.Min();
                DateTime maxDate = dates.Max();
                string dateRange = minDate.Date == maxDate.Date ?
                    minDate.ToString("yyyy-MM-dd") :
                    $"{minDate:yyyy-MM-dd} to {maxDate:yyyy-MM-dd}";
                Console.WriteLine($"   • Date range: {dateRange}");
            }

            // Calculate total words manually
            int totalWords = myJournal._entries.Sum(e => e._wordCount);
            Console.WriteLine($"   • Total words: {totalWords:N0}");
            Console.WriteLine();

            Console.Write("💾 Would you like to save before exiting? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                SaveJournal();
            }
        }

        Console.WriteLine("\n✨ Thank you for journaling today!");
        Console.WriteLine("  Reflection is the first step toward growth. 🌱");
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }

    static void WaitForContinue()
    {
        Console.Write("\nPress Enter to continue...");
        Console.ReadLine();
    }
}