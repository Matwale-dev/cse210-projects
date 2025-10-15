using System;
using System.Collections.Generic;

// Base Activity class
public abstract class Activity
{
    private DateTime _date;
    private int _lengthInMinutes;

    protected Activity(DateTime date, int lengthInMinutes)
    {
        _date = date;
        _lengthInMinutes = lengthInMinutes;
    }

    // Properties
    public DateTime Date => _date;
    public int LengthInMinutes => _lengthInMinutes;

    // Abstract methods to be implemented by derived classes
    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    // Virtual method that can be overridden if needed
    public virtual string GetSummary()
    {
        return $"{_date:dd MMM yyyy} {GetType().Name} ({_lengthInMinutes} min) - " +
               $"Distance: {GetDistance():F1} miles, Speed: {GetSpeed():F1} mph, Pace: {GetPace():F1} min per mile";
    }
}

// Running class
public class Running : Activity
{
    private double _distance;

    public Running(DateTime date, int lengthInMinutes, double distance) 
        : base(date, lengthInMinutes)
    {
        _distance = distance;
    }

    public override double GetDistance()
    {
        return _distance;
    }

    public override double GetSpeed()
    {
        return (_distance / LengthInMinutes) * 60;
    }

    public override double GetPace()
    {
        return LengthInMinutes / _distance;
    }
}

// Cycling class
public class Cycling : Activity
{
    private double _speed;

    public Cycling(DateTime date, int lengthInMinutes, double speed) 
        : base(date, lengthInMinutes)
    {
        _speed = speed;
    }

    public override double GetDistance()
    {
        return (_speed * LengthInMinutes) / 60;
    }

    public override double GetSpeed()
    {
        return _speed;
    }

    public override double GetPace()
    {
        return 60 / _speed;
    }
}

// Swimming class
public class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int lengthInMinutes, int laps) 
        : base(date, lengthInMinutes)
    {
        _laps = laps;
    }

    public override double GetDistance()
    {
        // Convert laps to miles (50 meters per lap, converted to miles)
        return (_laps * 50.0 / 1000) * 0.62;
    }

    public override double GetSpeed()
    {
        return (GetDistance() / LengthInMinutes) * 60;
    }

    public override double GetPace()
    {
        return LengthInMinutes / GetDistance();
    }

    public override string GetSummary()
    {
        return $"{Date:dd MMM yyyy} {GetType().Name} ({LengthInMinutes} min) - " +
               $"Distance: {GetDistance():F1} miles, Speed: {GetSpeed():F1} mph, " +
               $"Pace: {GetPace():F1} min per mile, Laps: {_laps}";
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        // Create activities
        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2022, 11, 3), 30, 3.0),
            new Cycling(new DateTime(2022, 11, 4), 45, 15.0),
            new Swimming(new DateTime(2022, 11, 5), 60, 40)
        };

        // Display summaries
        Console.WriteLine("Exercise Tracking Summary");
        Console.WriteLine("=========================");
        
        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
            Console.WriteLine();
        }
    }
}