namespace EternalQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * CREATIVITY & EXCEEDING REQUIREMENTS:
             * 
             * 1. LEVELING SYSTEM - Players level up every 1000 points earned.
             *    This provides long-term motivation and a sense of progression.
             * 
             * 2. TOTAL POINTS TRACKING - The program tracks lifetime points earned,
             *    not just current score, enabling the leveling system.
             * 
             * 3. VISUAL FEEDBACK - Added emoji indicators (🎯, 🎉, ✨, ⚠️) to make
             *    the interface more engaging and rewarding.
             * 
             * 4. STATISTICS DISPLAY - Shows total goals, completed goals count,
             *    and progress to next level on the main display.
             * 
             * 5. BONUS ANNOUNCEMENT - When a checklist goal is completed, a special
             *    celebration message appears showing the bonus points earned.
             * 
             * 6. ERROR HANDLING - Checks for empty goal lists and already-completed
             *    goals to prevent invalid operations.
             * 
             * 7. PROGRESS VISUALIZATION - Checklist goals show "Completed X/Y times"
             *    giving clear progress feedback.
             */

            GoalManager manager = new GoalManager();
            manager.Start();
        }
    }
}