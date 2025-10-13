using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class Goal
{
    protected string _name;
    protected string _description;
    protected int _points;
    protected bool _isComplete;

    public Goal(string name, string description, int points)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = false;
    }

    public Goal(string name, string description, int points, bool isComplete)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = isComplete;
    }

    public abstract int RecordEvent();
    public abstract string GetProgress();
    public abstract string GetStringRepresentation();

    public string GetName() => _name;
    public string GetDescription() => _description;
    public bool IsComplete() => _isComplete;

    protected string GetCompletionStatus()
    {
        return _isComplete ? "[X]" : "[ ]";
    }
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points) 
        : base(name, description, points) { }

    public SimpleGoal(string name, string description, int points, bool isComplete)
        : base(name, description, points, isComplete) { }

    public override int RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            return _points;
        }
        return 0;
    }

    public override string GetProgress()
    {
        return $"{GetCompletionStatus()} {_name} - {_description}";
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal|{_name}|{_description}|{_points}|{_isComplete}";
    }
}

public class EternalGoal : Goal
{
    private int _timesCompleted;

    public EternalGoal(string name, string description, int points) 
        : base(name, description, points)
    {
        _timesCompleted = 0;
    }

    public EternalGoal(string name, string description, int points, int timesCompleted)
        : base(name, description, points, false)
    {
        _timesCompleted = timesCompleted;
    }

    public override int RecordEvent()
    {
        _timesCompleted++;
        return _points;
    }

    public override string GetProgress()
    {
        return $"{GetCompletionStatus()} {_name} - {_description} (Completed {_timesCompleted} times)";
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal|{_name}|{_description}|{_points}|{_timesCompleted}";
    }
}

public class ChecklistGoal : Goal
{
    private int _timesCompleted;
    private int _targetCount;
    private int _bonusPoints;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints) 
        : base(name, description, points)
    {
        _timesCompleted = 0;
        _targetCount = targetCount;
        _bonusPoints = bonusPoints;
    }

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints, int timesCompleted)
        : base(name, description, points, timesCompleted >= targetCount)
    {
        _timesCompleted = timesCompleted;
        _targetCount = targetCount;
        _bonusPoints = bonusPoints;
    }

    public override int RecordEvent()
    {
        if (!_isComplete)
        {
            _timesCompleted++;
            
            if (_timesCompleted >= _targetCount)
            {
                _isComplete = true;
                return _points + _bonusPoints;
            }
            return _points;
        }
        return 0;
    }

    public override string GetProgress()
    {
        return $"{GetCompletionStatus()} {_name} - {_description} (Completed {_timesCompleted}/{_targetCount} times)";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal|{_name}|{_description}|{_points}|{_targetCount}|{_bonusPoints}|{_timesCompleted}";
    }
}

public class NegativeGoal : Goal
{
    public NegativeGoal(string name, string description, int points) 
        : base(name, description, points) { }

    public NegativeGoal(string name, string description, int points, bool isComplete)
        : base(name, description, points, isComplete) { }

    public override int RecordEvent()
    {
        return -_points;
    }

    public override string GetProgress()
    {
        return $"[!] {_name} - {_description} (Avoid this: -{_points} points)";
    }

    public override string GetStringRepresentation()
    {
        return $"NegativeGoal|{_name}|{_description}|{_points}|{_isComplete}";
    }
}

public class ProgressiveGoal : Goal
{
    private int _currentProgress;
    private int _targetProgress;
    private int _pointsPerUnit;

    public ProgressiveGoal(string name, string description, int pointsPerUnit, int targetProgress) 
        : base(name, description, pointsPerUnit)
    {
        _currentProgress = 0;
        _targetProgress = targetProgress;
        _pointsPerUnit = pointsPerUnit;
    }

    public ProgressiveGoal(string name, string description, int pointsPerUnit, int targetProgress, int currentProgress)
        : base(name, description, pointsPerUnit, currentProgress >= targetProgress)
    {
        _currentProgress = currentProgress;
        _targetProgress = targetProgress;
        _pointsPerUnit = pointsPerUnit;
    }

    public override int RecordEvent()
    {
        if (!_isComplete)
        {
            Console.Write($"How much progress did you make on '{_name}'? ");
            if (int.TryParse(Console.ReadLine(), out int progress) && progress > 0)
            {
                _currentProgress += progress;
                int pointsEarned = progress * _pointsPerUnit;
                
                if (_currentProgress >= _targetProgress)
                {
                    _isComplete = true;
                    pointsEarned += _points * 5;
                }
                
                return pointsEarned;
            }
        }
        return 0;
    }

    public override string GetProgress()
    {
        double percentage = (_currentProgress / (double)_targetProgress) * 100;
        return $"{GetCompletionStatus()} {_name} - {_description} (Progress: {_currentProgress}/{_targetProgress} - {percentage:F1}%)";
    }

