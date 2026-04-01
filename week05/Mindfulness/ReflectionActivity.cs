using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    class ReflectionActivity : Activity
    {
        private List<string> _prompts;
        private List<string> _questions;
        private List<string> _usedPrompts;
        private List<string> _usedQuestions;

        public ReflectionActivity() : base(
            "Reflection Activity",
            "This activity will help you reflect on times in your life when you have shown strength " +
            "and resilience. This will help you recognize the power you have and how you can use it " +
            "in other aspects of your life."
        )
        {
            _prompts = new List<string>
            {
                "Think of a time when you stood up for someone else.",
                "Think of a time when you did something really difficult.",
                "Think of a time when you helped someone in need.",
                "Think of a time when you did something truly selfless.",
                "Think of a time when you overcame a significant challenge.",
                "Think of a time when you showed courage in a difficult situation."
            };

            _questions = new List<string>
            {
                "Why was this experience meaningful to you?",
                "Have you ever done anything like this before?",
                "How did you get started?",
                "How did you feel when it was complete?",
                "What made this time different than other times when you were not as successful?",
                "What is your favorite thing about this experience?",
                "What could you learn from this experience that applies to other situations?",
                "What did you learn about yourself through this experience?",
                "How can you keep this experience in mind in the future?",
                "What strengths did you discover about yourself?",
                "How did this experience shape who you are today?"
            };

            _usedPrompts = new List<string>();
            _usedQuestions = new List<string>();
        }

        protected override void PerformActivity()
        {
            // Reset used lists for new session
            _usedPrompts.Clear();
            _usedQuestions.Clear();

            string prompt = GetRandomPrompt();
            Console.WriteLine("\nConsider this prompt:");
            Console.WriteLine($"\n--- {prompt} ---\n");
            Console.WriteLine("When you have something in mind, press Enter to continue.");
            Console.ReadLine();

            Console.WriteLine("Now, reflect on these questions:");

            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            int questionCount = 0;

            while (DateTime.Now < endTime)
            {
                string question = GetRandomQuestion();
                Console.Write($"\n> {question} ");
                ShowSpinner(5);
                Console.WriteLine();
                questionCount++;

                // Ensure we don't exceed duration
                if ((DateTime.Now.AddSeconds(6)) >= endTime)
                    break;
            }

            Console.WriteLine($"\nYou reflected on {questionCount} questions during this session.");
        }

        private string GetRandomPrompt()
        {
            // Reset used prompts if all have been used
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

        private string GetRandomQuestion()
        {
            // Reset used questions if all have been used
            if (_usedQuestions.Count >= _questions.Count)
            {
                _usedQuestions.Clear();
            }

            Random rand = new Random();
            string question;

            do
            {
                question = _questions[rand.Next(_questions.Count)];
            } while (_usedQuestions.Contains(question));

            _usedQuestions.Add(question);
            return question;
        }
    }
}