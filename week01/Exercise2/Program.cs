using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Emmanuel's grading system. Kindly enter your grade percentage:");
        string input = Console.ReadLine();
        int gradePercentage = int.Parse(input);
        string letterGrade;

        if (gradePercentage >= 90)
        {
            letterGrade = "A";
        }
        else if (gradePercentage >= 80)
        {
            letterGrade = "B";
        }
        else if (gradePercentage >= 70)
        {
            letterGrade = "C";
        }
        else if (gradePercentage >= 60)
        {
            letterGrade = "D";
        }
        else
        {
            letterGrade = "F";
        }

        // Determine the sign (+ or -)
        string sign = "";
        int lastDigit = gradePercentage % 10;

        // F does not get a sign
        if (letterGrade != "F")
        {
            if (letterGrade == "A")
            {
                if (lastDigit < 3 && gradePercentage < 100) // A- condition
                {
                    sign = "-";
                }
                // No A+ exists
            }
            else // For B, C, D
            {
                if (lastDigit >= 7)
                {
                    sign = "+";
                }
                else if (lastDigit < 3)
                {
                    sign = "-";
                }
            }
        }

        Console.WriteLine($"Your letter grade is: {letterGrade}{sign}");

        if (gradePercentage >= 70)
        {
            Console.WriteLine("Congratulations! You have passed the course.");
        }
        else
        {
            Console.WriteLine("Unfortunately, you have not passed the course. Better luck next time!");
        }
    }
}