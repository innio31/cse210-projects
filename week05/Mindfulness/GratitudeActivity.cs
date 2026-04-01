using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    class GratitudeActivity : Activity
    {
        private List<string> _categories;

        public GratitudeActivity() : base(
            "Gratitude Activity",
            "This activity will help you cultivate gratitude by focusing on three specific things " +
            "you're thankful for each day. Research shows that practicing gratitude improves happiness and well-being."
        )
        {
            _categories = new List<string>
            {
                "A person who made your life better",
                "A simple pleasure you enjoyed today",
                "A challenge that helped you grow",
                "A skill or talent you possess",
                "A place that brings you peace",
                "A memory that makes you smile",
                "Something in nature you appreciate",
                "An act of kindness you witnessed or received",
                "Something you learned recently",
                "A comfort in your life (food, home, etc.)"
            };
        }

        protected override void PerformActivity()
        {
            Console.WriteLine("\nLet's practice gratitude together.");
            Console.WriteLine("I'll guide you through reflecting on three things you're grateful for.\n");

            List<string> gratitudes = new List<string>();
            Random rand = new Random();

            for (int i = 1; i <= 3; i++)
            {
                string category = _categories[rand.Next(_categories.Count)];
                Console.WriteLine($"\n{i}. Think about: {category}");
                Console.Write("   What are you grateful for? ");

                string item = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(item))
                {
                    gratitudes.Add(item);
                }

                if (i < 3)
                {
                    Console.WriteLine("\n   Take a moment to really feel this gratitude...");
                    ShowSpinner(3);
                }
            }

            Console.WriteLine("\n\n=== Your Gratitude List ===");
            for (int i = 0; i < gratitudes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gratitudes[i]}");
            }

            Console.WriteLine("\nTake a deep breath and appreciate these blessings in your life.");
            ShowSpinner(3);

            // Add a little inspirational message
            Console.WriteLine("\n✨ Remember: Gratitude turns what we have into enough. ✨");
        }
    }
}