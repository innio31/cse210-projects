using System;

public class Entry
{
    // Enhanced member variables with additional metadata
    public string _date;
    public string _promptText;
    public string _entryText;
    public string _mood;                // User's mood/emotional state
    public int _energyLevel;           // Energy level on scale 1-10
    public string _location;           // Where the entry was written
    public List<string> _tags;         // Custom tags for categorization
    public int _wordCount;             // Auto-calculated word count

    // Constructor to initialize the entry with all enhanced features
    public Entry()
    {
        _tags = new List<string>();
        _date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        _energyLevel = 5; // Default medium energy
        _mood = "Neutral";
        _location = "Unknown";
    }

    // Constructor with parameters for easier creation
    public Entry(string prompt, string response, string mood = "Neutral",
                 int energy = 5, string location = "Unknown", List<string> tags = null)
    {
        _date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        _promptText = prompt;
        _entryText = response;
        _mood = mood;
        _energyLevel = Math.Clamp(energy, 1, 10); // Ensure between 1-10
        _location = location;
        _tags = tags ?? new List<string>();
        _wordCount = CountWords(response);
    }

    // Enhanced display method with all information
    public void Display()
    {
        Console.WriteLine("═".PadRight(50, '═'));
        Console.WriteLine($"📅 Date: {_date}");
        Console.WriteLine($"📍 Location: {_location}");
        Console.WriteLine($"😊 Mood: {_mood}");
        Console.WriteLine($"⚡ Energy Level: {GetEnergyIndicator()}");
        Console.WriteLine($"📝 Prompt: {_promptText}");
        Console.WriteLine($"📖 Entry: {_entryText}");
        Console.WriteLine($"📊 Stats: {_wordCount} words | {_entryText.Length} characters");

        if (_tags.Count > 0)
        {
            Console.Write($"🏷️  Tags: ");
            foreach (string tag in _tags)
            {
                Console.Write($"[{tag}] ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("═".PadRight(50, '═'));
        Console.WriteLine();
    }

    // Compact display for list views
    public void DisplayCompact()
    {
        string preview = _entryText.Length > 50 ? _entryText.Substring(0, 47) + "..." : _entryText;
        Console.WriteLine($"{_date} | {GetMoodEmoji()} {_mood} | {preview}");
    }

    // Display with color coding based on mood
    public void DisplayColorCoded()
    {
        ConsoleColor moodColor = GetMoodColor();

        Console.ForegroundColor = moodColor;
        Console.WriteLine($"╔══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║ Date: {_date,-38} ║");
        Console.WriteLine($"║ Mood: {_mood,-12} Energy: {GetEnergyIndicator(),-15} ║");
        Console.WriteLine($"╚══════════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine($"Prompt: {_promptText}");
        Console.WriteLine($"\n{_entryText}\n");

        if (_tags.Count > 0)
        {
            Console.Write("Tags: ");
            foreach (string tag in _tags)
            {
                Console.Write($"[{tag}] ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    // Method to format entry for file saving (CSV format with escaping)
    public string ToFileString()
    {
        // Escape commas and quotes for CSV format
        string escapedEntry = _entryText.Replace("\"", "\"\"");
        string escapedPrompt = _promptText.Replace("\"", "\"\"");
        string tagsString = string.Join(";", _tags);

        return $"\"{_date}\",\"{escapedPrompt}\",\"{escapedEntry}\",\"{_mood}\",{_energyLevel},\"{_location}\",\"{tagsString}\",{_wordCount}";
    }

    // Method to create entry from CSV string
    public static Entry FromFileString(string fileLine)
    {
        try
        {
            // Parse CSV line with quote handling
            var parts = ParseCsvLine(fileLine);

            if (parts.Count >= 8)
            {
                Entry entry = new Entry();
                entry._date = parts[0];
                entry._promptText = parts[1];
                entry._entryText = parts[2];
                entry._mood = parts[3];
                entry._energyLevel = int.Parse(parts[4]);
                entry._location = parts[5];

                // Parse tags if they exist
                if (!string.IsNullOrEmpty(parts[6]))
                {
                    entry._tags = new List<string>(parts[6].Split(';'));
                }
                else
                {
                    entry._tags = new List<string>();
                }

                entry._wordCount = int.Parse(parts[7]);

                return entry;
            }
        }
        catch
        {
            // If CSV parsing fails, try pipe format as fallback
            return FromLegacyFormat(fileLine);
        }

        return null;
    }

    // Fallback method for legacy pipe format
    private static Entry FromLegacyFormat(string fileLine)
    {
        string[] parts = fileLine.Split('|');

        if (parts.Length >= 3)
        {
            Entry entry = new Entry();
            entry._date = parts[0];
            entry._promptText = parts[1];
            entry._entryText = parts[2];

            // Set defaults for older entries
            entry._mood = parts.Length > 3 ? parts[3] : "Unknown";
            entry._energyLevel = parts.Length > 4 ? int.Parse(parts[4]) : 5;
            entry._location = parts.Length > 5 ? parts[5] : "Unknown";

            if (parts.Length > 6 && !string.IsNullOrEmpty(parts[6]))
            {
                entry._tags = new List<string>(parts[6].Split(','));
            }

            entry._wordCount = entry.CountWords(entry._entryText);

            return entry;
        }

        return null;
    }

    // Helper method to parse CSV lines with quoted fields
    private static List<string> ParseCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string currentField = "";

        for (int i = 0; i < line.Length; i++)
        {
            char currentChar = line[i];

            if (currentChar == '"')
            {
                // Check if this is an escaped quote
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField += '"';
                    i++; // Skip next quote
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (currentChar == ',' && !inQuotes)
            {
                result.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += currentChar;
            }
        }

        // Add the last field
        result.Add(currentField);

        return result;
    }

    // JSON format for modern serialization
    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    // Add a tag to the entry
    public void AddTag(string tag)
    {
        if (!_tags.Contains(tag))
        {
            _tags.Add(tag);
        }
    }

    // Remove a tag from the entry
    public bool RemoveTag(string tag)
    {
        return _tags.Remove(tag);
    }

    // Check if entry has a specific tag
    public bool HasTag(string tag)
    {
        return _tags.Contains(tag, StringComparer.OrdinalIgnoreCase);
    }

    // Calculate reading time (average 200 words per minute)
    public double GetReadingTimeMinutes()
    {
        return Math.Round(_wordCount / 200.0, 1);
    }

    // Get sentiment analysis (simplified based on keywords)
    public string GetSentiment()
    {
        string text = (_promptText + " " + _entryText).ToLower();

        string[] positiveWords = { "happy", "good", "great", "excellent", "love", "joy", "grateful", "thankful", "blessed", "excited" };
        string[] negativeWords = { "sad", "bad", "angry", "upset", "hate", "worried", "anxious", "stressed", "tired", "exhausted" };

        int positiveCount = CountWordsInList(text, positiveWords);
        int negativeCount = CountWordsInList(text, negativeWords);

        if (positiveCount > negativeCount * 2) return "Very Positive";
        if (positiveCount > negativeCount) return "Positive";
        if (negativeCount > positiveCount * 2) return "Very Negative";
        if (negativeCount > positiveCount) return "Negative";
        return "Neutral";
    }

    // Helper method for word counting
    private int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        // Split by whitespace and filter out empty entries
        return text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    // Helper method for counting specific words
    private int CountWordsInList(string text, string[] words)
    {
        int count = 0;
        foreach (string word in words)
        {
            if (text.Contains(word))
                count++;
        }
        return count;
    }

    // Get visual indicator for energy level
    private string GetEnergyIndicator()
    {
        string indicator = "";
        for (int i = 0; i < 10; i++)
        {
            indicator += i < _energyLevel ? "■" : "□";
        }
        return $"{indicator} ({_energyLevel}/10)";
    }

    // Get emoji based on mood
    private string GetMoodEmoji()
    {
        return _mood.ToLower() switch
        {
            "happy" => "😊",
            "excited" => "🤩",
            "grateful" => "🙏",
            "content" => "😌",
            "neutral" => "😐",
            "tired" => "😴",
            "sad" => "😔",
            "angry" => "😠",
            "anxious" => "😰",
            "stressed" => "😫",
            _ => "📝"
        };
    }

    // Get color based on mood for console display
    private ConsoleColor GetMoodColor()
    {
        return _mood.ToLower() switch
        {
            "happy" or "excited" or "grateful" => ConsoleColor.Green,
            "content" or "neutral" => ConsoleColor.Cyan,
            "tired" or "sad" => ConsoleColor.Blue,
            "angry" or "anxious" or "stressed" => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    // Method to update word count
    public void UpdateWordCount()
    {
        _wordCount = CountWords(_entryText);
    }

    // Create a summary of the entry
    public string GetSummary(int maxLength = 100)
    {
        if (_entryText.Length <= maxLength)
            return _entryText;

        return _entryText.Substring(0, maxLength - 3) + "...";
    }

    // Check if entry contains specific keywords
    public bool ContainsKeywords(string[] keywords)
    {
        string searchText = (_promptText + " " + _entryText).ToLower();
        foreach (string keyword in keywords)
        {
            if (searchText.Contains(keyword.ToLower()))
                return true;
        }
        return false;
    }

    // Get entry age in days
    public int GetAgeInDays()
    {
        if (DateTime.TryParse(_date, out DateTime entryDate))
        {
            return (DateTime.Now - entryDate).Days;
        }
        return -1;
    }

    // Format entry for different display modes
    public string ToString(string format)
    {
        return format.ToLower() switch
        {
            "csv" => ToFileString(),
            "json" => ToJson(),
            "short" => $"{_date}: {GetSummary(50)}",
            "detailed" => $"Date: {_date}\nMood: {_mood}\nPrompt: {_promptText}\nEntry: {_entryText}",
            _ => $"Entry from {_date}"
        };
    }
}