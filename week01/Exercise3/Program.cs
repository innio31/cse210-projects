using System;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;
        Random random = new Random();

        while (playAgain)
        {
            int magicNumber = random.Next(1, 101);
            int guess = -1;
            int guessCount = 0;

            Console.WriteLine("\n🎮 Welcome to the Number Guessing Game! 🎮");
            Console.WriteLine("I'm thinking of a number between 1 and 100.");

            while (guess != magicNumber)
            {
                Console.Write("What is your guess? ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out guess))
                {
                    Console.WriteLine("Please enter a valid number!");
                    continue;
                }

                guessCount++;

                if (guess < magicNumber)
                {
                    Console.WriteLine("Higher! ⬆️");
                }
                else if (guess > magicNumber)
                {
                    Console.WriteLine("Lower! ⬇️");
                }
                else
                {
                    Console.WriteLine($"🎉 You guessed it in {guessCount} tries! 🎉");
                }
            }

            Console.Write("\nDo you want to play again? (yes/no): ");
            string playAgainInput = Console.ReadLine().ToLower();

            playAgain = (playAgainInput == "yes" || playAgainInput == "y");
        }

        Console.WriteLine("\nThanks for playing! Goodbye! 👋");
    }
}