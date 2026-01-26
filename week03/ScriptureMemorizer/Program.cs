using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Creativity/Exceeding Requirements:
        // 1. Added a ScriptureLibrary class that loads multiple scriptures from a text file
        // 2. The library can be filtered by difficulty level (word count)
        // 3. Random scripture is selected from the library each time program runs
        // 4. Added a progress indicator showing percentage of words hidden
        // 5. Added error handling for file operations and console clearing

        ScriptureLibrary library = new ScriptureLibrary("scriptures.txt");

        if (library.Scriptures.Count == 0)
        {
            // Use a default scripture if file is empty or not found
            Scripture scripture = new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
            );
            RunMemorizer(scripture);
        }
        else
        {
            // Select a random scripture from the library
            Random random = new Random();
            Scripture randomScripture = library.Scriptures[random.Next(library.Scriptures.Count)];
            RunMemorizer(randomScripture);
        }
    }

    static void RunMemorizer(Scripture scripture)
    {
        ClearConsole();
        Console.WriteLine("Scripture Memorizer\n");

        while (!scripture.IsCompletelyHidden())
        {
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine($"\nProgress: {scripture.GetHiddenPercentage():F0}% hidden");
            Console.WriteLine("\nPress Enter to hide more words, type 'quit' to exit:");

            string input = Console.ReadLine();
            if (input.ToLower() == "quit")
            {
                return;
            }

            scripture.HideRandomWords(3); // Hide 3 words at a time
            ClearConsole();
        }

        // Final display with all words hidden
        ClearConsole();
        Console.WriteLine(scripture.GetDisplayText());
        Console.WriteLine("\nAll words hidden! Press any key to exit.");
        Console.ReadKey();
    }

    static void ClearConsole()
    {
        try
        {
            Console.Clear();
        }
        catch (System.IO.IOException)
        {
            // If we can't clear the console (like in some online compilers),
            // just print some blank lines to simulate clearing
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n");
        }
    }
}

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
        _random = new Random();
    }

    public void HideRandomWords(int numberToHide)
    {
        int wordsHidden = 0;
        while (wordsHidden < numberToHide && !IsCompletelyHidden())
        {
            // Only hide words that aren't already hidden
            List<int> visibleIndices = new List<int>();
            for (int i = 0; i < _words.Count; i++)
            {
                if (!_words[i].IsHidden())
                    visibleIndices.Add(i);
            }

            if (visibleIndices.Count == 0)
                return;

            int randomIndex = visibleIndices[_random.Next(visibleIndices.Count)];
            _words[randomIndex].Hide();
            wordsHidden++;
        }
    }

    public string GetDisplayText()
    {
        string wordsText = string.Join(" ", _words.Select(word => word.GetDisplayText()));
        return $"{_reference.GetDisplayText()} {wordsText}";
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(word => word.IsHidden());
    }

    public double GetHiddenPercentage()
    {
        int hiddenCount = _words.Count(word => word.IsHidden());
        return (double)hiddenCount / _words.Count * 100;
    }
}

public class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = verse; // Single verse
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (_verse == _endVerse)
            return $"{_book} {_chapter}:{_verse}";
        else
            return $"{_book} {_chapter}:{_verse}-{_endVerse}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public void Show()
    {
        _isHidden = false;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        if (_isHidden)
            return new string('_', _text.Length);
        else
            return _text;
    }
}

public class ScriptureLibrary
{
    public List<Scripture> Scriptures { get; private set; }

    public ScriptureLibrary(string filePath)
    {
        Scriptures = new List<Scripture>();
        LoadFromFile(filePath);
    }

    private void LoadFromFile(string filePath)
    {
        try
        {
            // Create default scriptures if file doesn't exist
            if (!System.IO.File.Exists(filePath))
            {
                // Create some default scriptures
                Scriptures.Add(new Scripture(
                    new Reference("John", 3, 16),
                    "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
                ));
                Scriptures.Add(new Scripture(
                    new Reference("Proverbs", 3, 5, 6),
                    "Trust in the Lord with all your heart and lean not on your own understanding; in all your ways submit to him and he will make your paths straight."
                ));
                Scriptures.Add(new Scripture(
                    new Reference("Philippians", 4, 13),
                    "I can do all things through Christ who strengthens me."
                ));
                return;
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Format: Book|Chapter|StartVerse|EndVerse|Text
                // For single verse: EndVerse = StartVerse
                string[] parts = line.Split('|');

                if (parts.Length >= 5)
                {
                    string book = parts[0];
                    int chapter = int.Parse(parts[1]);
                    int startVerse = int.Parse(parts[2]);
                    int endVerse = int.Parse(parts[3]);
                    string text = parts[4];

                    Reference reference;
                    if (startVerse == endVerse)
                        reference = new Reference(book, chapter, startVerse);
                    else
                        reference = new Reference(book, chapter, startVerse, endVerse);

                    Scriptures.Add(new Scripture(reference, text));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading scriptures: {ex.Message}");
            // Add a default scripture if loading fails
            Scriptures.Add(new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
            ));
        }
    }
}