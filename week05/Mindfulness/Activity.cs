using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    abstract class Activity
    {
        protected string _name;
        protected string _description;
        protected int _duration;

        protected static List<string> _spinnerFrames = new List<string>
        {
            "|", "/", "-", "\\", "|", "/", "-", "\\"
        };

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public void Run()
        {
            DisplayStartingMessage();
            SetDuration();
            Console.Clear();
            Console.WriteLine("Get ready to begin...");
            ShowSpinner(3);
            Console.WriteLine();

            PerformActivity();

            DisplayEndingMessage();
        }

        protected virtual void PerformActivity()
        {
            // To be overridden by derived classes
        }

        protected void DisplayStartingMessage()
        {
            Console.Clear();
            Console.WriteLine($"=== {_name} ===\n");
            Console.WriteLine(_description);
            Console.WriteLine();
        }

        protected void SetDuration()
        {
            Console.Write("How many seconds would you like this activity to last? ");
            _duration = int.Parse(Console.ReadLine());
        }

        protected void DisplayEndingMessage()
        {
            Console.WriteLine("\nGood job! You've completed the activity.");
            ShowSpinner(2);
            Console.WriteLine($"\nYou have completed {_duration} seconds of the {_name}.");
            ShowSpinner(3);
        }

        protected void ShowSpinner(int seconds)
        {
            DateTime startTime = DateTime.Now;
            int frameIndex = 0;

            while ((DateTime.Now - startTime).TotalSeconds < seconds)
            {
                Console.Write(_spinnerFrames[frameIndex % _spinnerFrames.Count]);
                Thread.Sleep(100);
                Console.Write("\b");
                frameIndex++;
            }
        }

        protected void ShowCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b");
            }
        }

        protected void ShowGrowingText(string text, int durationMs)
        {
            // Creative animation: text that grows in size visually
            int startDelay = 50;
            int endDelay = durationMs;
            int step = (endDelay - startDelay) / text.Length;

            for (int i = 1; i <= text.Length; i++)
            {
                Console.Write(text.Substring(0, i));
                Thread.Sleep(startDelay + (i * step / 2));
                Console.SetCursorPosition(Console.CursorLeft - i, Console.CursorTop);
            }
            Console.Write(text);
        }
    }
}