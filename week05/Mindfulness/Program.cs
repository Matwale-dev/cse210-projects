using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;

namespace MindfulnessApp
{
    // Base Activity class
    public abstract class Activity
    {
        protected string _name;
        protected string _description;
        protected int _duration;
        protected static int _totalActivitiesCompleted = 0;
        protected static List<string> _activityLog = new List<string>();

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine($"Starting {_name} Activity");
            Console.WriteLine(_description);
            Console.WriteLine();
            
            SetDuration();
            Console.WriteLine("Prepare to begin...");
            ShowSpinner(3);
            
            ExecuteActivity();
            
            End();
            LogActivity();
        }

        protected virtual void SetDuration()
        {
            Console.Write("How long, in seconds, would you like for your session? ");
            while (!int.TryParse(Console.ReadLine(), out _duration) || _duration <= 0)
            {
                Console.Write("Please enter a positive number: ");
            }
        }

        protected abstract void ExecuteActivity();

        protected virtual void End()
        {
            Console.WriteLine();
            Console.WriteLine("Good job! You have completed the activity.");
            ShowSpinner(2);
            Console.WriteLine($"You have completed the {_name} activity for {_duration} seconds.");
            ShowSpinner(3);
        }

        protected void ShowSpinner(int seconds)
        {
            List<string> animation = new List<string> { "|", "/", "-", "\\" };
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(seconds);

            int animationIndex = 0;
            while (DateTime.Now < endTime)
            {
                Console.Write(animation[animationIndex]);
                Thread.Sleep(250);
                Console.Write("\b \b");
                animationIndex = (animationIndex + 1) % animation.Count;
            }
            Console.WriteLine();
        }

