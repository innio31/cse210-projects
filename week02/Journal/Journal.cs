using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Journal
{
    // Member variables
    public List<Entry> _entries = new List<Entry>();
    public string _journalName;
    public string _author;
    public DateTime _createdDate;
    public DateTime _lastModified;
    public string _theme;
    public List<string> _categories;

    // Statistics cache (performance optimization)
    private Dictionary<string, int> _moodStats;
    private Dictionary<string, int> _tagStats;
    private DateTime _lastStatsUpdate;

    // Constructor
    public Journal(string name = "My Journal", string author = "Anonymous")
    {
        _journalName = name;
        _author = author;
        _createdDate = DateTime.Now;
        _lastModified = DateTime.Now;
        _theme = "Default";
        _categories = new List<string>();
        _moodStats = new Dictionary<string, int>();
        _tagStats = new Dictionary<string, int>();
        _lastStatsUpdate = DateTime.MinValue;
    }

    // ========== CORE FUNCTIONALITY ==========

    // Method to add a new entry
    public void AddEntry(Entry newEntry)
    {
        _entries.Add(newEntry);
        _lastModified = DateTime.Now;
        InvalidateStatsCache();

        // Auto-add entry tags to journal categories
        foreach (string tag in newEntry._tags)
        {
            if (!_categories.Contains(tag, StringComparer.OrdinalIgnoreCase))
            {
                _categories.Add(tag);
            }
        }

        Console.WriteLine($"✓ Entry added successfully! (Total entries: {_entries.Count})");
    }

    // Method to display all entries
    public void DisplayAll()
    {
        Console.Clear();
        Console.WriteLine($"\n📖 {_journalName.ToUpper()} 📖");
        Console.WriteLine($"👤 Author: {_author}");
        Console.WriteLine($"📊 Total Entries: {_entries.Count}");
        Console.WriteLine($"📅 Date Range: {GetDateRange()}");
        Console.WriteLine("═".PadRight(60, '═'));

        if (_entries.Count == 0)
        {
            Console.WriteLine("\n📭 No entries found. Start by writing a new entry!");
            Console.WriteLine("   Use option 1 from the main menu.\n");
            return;
        }

        // Display options for sorting/filtering
        Console.WriteLine("\nSort by: [1] Date (Newest First) | [2] Date (Oldest First) | [3] Mood | [4] Word Count");
        Console.Write("Filter by mood (or press Enter for all): ");
        string filterMood = Console.ReadLine();

        List<Entry> entriesToDisplay = _entries.ToList();

        // Apply filter if specified
        if (!string.IsNullOrWhiteSpace(filterMood))
        {
            entriesToDisplay = entriesToDisplay
                .Where(e => e._mood.Equals(filterMood, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (entriesToDisplay.Count == 0)
            {
                Console.WriteLine($"\nNo entries found with mood: {filterMood}");
                return;
            }
        }

        // Apply sorting
        Console.Write("Choose sort option (1-4): ");
        string sortChoice = Console.ReadLine();

        entriesToDisplay = sortChoice switch
        {
            "2" => entriesToDisplay.OrderBy(e => e._date).ToList(), // Oldest first
            "3" => entriesToDisplay.OrderBy(e => e._mood).ToList(), // By mood
            "4" => entriesToDisplay.OrderByDescending(e => e._wordCount).ToList(), // By word count
            _ => entriesToDisplay.OrderByDescending(e => e._date).ToList() // Default: newest first
        };

        // Display entries
        int entryNumber = 1;
        foreach (Entry entry in entriesToDisplay)
        {
            Console.WriteLine($"\n📄 ENTRY #{entryNumber++}");
            entry.DisplayColorCoded();
        }

        Console.WriteLine($"\n📊 Showing {entriesToDisplay.Count} of {_entries.Count} total entries");
        if (!string.IsNullOrWhiteSpace(filterMood))
        {
            Console.WriteLine($"🔍 Filtered by mood: {filterMood}");
        }
    }

    // Method to display entries in compact view
    public void DisplayCompact()
    {
        Console.WriteLine($"\n📓 {_journalName} - Compact View");
        Console.WriteLine("─".PadRight(50, '─'));

        if (_entries.Count == 0)
        {
            Console.WriteLine("No entries available.");
            return;
        }

        int index = 1;
        foreach (Entry entry in _entries.OrderByDescending(e => e._date))
        {
            Console.Write($"{index:00}. ");
            entry.DisplayCompact();
            index++;
        }
    }

    // ========== FILE OPERATIONS ==========

    // Save journal to a CSV file
    public void SaveToFile(string filename, string format = "csv")
    {
        try
        {
            UpdateStatsCache(); // Ensure stats are current

            if (format.ToLower() == "csv")
            {
                SaveToCsv(filename);
            }
            else if (format.ToLower() == "json")
            {
                SaveToJson(filename);
            }
            else
            {
                SaveToLegacyFormat(filename);
            }

            Console.WriteLine($"\n✅ Journal saved successfully!");
            Console.WriteLine($"   📍 Location: {Path.GetFullPath(filename)}");
            Console.WriteLine($"   📊 Entries: {_entries.Count}");
            Console.WriteLine($"   💾 Format: {format.ToUpper()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error saving file: {ex.Message}");
            Console.WriteLine("   Please try a different filename or location.");
        }
    }

    private void SaveToCsv(string filename)
    {
        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            // Write header
            outputFile.WriteLine("Date,Prompt,Entry,Mood,EnergyLevel,Location,Tags,WordCount");

            // Write journal metadata as comments
            outputFile.WriteLine($"# Journal: {_journalName}");
            outputFile.WriteLine($"# Author: {_author}");
            outputFile.WriteLine($"# Created: {_createdDate:yyyy-MM-dd HH:mm:ss}");
            outputFile.WriteLine($"# Last Modified: {_lastModified:yyyy-MM-dd HH:mm:ss}");
            outputFile.WriteLine($"# Theme: {_theme}");
            outputFile.WriteLine($"# Categories: {string.Join(",", _categories)}");

            // Write entries
            foreach (Entry entry in _entries)
            {
                outputFile.WriteLine(entry.ToFileString());
            }
        }
    }

    private void SaveToJson(string filename)
    {
        var journalData = new
        {
            JournalName = _journalName,
            Author = _author,
            CreatedDate = _createdDate,
            LastModified = _lastModified,
            Theme = _theme,
            Categories = _categories,
            Entries = _entries
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string json = JsonSerializer.Serialize(journalData, options);
        File.WriteAllText(filename, json);
    }

    private void SaveToLegacyFormat(string filename)
    {
        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            // Write metadata first
            outputFile.WriteLine($"JOURNAL:{_journalName}");
            outputFile.WriteLine($"AUTHOR:{_author}");
            outputFile.WriteLine($"CREATED:{_createdDate:yyyy-MM-dd}");

            foreach (Entry entry in _entries)
            {
                // Pipe-separated format for backward compatibility
                outputFile.WriteLine($"{entry._date}|{entry._promptText}|{entry._entryText}|{entry._mood}|{entry._energyLevel}|{entry._location}|{string.Join(",", entry._tags)}");
            }
        }
    }

    // Load journal from a file
    public void LoadFromFile(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"\n❌ File '{filename}' not found.");
                return;
            }

            string firstLine = File.ReadLines(filename).FirstOrDefault() ?? "";

            if (firstLine.StartsWith("{")) // JSON format
            {
                LoadFromJson(filename);
            }
            else if (firstLine.Contains(",") && firstLine.Contains("Date") && firstLine.Contains("Prompt")) // CSV with header
            {
                LoadFromCsv(filename);
            }
            else // Legacy format
            {
                LoadFromLegacyFormat(filename);
            }

            _lastModified = DateTime.Now;
            InvalidateStatsCache();

            Console.WriteLine($"\n✅ Journal loaded successfully!");
            Console.WriteLine($"   📖 Journal: {_journalName}");
            Console.WriteLine($"   👤 Author: {_author}");
            Console.WriteLine($"   📊 Entries loaded: {_entries.Count}");
            Console.WriteLine($"   📅 Created: {_createdDate:yyyy-MM-dd}");

            ShowQuickStats();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error loading file: {ex.Message}");
            Console.WriteLine("   The file may be corrupted or in an unsupported format.");
        }
    }

    private void LoadFromCsv(string filename)
    {
        _entries.Clear();

        string[] lines = File.ReadAllLines(filename);
        bool readingEntries = false;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            if (line.StartsWith("Date,Prompt,Entry,Mood,EnergyLevel,Location,Tags,WordCount"))
            {
                readingEntries = true;
                continue;
            }

            if (readingEntries)
            {
                Entry entry = Entry.FromFileString(line);
                if (entry != null)
                {
                    _entries.Add(entry);
                }
            }
            else if (line.StartsWith("# Journal:"))
            {
                _journalName = line.Substring(10).Trim();
            }
            else if (line.StartsWith("# Author:"))
            {
                _author = line.Substring(9).Trim();
            }
            else if (line.StartsWith("# Created:"))
            {
                DateTime.TryParse(line.Substring(10).Trim(), out _createdDate);
            }
            else if (line.StartsWith("# Categories:"))
            {
                string categoriesStr = line.Substring(13).Trim();
                if (!string.IsNullOrEmpty(categoriesStr))
                {
                    _categories = new List<string>(categoriesStr.Split(','));
                }
            }
        }
    }

    private void LoadFromJson(string filename)
    {
        string json = File.ReadAllText(filename);

        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;

        _journalName = root.GetProperty("journalName").GetString();
        _author = root.GetProperty("author").GetString();
        _createdDate = root.GetProperty("createdDate").GetDateTime();
        _lastModified = root.GetProperty("lastModified").GetDateTime();
        _theme = root.GetProperty("theme").GetString();

        _categories.Clear();
        foreach (var category in root.GetProperty("categories").EnumerateArray())
        {
            _categories.Add(category.GetString());
        }

        _entries.Clear();
        foreach (var entryElement in root.GetProperty("entries").EnumerateArray())
        {
            Entry entry = new Entry();
            entry._date = entryElement.GetProperty("_date").GetString();
            entry._promptText = entryElement.GetProperty("_promptText").GetString();
            entry._entryText = entryElement.GetProperty("_entryText").GetString();
            entry._mood = entryElement.GetProperty("_mood").GetString();
            entry._energyLevel = entryElement.GetProperty("_energyLevel").GetInt32();
            entry._location = entryElement.GetProperty("_location").GetString();
            entry._wordCount = entryElement.GetProperty("_wordCount").GetInt32();

            entry._tags = new List<string>();
            foreach (var tag in entryElement.GetProperty("_tags").EnumerateArray())
            {
                entry._tags.Add(tag.GetString());
            }

            _entries.Add(entry);
        }
    }

    private void LoadFromLegacyFormat(string filename)
    {
        _entries.Clear();

        string[] lines = File.ReadAllLines(filename);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("JOURNAL:"))
            {
                _journalName = line.Substring(8).Trim();
            }
            else if (line.StartsWith("AUTHOR:"))
            {
                _author = line.Substring(7).Trim();
            }
            else if (line.StartsWith("CREATED:"))
            {
                DateTime.TryParse(line.Substring(8).Trim(), out _createdDate);
            }
            else
            {
                string[] parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    Entry entry = new Entry();
                    entry._date = parts[0];
                    entry._promptText = parts[1];
                    entry._entryText = parts[2];

                    if (parts.Length > 3) entry._mood = parts[3];
                    if (parts.Length > 4) int.TryParse(parts[4], out entry._energyLevel);
                    if (parts.Length > 5) entry._location = parts[5];

                    if (parts.Length > 6 && !string.IsNullOrEmpty(parts[6]))
                    {
                        entry._tags = new List<string>(parts[6].Split(','));
                    }

                    entry.UpdateWordCount();
                    _entries.Add(entry);
                }
            }
        }
    }

    // ========== ENTRY MANAGEMENT ==========

    // Edit an existing entry
    public void EditEntry(int index)
    {
        if (index < 1 || index > _entries.Count)
        {
            Console.WriteLine($"Invalid entry number. Please choose between 1 and {_entries.Count}.");
            return;
        }

        Entry entryToEdit = _entries[index - 1];

        Console.WriteLine($"\n✏️  EDITING ENTRY #{index}");
        Console.WriteLine($"Date: {entryToEdit._date}");
        Console.WriteLine($"Current prompt: {entryToEdit._promptText}");
        Console.WriteLine($"Current entry: {entryToEdit._entryText}");
        Console.WriteLine("\nWhat would you like to edit?");
        Console.WriteLine("[1] Entry text");
        Console.WriteLine("[2] Mood");
        Console.WriteLine("[3] Energy level");
        Console.WriteLine("[4] Tags");
        Console.WriteLine("[5] Location");
        Console.Write("Choose (1-5): ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("New entry text: ");
                entryToEdit._entryText = Console.ReadLine();
                entryToEdit.UpdateWordCount();
                break;
            case "2":
                Console.Write("New mood: ");
                entryToEdit._mood = Console.ReadLine();
                break;
            case "3":
                Console.Write("New energy level (1-10): ");
                if (int.TryParse(Console.ReadLine(), out int energy))
                {
                    entryToEdit._energyLevel = Math.Clamp(energy, 1, 10);
                }
                break;
            case "4":
                Console.WriteLine($"Current tags: {string.Join(", ", entryToEdit._tags)}");
                Console.Write("Enter new tags (comma-separated): ");
                string[] newTags = Console.ReadLine().Split(',');
                entryToEdit._tags = new List<string>(newTags.Select(t => t.Trim()));
                break;
            case "5":
                Console.Write("New location: ");
                entryToEdit._location = Console.ReadLine();
                break;
        }

        _lastModified = DateTime.Now;
        InvalidateStatsCache();
        Console.WriteLine("✅ Entry updated successfully!");
    }

    // Delete an entry
    public void DeleteEntry(int index)
    {
        if (index < 1 || index > _entries.Count)
        {
            Console.WriteLine($"Invalid entry number. Please choose between 1 and {_entries.Count}.");
            return;
        }

        Entry entryToDelete = _entries[index - 1];

        Console.WriteLine($"\n🗑️  DELETING ENTRY #{index}");
        entryToDelete.DisplayCompact();
        Console.Write("\nAre you sure you want to delete this entry? (y/n): ");

        if (Console.ReadLine().ToLower() == "y")
        {
            _entries.RemoveAt(index - 1);
            _lastModified = DateTime.Now;
            InvalidateStatsCache();
            Console.WriteLine("✅ Entry deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    // Search entries
    public void SearchEntries(string keyword = "")
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.Write("Enter search keyword: ");
            keyword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(keyword))
                return;
        }

        Console.WriteLine($"\n🔍 SEARCH RESULTS FOR: '{keyword}'");
        Console.WriteLine("─".PadRight(50, '─'));

        var results = _entries.Where(e =>
            e._entryText.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            e._promptText.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            e._mood.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            e._location.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            e._tags.Any(t => t.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No entries found matching your search.");
            return;
        }

        foreach (Entry entry in results)
        {
            entry.DisplayCompact();
        }

        Console.WriteLine($"\n📊 Found {results.Count} matching entries.");

        // Show search statistics
        var moodBreakdown = results.GroupBy(e => e._mood)
                                  .Select(g => new { Mood = g.Key, Count = g.Count() })
                                  .OrderByDescending(x => x.Count);

        if (moodBreakdown.Any())
        {
            Console.WriteLine("\n📈 Mood breakdown in search results:");
            foreach (var mood in moodBreakdown)
            {
                Console.WriteLine($"   {mood.Mood}: {mood.Count} entries");
            }
        }
    }

    // Search by date range
    public void SearchByDateRange()
    {
        Console.Write("Enter start date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.WriteLine("Invalid start date.");
            return;
        }

        Console.Write("Enter end date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
        {
            Console.WriteLine("Invalid end date.");
            return;
        }

        endDate = endDate.AddDays(1).AddSeconds(-1); // Include entire end day

        var results = _entries.Where(e =>
        {
            if (DateTime.TryParse(e._date, out DateTime entryDate))
            {
                return entryDate >= startDate && entryDate <= endDate;
            }
            return false;
        }).ToList();

        Console.WriteLine($"\n📅 Entries from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
        Console.WriteLine("─".PadRight(50, '─'));

        if (results.Count == 0)
        {
            Console.WriteLine("No entries found in this date range.");
            return;
        }

        foreach (Entry entry in results.OrderBy(e => e._date))
        {
            entry.DisplayCompact();
        }

        Console.WriteLine($"\n📊 Found {results.Count} entries in this date range.");
    }

    // ========== STATISTICS & ANALYTICS ==========

    // Show comprehensive statistics
    public void ShowStatistics()
    {
        UpdateStatsCache();

        Console.Clear();
        Console.WriteLine($"\n📈 JOURNAL STATISTICS");
        Console.WriteLine($"📖 {_journalName}");
        Console.WriteLine("═".PadRight(60, '═'));

        Console.WriteLine($"\n📊 OVERVIEW");
        Console.WriteLine($"   Total Entries: {_entries.Count}");
        Console.WriteLine($"   Date Range: {GetDateRange()}");
        Console.WriteLine($"   Average Entries Per Day: {GetAverageEntriesPerDay():F2}");
        Console.WriteLine($"   Total Words: {GetTotalWordCount():N0}");
        Console.WriteLine($"   Average Words Per Entry: {GetAverageWordCount():F0}");
        Console.WriteLine($"   Total Reading Time: {GetTotalReadingTime():F1} minutes");

        Console.WriteLine($"\n😊 MOOD ANALYSIS");
        if (_moodStats.Count > 0)
        {
            foreach (var mood in _moodStats.OrderByDescending(m => m.Value))
            {
                int percentage = (int)((double)mood.Value / _entries.Count * 100);
                string bar = new string('█', percentage / 5);
                Console.WriteLine($"   {mood.Key,-12} {mood.Value,3} entries {bar} {percentage}%");
            }

            string mostCommonMood = _moodStats.OrderByDescending(m => m.Value).First().Key;
            Console.WriteLine($"\n   Most common mood: {mostCommonMood}");
        }

        Console.WriteLine($"\n🏷️  TAG ANALYSIS");
        if (_tagStats.Count > 0)
        {
            var topTags = _tagStats.OrderByDescending(t => t.Value).Take(10);
            foreach (var tag in topTags)
            {
                Console.WriteLine($"   #{tag.Key,-15} {tag.Value,3} entries");
            }

            Console.WriteLine($"\n   Total unique tags: {_tagStats.Count}");
        }

        Console.WriteLine($"\n📅 ACTIVITY OVER TIME");
        var entriesByMonth = _entries.GroupBy(e =>
        {
            if (DateTime.TryParse(e._date, out DateTime entryDate))
                return entryDate.ToString("yyyy-MM");
            return "Unknown";
        }).OrderBy(g => g.Key);

        foreach (var month in entriesByMonth)
        {
            Console.WriteLine($"   {month.Key}: {month.Count()} entries");
        }

        Console.WriteLine($"\n⚡ ENERGY LEVELS");
        if (_entries.Any(e => e._energyLevel > 0))
        {
            double avgEnergy = _entries.Average(e => e._energyLevel);
            Console.WriteLine($"   Average energy level: {avgEnergy:F1}/10");

            var energyDistribution = _entries.GroupBy(e => (e._energyLevel - 1) / 2) // Group into ranges
                                            .OrderBy(g => g.Key)
                                            .Select(g => new
                                            {
                                                Range = $"{g.Key * 2 + 1}-{g.Key * 2 + 2}",
                                                Count = g.Count()
                                            });

            foreach (var range in energyDistribution)
            {
                int percentage = (int)((double)range.Count / _entries.Count * 100);
                Console.WriteLine($"   {range.Range}/10: {range.Count,3} entries ({percentage}%)");
            }
        }

        Console.WriteLine($"\n📍 LOCATIONS");
        var locations = _entries.GroupBy(e => e._location)
                               .OrderByDescending(g => g.Count())
                               .Take(5);

        foreach (var location in locations)
        {
            Console.WriteLine($"   {location.Key,-20} {location.Count(),3} entries");
        }

        Console.WriteLine($"\n📝 WRITING HABITS");
        var bestDay = GetMostProductiveDay();
        Console.WriteLine($"   Most productive day: {bestDay}");

        var longestEntry = _entries.OrderByDescending(e => e._wordCount).FirstOrDefault();
        var shortestEntry = _entries.OrderBy(e => e._wordCount).FirstOrDefault();

        if (longestEntry != null)
        {
            Console.WriteLine($"   Longest entry: {longestEntry._wordCount} words");
        }
        if (shortestEntry != null)
        {
            Console.WriteLine($"   Shortest entry: {shortestEntry._wordCount} words");
        }

        Console.WriteLine($"\n🎯 CONSISTENCY");
        Console.WriteLine($"   Current streak: {GetCurrentStreak()} days");
        Console.WriteLine($"   Longest streak: {GetLongestStreak()} days");
        Console.WriteLine($"   Last entry: {GetLastEntryDate():yyyy-MM-dd}");
    }

    // Quick statistics display
    public void ShowQuickStats()
    {
        if (_entries.Count == 0) return;

        Console.WriteLine($"\n📊 QUICK STATS:");
        Console.WriteLine($"   • Entries: {_entries.Count}");
        Console.WriteLine($"   • Date Range: {GetDateRange()}");
        Console.WriteLine($"   • Total Words: {GetTotalWordCount():N0}");

        if (_entries.Any(e => !string.IsNullOrEmpty(e._mood)))
        {
            var topMood = _entries.GroupBy(e => e._mood)
                                 .OrderByDescending(g => g.Count())
                                 .First();
            Console.WriteLine($"   • Most Common Mood: {topMood.Key}");
        }

        Console.WriteLine($"   • Last Modified: {_lastModified:yyyy-MM-dd HH:mm}");
    }

    // ========== UTILITY METHODS ==========

    // Update statistics cache
    private void UpdateStatsCache()
    {
        if (_lastStatsUpdate.AddMinutes(5) > DateTime.Now && _entries.Count > 0)
            return;

        // Update mood statistics
        _moodStats.Clear();
        foreach (Entry entry in _entries)
        {
            if (_moodStats.ContainsKey(entry._mood))
                _moodStats[entry._mood]++;
            else
                _moodStats[entry._mood] = 1;
        }

        // Update tag statistics
        _tagStats.Clear();
        foreach (Entry entry in _entries)
        {
            foreach (string tag in entry._tags)
            {
                if (_tagStats.ContainsKey(tag))
                    _tagStats[tag]++;
                else
                    _tagStats[tag] = 1;
            }
        }

        _lastStatsUpdate = DateTime.Now;
    }

    // Invalidate statistics cache
    private void InvalidateStatsCache()
    {
        _lastStatsUpdate = DateTime.MinValue;
    }

    // Get date range of entries
    public string GetDateRange()
    {
        if (_entries.Count == 0)
            return "No entries";

        var dates = _entries.Select(e =>
        {
            DateTime.TryParse(e._date, out DateTime dt);
            return dt;
        }).Where(dt => dt != DateTime.MinValue).ToList();

        if (dates.Count == 0)
            return "Unknown";

        DateTime minDate = dates.Min();
        DateTime maxDate = dates.Max();

        if (minDate.Date == maxDate.Date)
            return minDate.ToString("yyyy-MM-dd");

        return $"{minDate:yyyy-MM-dd} to {maxDate:yyyy-MM-dd}";
    }

    // Get total word count
    public int GetTotalWordCount()
    {
        return _entries.Sum(e => e._wordCount);
    }

    // Get average word count per entry
    private double GetAverageWordCount()
    {
        return _entries.Count > 0 ? _entries.Average(e => e._wordCount) : 0;
    }

    // Get total reading time
    private double GetTotalReadingTime()
    {
        return _entries.Sum(e => e.GetReadingTimeMinutes());
    }

    // Get most productive day of week
    private string GetMostProductiveDay()
    {
        if (_entries.Count == 0)
            return "No data";

        var dayCounts = _entries.GroupBy(e =>
        {
            if (DateTime.TryParse(e._date, out DateTime entryDate))
                return entryDate.DayOfWeek.ToString();
            return "Unknown";
        }).OrderByDescending(g => g.Count()).First();

        return $"{dayCounts.Key} ({dayCounts.Count()} entries)";
    }

    // Get current writing streak
    public int GetCurrentStreak()
    {
        if (_entries.Count == 0)
            return 0;

        var sortedDates = _entries.Select(e =>
        {
            DateTime.TryParse(e._date, out DateTime dt);
            return dt.Date;
        }).Distinct().OrderByDescending(d => d).ToList();

        if (sortedDates.Count == 0)
            return 0;

        int streak = 1;
        for (int i = 1; i < sortedDates.Count; i++)
        {
            if ((sortedDates[i - 1] - sortedDates[i]).Days == 1)
                streak++;
            else
                break;
        }

        return streak;
    }

    // Get longest writing streak
    private int GetLongestStreak()
    {
        if (_entries.Count == 0)
            return 0;

        var dates = _entries.Select(e =>
        {
            DateTime.TryParse(e._date, out DateTime dt);
            return dt.Date;
        }).Distinct().OrderBy(d => d).ToList();

        if (dates.Count == 0)
            return 0;

        int longestStreak = 1;
        int currentStreak = 1;

        for (int i = 1; i < dates.Count; i++)
        {
            if ((dates[i] - dates[i - 1]).Days == 1)
            {
                currentStreak++;
                longestStreak = Math.Max(longestStreak, currentStreak);
            }
            else
            {
                currentStreak = 1;
            }
        }

        return longestStreak;
    }

    // Get last entry date
    private DateTime GetLastEntryDate()
    {
        if (_entries.Count == 0)
            return DateTime.MinValue;

        var dates = _entries.Select(e =>
        {
            DateTime.TryParse(e._date, out DateTime dt);
            return dt;
        }).Where(dt => dt != DateTime.MinValue).ToList();

        return dates.Count > 0 ? dates.Max() : DateTime.MinValue;
    }

    // Get average entries per day
    private double GetAverageEntriesPerDay()
    {
        var dates = _entries.Select(e =>
        {
            DateTime.TryParse(e._date, out DateTime dt);
            return dt.Date;
        }).Distinct().ToList();

        if (dates.Count == 0)
            return 0;

        double totalDays = (DateTime.Now.Date - dates.Min()).TotalDays + 1;
        return dates.Count / totalDays;
    }

    // ========== EXPORT FUNCTIONALITY ==========

    // Export to text file
    public void ExportToText(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine($"JOURNAL: {_journalName}");
                writer.WriteLine($"AUTHOR: {_author}");
                writer.WriteLine($"EXPORT DATE: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('=', 60));
                writer.WriteLine();

                int entryNumber = 1;
                foreach (Entry entry in _entries.OrderBy(e => e._date))
                {
                    writer.WriteLine($"ENTRY #{entryNumber++}");
                    writer.WriteLine($"Date: {entry._date}");
                    writer.WriteLine($"Mood: {entry._mood}");
                    writer.WriteLine($"Location: {entry._location}");
                    writer.WriteLine($"Prompt: {entry._promptText}");
                    writer.WriteLine();
                    writer.WriteLine(entry._entryText);
                    writer.WriteLine();
                    writer.WriteLine($"Word Count: {entry._wordCount} | Tags: {string.Join(", ", entry._tags)}");
                    writer.WriteLine(new string('-', 60));
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"✅ Journal exported to {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error exporting: {ex.Message}");
        }
    }

    // Export statistics to CSV
    public void ExportStatsToCsv(string filename)
    {
        try
        {
            UpdateStatsCache();

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("Statistic,Value");
                writer.WriteLine($"Total Entries,{_entries.Count}");
                writer.WriteLine($"Date Range,{GetDateRange()}");
                writer.WriteLine($"Total Words,{GetTotalWordCount()}");
                writer.WriteLine($"Average Words Per Entry,{GetAverageWordCount():F2}");
                writer.WriteLine($"Current Streak,{GetCurrentStreak()}");
                writer.WriteLine($"Longest Streak,{GetLongestStreak()}");

                writer.WriteLine(",,");
                writer.WriteLine("Mood,Count,Percentage");
                foreach (var mood in _moodStats.OrderByDescending(m => m.Value))
                {
                    double percentage = (double)mood.Value / _entries.Count * 100;
                    writer.WriteLine($"{mood.Key},{mood.Value},{percentage:F1}%");
                }

                writer.WriteLine(",,");
                writer.WriteLine("Top Tags,Count");
                foreach (var tag in _tagStats.OrderByDescending(t => t.Value).Take(20))
                {
                    writer.WriteLine($"{tag.Key},{tag.Value}");
                }
            }

            Console.WriteLine($"✅ Statistics exported to {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error exporting statistics: {ex.Message}");
        }
    }

    // ========== MAINTENANCE & UTILITY ==========

    // Clean up empty entries
    public void Cleanup()
    {
        int initialCount = _entries.Count;
        _entries = _entries.Where(e => !string.IsNullOrWhiteSpace(e._entryText)).ToList();
        int removedCount = initialCount - _entries.Count;

        if (removedCount > 0)
        {
            _lastModified = DateTime.Now;
            InvalidateStatsCache();
            Console.WriteLine($"✅ Removed {removedCount} empty entries.");
        }
        else
        {
            Console.WriteLine("✅ No empty entries found.");
        }
    }

    // Merge another journal into this one
    public void MergeJournal(Journal otherJournal)
    {
        int initialCount = _entries.Count;
        _entries.AddRange(otherJournal._entries);
        _entries = _entries.OrderBy(e => e._date).ToList();

        _lastModified = DateTime.Now;
        InvalidateStatsCache();

        Console.WriteLine($"✅ Merged {otherJournal._entries.Count} entries from '{otherJournal._journalName}'");
        Console.WriteLine($"   Total entries now: {_entries.Count}");
    }

    // Backup journal
    public void CreateBackup()
    {
        string backupDir = "backups";
        if (!Directory.Exists(backupDir))
            Directory.CreateDirectory(backupDir);

        string backupFile = Path.Combine(backupDir, $"{_journalName}_{DateTime.Now:yyyyMMdd_HHmmss}.json");
        SaveToJson(backupFile);

        Console.WriteLine($"✅ Backup created: {backupFile}");

        // Clean up old backups (keep last 10)
        var backupFiles = Directory.GetFiles(backupDir, "*.json")
                                  .OrderByDescending(f => f)
                                  .Skip(10);

        foreach (string oldBackup in backupFiles)
        {
            File.Delete(oldBackup);
        }
    }

    // Change journal settings
    public void ChangeSettings()
    {
        Console.WriteLine($"\n⚙️  JOURNAL SETTINGS");
        Console.WriteLine($"Current name: {_journalName}");
        Console.Write("New name (or Enter to keep current): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            _journalName = newName;

        Console.WriteLine($"Current author: {_author}");
        Console.Write("New author (or Enter to keep current): ");
        string newAuthor = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newAuthor))
            _author = newAuthor;

        Console.WriteLine($"Current theme: {_theme}");
        Console.Write("New theme (or Enter to keep current): ");
        string newTheme = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newTheme))
            _theme = newTheme;

        _lastModified = DateTime.Now;
        Console.WriteLine("✅ Journal settings updated!");
    }
}