    public override string GetStringRepresentation()
    {
        return $"ProgressiveGoal|{_name}|{_description}|{_pointsPerUnit}|{_targetProgress}|{_currentProgress}";
    }
}

public class User
{
    private int _score;
    private int _level;
    private string _title;
    private List<string> _achievements;

    public User(int score = 0)
    {
        _score = score;
        _level = CalculateLevel();
        _title = GetTitleForLevel(_level);
        _achievements = new List<string>();
    }

    public void AddPoints(int points)
    {
        _score += points;
        int newLevel = CalculateLevel();
        
        if (newLevel > _level)
        {
            _level = newLevel;
            string oldTitle = _title;
            _title = GetTitleForLevel(_level);
            Console.WriteLine($"\n🎉 LEVEL UP! You reached level {_level} - {_title}! 🎉");
            
            CheckForAchievements();
        }
        
        if (points > 0)
        {
            Console.WriteLine($"\n✨ +{points} points added! Total: {_score} ✨");
        }
        else if (points < 0)
        {
            Console.WriteLine($"\n😞 {points} points deducted! Total: {_score}");
        }
    }

    private int CalculateLevel()
    {
        return (_score / 1000) + 1;
    }

    private string GetTitleForLevel(int level)
    {
        string[] titles = {
            "Beginner", "Apprentice", "Journeyman", "Adept", "Expert", 
            "Master", "Grandmaster", "Legend", "Eternal Champion", "Celestial Being"
        };
        
        int index = Math.Min(level - 1, titles.Length - 1);
        return index >= 0 ? titles[index] : titles[0];
    }

    private void CheckForAchievements()
    {
        string[] possibleAchievements = {
            "First Steps: Reach Level 2",
            "Dedicated: Reach Level 5", 
            "Master: Reach Level 10",
            "Goal Getter: Complete 10 goals",
            "Eternal Quest: Score 5000 points"
        };

        int[] achievementRequirements = { 2, 5, 10, 10, 5000 };

        for (int i = 0; i < possibleAchievements.Length; i++)
        {
            if (!_achievements.Contains(possibleAchievements[i]))
            {
                if ((i < 3 && _level >= achievementRequirements[i]) ||
                    (i == 3 && GetCompletedGoalsCount() >= achievementRequirements[i]) ||
                    (i == 4 && _score >= achievementRequirements[i]))
                {
                    _achievements.Add(possibleAchievements[i]);
                    Console.WriteLine($"🏆 ACHIEVEMENT UNLOCKED: {possibleAchievements[i]}! 🏆");
                }
            }
        }
    }

    private int GetCompletedGoalsCount()
    {
        return 0;
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"\n=== YOUR STATUS ===");
        Console.WriteLine($"Level: {_level} - {_title}");
        Console.WriteLine($"Total Score: {_score} points");
        Console.WriteLine($"Points to next level: {(_level * 1000) - _score}");
        
        if (_achievements.Count > 0)
        {
            Console.WriteLine($"\n🏆 Achievements ({_achievements.Count}):");
            foreach (string achievement in _achievements)
            {
                Console.WriteLine($"   ✓ {achievement}");
            }
        }
    }

    public int GetScore() => _score;
    public int GetLevel() => _level;
    public string GetTitle() => _title;
    public List<string> GetAchievements() => _achievements;

    public void SetAchievements(List<string> achievements)
    {
        _achievements = achievements;
    }
}

class EternalQuestProgram
{
    private List<Goal> _goals;
    private User _user;
    private string _dataFile = "goals.txt";

    public EternalQuestProgram()
    {
        _goals = new List<Goal>();
        _user = new User();
    }

