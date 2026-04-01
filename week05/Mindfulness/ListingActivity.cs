using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    class ListingActivity : Activity
    {
        private List<string> _prompts;
        private List<string> _usedPrompts;

        public ListingActivity() : base(
            "Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list " +
            "as many things as you can in a certain area."
        )
        {
            _prompts = new List<string>
            {
                "Who are people that you appreciate?",
                "What are personal strengths of yours?",
                "Who are people that you have helped this week?",
                "When have you felt the Holy Ghost this month?",
                "Who are some of your personal heroes?",
                "What are things you're grateful for today?",
                "What accomplishments are you proud of?",
                "What are your favorite moments from this week?"
            };

            _usedPrompts = new List<string>();
        }

        protected override void PerformActivity()
        {
            _usedPrompts.Clear();
            string prompt = GetRandomPrompt();

            Console.WriteLine($"\nList as many things as you can based on this prompt:");
            Console.WriteLine($"\n--- {prompt} ---\n");
            Console.WriteLine("You have a few seconds to think...");
            ShowCountdown(5);
            Console.WriteLine("\nStart listing items (press Enter after each item, type 'done' to finish early):\n");

            List<string> items = new List<string>();
            DateTime endTime = DateTime.Now.AddSeconds(_duration);

            while (DateTime.Now < endTime)
            {
                Console.Write("> ");
                string item = Console.ReadLine();

                if (item.ToLower() == "done")
                    break;

                if (!string.IsNullOrWhiteSpace(item))
                {
                    items.Add(item);
                }

                // Check if we're out of time
                if (DateTime.Now >= endTime)
                    break;
            }

            Console.WriteLine($"\nYou listed {items.Count} items!");

            if (items.Count > 0)
            {
                Console.WriteLine("\nHere's what you listed:");
                for (int i = 0; i < Math.Min(items.Count, 10); i++)
                {
                    Console.WriteLine($"  {i + 1}. {items[i]}");
                }
                if (items.Count > 10)
                {
                    Console.WriteLine($"  ... and {items.Count - 10} more");
                }
            }
        }

        private string GetRandomPrompt()
        {
            if (_usedPrompts.Count >= _prompts.Count)
            {
                _usedPrompts.Clear();
            }

            Random rand = new Random();
            string prompt;

            do
            {
                prompt = _prompts[rand.Next(_prompts.Count)];
            } while (_usedPrompts.Contains(prompt));

            _usedPrompts.Add(prompt);
            return prompt;
        }
    }
}