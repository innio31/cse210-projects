using System;
using System.Collections.Generic;

namespace Shapes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test individual classes
            Console.WriteLine("Testing individual shapes:");

            Square testSquare = new Square("Red", 5);
            Console.WriteLine($"Square - Color: {testSquare.GetColor()}, Area: {testSquare.GetArea():F2}");

            Rectangle testRect = new Rectangle("Blue", 4, 6);
            Console.WriteLine($"Rectangle - Color: {testRect.GetColor()}, Area: {testRect.GetArea():F2}");

            Circle testCircle = new Circle("Green", 3);
            Console.WriteLine($"Circle - Color: {testCircle.GetColor()}, Area: {testCircle.GetArea():F2}");

            Console.WriteLine("\n--- Polymorphism Demo ---");

            // Create a list of shapes
            List<Shape> shapes = new List<Shape>();

            // Add different shapes to the list
            shapes.Add(new Square("Red", 5));
            shapes.Add(new Rectangle("Blue", 4, 6));
            shapes.Add(new Circle("Green", 3));
            shapes.Add(new Square("Yellow", 2.5));
            shapes.Add(new Rectangle("Purple", 7, 3));
            shapes.Add(new Circle("Orange", 4.2));

            // Iterate through the list and display their areas
            foreach (Shape shape in shapes)
            {
                Console.WriteLine($"Shape: {shape.GetType().Name}, Color: {shape.GetColor()}, Area: {shape.GetArea():F2}");
            }
        }
    }
}