    public void Run()
    {
        LoadData();
        
        Console.WriteLine("🌟 Welcome to the Eternal Quest Program! 🌟");
        Console.WriteLine("Your journey to self-improvement begins now!\n");

        bool running = true;
        while (running)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateNewGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    SaveData();
                    break;
                case "4":
                    LoadData();
                    break;
                case "5":
                    RecordEvent();
                    break;
                case "6":
                    _user.DisplayStatus();
                    break;
                case "7":
                    running = false;
                    Console.WriteLine("Thank you for using Eternal Quest! Keep striving!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("\n=== MAIN MENU ===");
        Console.WriteLine($"Current Score: {_user.GetScore()} points | Level: {_user.GetLevel()} - {_user.GetTitle()}");
        Console.WriteLine("1. Create New Goal");
        Console.WriteLine("2. List Goals");
        Console.WriteLine("3. Save Goals");
        Console.WriteLine("4. Load Goals");
        Console.WriteLine("5. Record Event");
        Console.WriteLine("6. Show My Status");
        Console.WriteLine("7. Quit");
        Console.Write("Select an option: ");
    }

    private void CreateNewGoal()
    {
        Console.WriteLine("\n=== CREATE NEW GOAL ===");
        Console.WriteLine("1. Simple Goal (one-time)");
        Console.WriteLine("2. Eternal Goal (repeating)");
        Console.WriteLine("3. Checklist Goal (multiple times)");
        Console.WriteLine("4. Negative Goal (avoid bad habits)");
        Console.WriteLine("5. Progressive Goal (track progress)");
        Console.Write("Select goal type: ");

        string typeChoice = Console.ReadLine();
        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();

        Goal newGoal = null;

        switch (typeChoice)
        {
            case "1":
                Console.Write("Enter points for completion: ");
                int points = int.Parse(Console.ReadLine());
                newGoal = new SimpleGoal(name, description, points);
                break;

            case "2":
                Console.Write("Enter points per completion: ");
                int eternalPoints = int.Parse(Console.ReadLine());
                newGoal = new EternalGoal(name, description, eternalPoints);
                break;

            case "3":
                Console.Write("Enter points per completion: ");
                int checklistPoints = int.Parse(Console.ReadLine());
                Console.Write("Enter target number of times: ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points for completion: ");
                int bonus = int.Parse(Console.ReadLine());
                newGoal = new ChecklistGoal(name, description, checklistPoints, target, bonus);
                break;

            case "4":
                Console.Write("Enter points to deduct: ");
                int negativePoints = int.Parse(Console.ReadLine());
                newGoal = new NegativeGoal(name, description, negativePoints);
                break;

            case "5":
                Console.Write("Enter points per unit of progress: ");
                int progressPoints = int.Parse(Console.ReadLine());
                Console.Write("Enter target progress amount: ");
                int targetProgress = int.Parse(Console.ReadLine());
                newGoal = new ProgressiveGoal(name, description, progressPoints, targetProgress);
                break;

            default:
                Console.WriteLine("Invalid goal type.");
                return;
        }

        _goals.Add(newGoal);
        Console.WriteLine($"\n✅ Goal '{name}' created successfully!");
    }

    private void ListGoals()
    {
        Console.WriteLine("\n=== YOUR GOALS ===");
        
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals yet. Create some goals to get started!");
            return;
        }

        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetProgress()}");
        }
    }

    private void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available. Please create some goals first.");
            return;
        }

        Console.WriteLine("\n=== RECORD EVENT ===");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetName()}");
        }

        Console.Write("Select a goal to record: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _goals.Count)
        {
            Goal selectedGoal = _goals[choice - 1];
            int pointsEarned = selectedGoal.RecordEvent();
            _user.AddPoints(pointsEarned);
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    private void SaveData()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(_dataFile))
            {
                writer.WriteLine($"User|{_user.GetScore()}|{string.Join(",", _user.GetAchievements())}");
                
                foreach (Goal goal in _goals)
                {
                    writer.WriteLine(goal.GetStringRepresentation());
                }
            }
            Console.WriteLine($"\n💾 Data saved successfully to {_dataFile}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    private void LoadData()
    {
        if (!File.Exists(_dataFile))
        {
            Console.WriteLine("No saved data found. Starting fresh!");
            return;
        }

        try
        {
            _goals.Clear();
            string[] lines = File.ReadAllLines(_dataFile);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                
                if (parts[0] == "User")
                {
                    _user = new User(int.Parse(parts[1]));
                    if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                    {
                        _user.SetAchievements(parts[2].Split(',').ToList());
                    }
                }
                else
                {
                    Goal goal = CreateGoalFromString(parts);
                    if (goal != null)
                    {
                        _goals.Add(goal);
                    }
                }
            }
            
            Console.WriteLine($"\n📂 Data loaded successfully from {_dataFile}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    private Goal CreateGoalFromString(string[] parts)
    {
        string type = parts[0];
        string name = parts[1];
        string description = parts[2];

        switch (type)
        {
            case "SimpleGoal":
                return new SimpleGoal(name, description, int.Parse(parts[3]), bool.Parse(parts[4]));
            
            case "EternalGoal":
                return new EternalGoal(name, description, int.Parse(parts[3]), int.Parse(parts[4]));
            
            case "ChecklistGoal":
                return new ChecklistGoal(name, description, int.Parse(parts[3]), int.Parse(parts[4]), 
                    int.Parse(parts[5]), int.Parse(parts[6]));
            
            case "NegativeGoal":
                return new NegativeGoal(name, description, int.Parse(parts[3]), bool.Parse(parts[4]));
            
            case "ProgressiveGoal":
                return new ProgressiveGoal(name, description, int.Parse(parts[3]), int.Parse(parts[4]), 
                    int.Parse(parts[5]));
            
            default:
                return null;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        EternalQuestProgram program = new EternalQuestProgram();
        program.Run();
    }
}