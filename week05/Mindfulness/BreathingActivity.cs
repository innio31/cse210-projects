using System;
using System.Threading;

namespace MindfulnessProgram
{
    class BreathingActivity : Activity
    {
        public BreathingActivity() : base(
            "Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. " +
            "Clear your mind and focus on your breathing."
        )
        {
        }

        protected override void PerformActivity()
        {
            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            bool isInhale = true;

            while (DateTime.Now < endTime)
            {
                if (isInhale)
                {
                    Console.Write("\nBreathe in... ");
                    ShowGrowingText("▼", 4000);
                    ShowCountdown(4);
                }
                else
                {
                    Console.Write("\nBreathe out... ");
                    ShowGrowingText("▲", 4000);
                    ShowCountdown(4);
                }

                isInhale = !isInhale;
            }
        }
    }
}