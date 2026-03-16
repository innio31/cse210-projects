using System;

public class Fraction
{
    // Private attributes (encapsulation)
    private int _top;
    private int _bottom;

    // Constructors
    public Fraction()
    {
        // Default to 1/1
        _top = 1;
        _bottom = 1;
    }

    public Fraction(int top)
    {
        // Single parameter constructor - denominator defaults to 1
        _top = top;
        _bottom = 1;
    }

    public Fraction(int top, int bottom)
    {
        // Two parameter constructor
        _top = top;
        _bottom = bottom;
    }

    // Getters and Setters
    public int GetTop()
    {
        return _top;
    }

    public void SetTop(int top)
    {
        _top = top;
    }

    public int GetBottom()
    {
        return _bottom;
    }

    public void SetBottom(int bottom)
    {
        _bottom = bottom;
    }

    // Methods to return representations
    public string GetFractionString()
    {
        return $"{_top}/{_bottom}";
    }

    public double GetDecimalValue()
    {
        return (double)_top / _bottom;
    }
}