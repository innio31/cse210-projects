using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Testing Fraction Class ===\n");

        // Test constructor with no parameters (1/1)
        Console.WriteLine("Testing constructor with no parameters:");
        Fraction f1 = new Fraction();
        Console.WriteLine($"Fraction: {f1.GetFractionString()}");
        Console.WriteLine($"Decimal: {f1.GetDecimalValue()}");
        Console.WriteLine();

        // Test constructor with one parameter (5/1)
        Console.WriteLine("Testing constructor with one parameter (5):");
        Fraction f2 = new Fraction(5);
        Console.WriteLine($"Fraction: {f2.GetFractionString()}");
        Console.WriteLine($"Decimal: {f2.GetDecimalValue()}");
        Console.WriteLine();

        // Test constructor with two parameters (3/4)
        Console.WriteLine("Testing constructor with two parameters (3, 4):");
        Fraction f3 = new Fraction(3, 4);
        Console.WriteLine($"Fraction: {f3.GetFractionString()}");
        Console.WriteLine($"Decimal: {f3.GetDecimalValue()}");
        Console.WriteLine();

        // Test constructor with two parameters (1/3)
        Console.WriteLine("Testing constructor with two parameters (1, 3):");
        Fraction f4 = new Fraction(1, 3);
        Console.WriteLine($"Fraction: {f4.GetFractionString()}");
        Console.WriteLine($"Decimal: {f4.GetDecimalValue()}");
        Console.WriteLine();

        // Test getters and setters
        Console.WriteLine("=== Testing Getters and Setters ===");
        Console.WriteLine("Creating fraction 2/5, then changing to 7/8:");

        Fraction f5 = new Fraction(2, 5);
        Console.WriteLine($"Initial: {f5.GetFractionString()}");

        // Use setters to change values
        f5.SetTop(7);
        f5.SetBottom(8);

        // Use getters to retrieve and display
        Console.WriteLine($"After changes: {f5.GetTop()}/{f5.GetBottom()}");
        Console.WriteLine($"Fraction string: {f5.GetFractionString()}");
        Console.WriteLine($"Decimal value: {f5.GetDecimalValue()}");
        Console.WriteLine();

        // Additional test cases
        Console.WriteLine("=== Additional Test Cases ===");

        Fraction f6 = new Fraction(6, 1);
        Console.WriteLine($"6/1 = {f6.GetDecimalValue()}");

        Fraction f7 = new Fraction(2, 3);
        Console.WriteLine($"2/3 = {f7.GetDecimalValue()}");

        Fraction f8 = new Fraction(4, 8);
        Console.WriteLine($"4/8 = {f8.GetDecimalValue()} (should be 0.5)");
    }
}