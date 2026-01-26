using System;
using System.Collections.Generic;

public class PromptGenerator
{
    public List<string> _prompts = new List<string>();

    // Constructor to initialize prompts
    public PromptGenerator()
    {
        // Core prompts
        _prompts.Add("Who was the most interesting person I interacted with today?");
        _prompts.Add("What was the best part of my day?");
        _prompts.Add("How did I see the hand of the Lord in my life today?");
        _prompts.Add("What was the strongest emotion I felt today?");
        _prompts.Add("If I had one thing I could do over today, what would it be?");

        // Personal growth prompts
        _prompts.Add("What did I learn today that surprised me?");
        _prompts.Add("What made me laugh today?");
        _prompts.Add("What am I grateful for today?");
        _prompts.Add("What challenge did I overcome today?");
        _prompts.Add("What would make tomorrow even better?");

        // Reflection prompts
        _prompts.Add("What's something beautiful I noticed today?");
        _prompts.Add("How did I help someone today?");
        _prompts.Add("What's a decision I made today that I'm proud of?");
        _prompts.Add("What's something I'm looking forward to?");
        _prompts.Add("What lesson did today teach me?");

        // Creative prompts
        _prompts.Add("If today was a chapter in a book, what would the title be?");
        _prompts.Add("Describe today using only three words.");
        _prompts.Add("What color best represents my day and why?");
        _prompts.Add("What song reminds me of today?");
        _prompts.Add("If I could give today a theme song, what would it be?");

        // Evening reflection prompts
        _prompts.Add("What's one thing I could have done better today?");
        _prompts.Add("How did I take care of myself today?");
        _prompts.Add("What's something I want to remember about today?");
        _prompts.Add("How did I grow as a person today?");
        _prompts.Add("What's my intention for tomorrow?");
    }

    // Method to get a random prompt
    public string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(_prompts.Count);
        return _prompts[index];
    }

    // Get a prompt by category
    public string GetPromptByCategory(string category)
    {
        var filteredPrompts = category.ToLower() switch
        {
            "gratitude" => _prompts.Where(p => p.Contains("grateful", StringComparison.OrdinalIgnoreCase)
                                             || p.Contains("thankful", StringComparison.OrdinalIgnoreCase)),
            "reflection" => _prompts.Where(p => p.Contains("learn", StringComparison.OrdinalIgnoreCase)
                                              || p.Contains("grow", StringComparison.OrdinalIgnoreCase)),
            "people" => _prompts.Where(p => p.Contains("person", StringComparison.OrdinalIgnoreCase)
                                          || p.Contains("interact", StringComparison.OrdinalIgnoreCase)),
            "emotion" => _prompts.Where(p => p.Contains("emotion", StringComparison.OrdinalIgnoreCase)
                                           || p.Contains("feel", StringComparison.OrdinalIgnoreCase)),
            "creative" => _prompts.Where(p => p.Contains("color", StringComparison.OrdinalIgnoreCase)
                                            || p.Contains("song", StringComparison.OrdinalIgnoreCase)
                                            || p.Contains("chapter", StringComparison.OrdinalIgnoreCase)),
            _ => _prompts
        };

        var promptList = filteredPrompts.ToList();
        if (promptList.Count == 0)
            return GetRandomPrompt();

        Random random = new Random();
        return promptList[random.Next(promptList.Count)];
    }
}