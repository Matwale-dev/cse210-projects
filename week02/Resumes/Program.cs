using System;
using System.Collections.Generic;

public class Job
{
    // Member variables
    public string _jobTitle;
    public string _company;
    public int _startYear;
    public int _endYear;

    // Method to display job details
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}

public class Resume
{
    // Member variables
    public string _name;
    public List<Job> _jobs = new List<Job>();

    // Method to display resume details
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        foreach (Job job in _jobs)
        {
            job.Display();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create first job
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer";
        job1._company = "Microsoft";
        job1._startYear = 2019;
        job1._endYear = 2022;

        // Create second job
        Job job2 = new Job();
        job2._jobTitle = "Manager";
        job2._company = "Apple";
        job2._startYear = 2022;
        job2._endYear = 2023;

        // Create a resume and add jobs
        Resume myResume = new Resume();
        myResume._name = "Thomas Matwale";  // Replace with your name
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        // Display the full resume
        myResume.Display();
    }
}