        protected void ShowCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b");
            }
            Console.WriteLine();
        }

        protected void LogActivity()
        {
            _totalActivitiesCompleted++;
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Completed {_name} for {_duration} seconds";
            _activityLog.Add(logEntry);
        }

        public static void DisplayStatistics()
        {
            Console.WriteLine("\n=== Activity Statistics ===");
            Console.WriteLine($"Total activities completed: {_totalActivitiesCompleted}");
            Console.WriteLine("\nActivity Log:");
            foreach (var log in _activityLog)
            {
                Console.WriteLine($"  {log}");
            }
        }

        public static void SaveLogToFile()
        {
            string filename = $"mindfulness_log_{DateTime.Now:yyyyMMdd}.txt";
            try
            {
                File.WriteAllLines(filename, _activityLog);
                Console.WriteLine($"Log saved to {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving log: {ex.Message}");
            }
        }
    }

    // Breathing Activity class
    public class BreathingActivity : Activity
    {
        public BreathingActivity() : base(
            "Breathing", 
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing."
        ) { }

        protected override void ExecuteActivity()
        {
            Console.WriteLine("Starting breathing exercise...");
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);

            bool breatheIn = true;
            while (DateTime.Now < endTime)
            {
                if (breatheIn)
                {
                    Console.WriteLine("Breathe in...");
                    ShowAnimatedBreath(4, true);
                }
                else
                {
                    Console.WriteLine("Breathe out...");
                    ShowAnimatedBreath(6, false);
                }
                breatheIn = !breatheIn;
                
                if (DateTime.Now >= endTime) break;
            }
        }

        private void ShowAnimatedBreath(int seconds, bool isInhale)
        {
            // Creative enhancement: animated breathing text
            string baseText = isInhale ? "Breathe IN " : "Breathe OUT";
            int maxSize = 10;
            
            if (isInhale)
            {
                for (int i = 1; i <= maxSize; i++)
                {
                    if (DateTime.Now.AddSeconds(seconds) >= DateTime.Now.AddSeconds(_duration - (DateTime.Now - DateTime.Now).TotalSeconds))
                        break;
                        
                    Console.Write($"\r{baseText} {new string('=', i)}");
                    Thread.Sleep(seconds * 1000 / maxSize);
                }
            }
            else
            {
                for (int i = maxSize; i > 0; i--)
                {
                    if (DateTime.Now.AddSeconds(seconds) >= DateTime.Now.AddSeconds(_duration - (DateTime.Now - DateTime.Now).TotalSeconds))
                        break;
                        
                    Console.Write($"\r{baseText} {new string('=', i)}");
                    Thread.Sleep(seconds * 1000 / maxSize);
                }
            }
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }
    }

    // Reflection Activity class
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts;
        private List<string> _questions;
        private List<int> _usedQuestionIndices;

        public ReflectionActivity() : base(
            "Reflection",
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life."
        )
        {
            InitializePrompts();
            InitializeQuestions();
            _usedQuestionIndices = new List<int>();
        }

        private void InitializePrompts()
        {
            _prompts = new List<string>
            {
                "Think of a time when you stood up for someone else.",
                "Think of a time when you did something really difficult.",
                "Think of a time when you helped someone in need.",
                "Think of a time when you did something truly selfless.",
                "Think of a time when you overcame a significant challenge."
            };
        }

        private void InitializeQuestions()
        {
            _questions = new List<string>
            {
                "Why was this experience meaningful to you?",
                "Have you ever done anything like this before?",
                "How did you get started?",
                "How did you feel when it was complete?",
                "What made this time different than other times when you were not as successful?",
                "What is your favorite thing about this experience?",
                "What could you learn from this experience that applies to other situations?",
                "What did you learn about yourself through this experience?",
                "How can you keep this experience in mind in the future?",
                "What strength did you discover in yourself through this experience?"
            };
        }

        protected override void ExecuteActivity()
        {
            string prompt = GetRandomPrompt();
            Console.WriteLine($"Reflect on this: {prompt}");
            Console.WriteLine("When you have something in mind, press enter to continue.");
            Console.ReadLine();

            Console.WriteLine("Now ponder on the following questions:");
            ShowCountdown(5);

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);

            while (DateTime.Now < endTime)
            {
                string question = GetUniqueRandomQuestion();
                if (question == null) break; // All questions used

                Console.WriteLine($"\n{question}");
                ShowSpinner(8);
                
                if (DateTime.Now >= endTime) break;
            }
        }

        private string GetRandomPrompt()
        {
            Random random = new Random();
            return _prompts[random.Next(_prompts.Count)];
        }

        private string GetUniqueRandomQuestion()
        {
            if (_usedQuestionIndices.Count >= _questions.Count)
            {
                // Creative enhancement: reshuffle when all questions used
                _usedQuestionIndices.Clear();
            }

            Random random = new Random();
            int index;
            do
            {
                index = random.Next(_questions.Count);
            } while (_usedQuestionIndices.Contains(index));

            _usedQuestionIndices.Add(index);
            return _questions[index];
        }
    }

    // Listing Activity class
    public class ListingActivity : Activity
    {
        private List<string> _prompts;
        private int _itemCount;

        public ListingActivity() : base(
            "Listing",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."
        )
        {
            InitializePrompts();
        }

        private void InitializePrompts()
        {
            _prompts = new List<string>
            {
                "Who are people that you appreciate?",
                "What are personal strengths of yours?",
                "Who are people that you have helped this week?",
                "When have you felt the Holy Ghost this month?",
                "Who are some of your personal heroes?",
                "What are you grateful for today?",
                "What made you smile recently?",
                "What accomplishments are you proud of?"
            };
        }

        protected override void ExecuteActivity()
        {
            string prompt = GetRandomPrompt();
            Console.WriteLine($"List as many things as you can about: {prompt}");
            Console.WriteLine("You have a few seconds to prepare...");
            ShowCountdown(5);

            Console.WriteLine("Start listing now! (Press enter after each item, type 'done' when finished)");
            
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            _itemCount = 0;

            // Creative enhancement: show remaining time while listing
            while (DateTime.Now < endTime)
            {
                int remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;
                Console.Write($"[{remainingSeconds}s remaining] Item {_itemCount + 1}: ");
                
                var inputTask = Task.Run(() => Console.ReadLine());
                if (inputTask.Wait(TimeSpan.FromSeconds(remainingSeconds)))
                {
                    string item = inputTask.Result;
                    if (item?.ToLower() == "done") break;
                    if (!string.IsNullOrWhiteSpace(item)) _itemCount++;
                }
                else
                {
                    Console.WriteLine("\nTime's up!");
                    break;
                }
            }

            Console.WriteLine($"\nYou listed {_itemCount} items!");
        }

        private string GetRandomPrompt()
        {
            Random random = new Random();
            return _prompts[random.Next(_prompts.Count)];
        }

        protected override void End()
        {
            base.End();
            Console.WriteLine($"You listed {_itemCount} items during this activity!");
            ShowSpinner(2);
        }
    }

    // Creative enhancement: Gratitude Journal Activity
    public class GratitudeActivity : Activity
    {
        public GratitudeActivity() : base(
            "Gratitude Journal",
            "This activity will help you cultivate gratitude by writing about specific things you're thankful for in detail."
        ) { }

        protected override void ExecuteActivity()
        {
            Console.WriteLine("Take this time to write about something you're grateful for.");
            Console.WriteLine("Be specific and detailed about why you're grateful for it.");
            Console.WriteLine();

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            List<string> gratitudeEntries = new List<string>();

            while (DateTime.Now < endTime)
            {
                int remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;
                Console.Write($"[{remainingSeconds}s remaining] What are you grateful for? ");
                
                var inputTask = Task.Run(() => Console.ReadLine());
                if (inputTask.Wait(TimeSpan.FromSeconds(Math.Min(remainingSeconds, 30))))
                {
                    string entry = inputTask.Result;
                    if (!string.IsNullOrWhiteSpace(entry))
                    {
                        gratitudeEntries.Add(entry);
                        Console.WriteLine("Thank you for sharing. Reflect on this for a moment...");
                        ShowSpinner(3);
                    }
                }

                if (DateTime.Now >= endTime) break;
                
                if (gratitudeEntries.Count >= 3) // Limit to focus on quality
                {
                    Console.WriteLine("You've written several grateful thoughts. Take a moment to reflect...");
                    ShowSpinner(5);
                    break;
                }
            }

            Console.WriteLine($"\nYou expressed gratitude for {gratitudeEntries.Count} things:");
            foreach (var entry in gratitudeEntries)
            {
                Console.WriteLine($"  - {entry}");
            }
        }
    }

    // Main Program class
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Mindfulness Program!");
            
            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                Activity activity = null;

                switch (choice)
                {
                    case "1":
                        activity = new BreathingActivity();
                        break;
                    case "2":
                        activity = new ReflectionActivity();
                        break;
                    case "3":
                        activity = new ListingActivity();
                        break;
                    case "4":
                        activity = new GratitudeActivity();
                        break;
                    case "5":
                        Activity.DisplayStatistics();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    case "6":
                        Activity.SaveLogToFile();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    case "7":
                        Console.WriteLine("Thank you for using the Mindfulness Program. Be well!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        ShowSpinner(2);
                        continue;
                }

                activity?.Start();
            }
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Mindfulness Program ===");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Gratitude Journal Activity");
            Console.WriteLine("5. View Statistics");
            Console.WriteLine("6. Save Activity Log");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an activity: ");
        }

        static void ShowSpinner(int seconds)
        {
            List<string> animation = new List<string> { "|", "/", "-", "\\" };
            DateTime endTime = DateTime.Now.AddSeconds(seconds);
            int animationIndex = 0;

            while (DateTime.Now < endTime)
            {
                Console.Write(animation[animationIndex]);
                Thread.Sleep(250);
                Console.Write("\b \b");
                animationIndex = (animationIndex + 1) % animation.Count;
            }
            Console.WriteLine();
        }
    }
}