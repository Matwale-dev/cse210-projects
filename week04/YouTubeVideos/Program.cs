using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string CommentText { get; set; }

    public Comment(string commenterName, string commentText)
    {
        CommenterName = commenterName;
        CommentText = commentText;
    }

    public void Display()
    {
        Console.WriteLine($"{CommenterName}: {CommentText}");
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    private List<Comment> Comments { get; set; }

    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return Comments.Count;
    }

    private string GetFormattedLength()
    {
        int minutes = LengthInSeconds / 60;
        int seconds = LengthInSeconds % 60;
        return $"{minutes}:{seconds:D2}"; // D2 ensures two digits for seconds
    }

    public void Display()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {GetFormattedLength()} (mm:ss)");
        Console.WriteLine($"Number of comments: {GetNumberOfComments()}");

        Console.WriteLine("Comments:");
        foreach (Comment comment in Comments)
        {
            comment.Display();
        }
        Console.WriteLine();
    }
}

public class Program
{
    public static void Main()
    {
        // Create video objects
        Video video1 = new Video("C# OOP Basics", "Code Academy", 600);
        Video video2 = new Video("Learn Python in 10 Minutes", "Tech Guru", 720);
        Video video3 = new Video("Top 10 AI Trends", "Future Tech", 905);

        // Add comments to video1
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "This really helped me."));
        video1.AddComment(new Comment("Charlie", "Clear and concise."));

        // Add comments to video2
        video2.AddComment(new Comment("David", "Python is awesome!"));
        video2.AddComment(new Comment("Emma", "Loved the quick breakdown."));
        video2.AddComment(new Comment("Frank", "Can you make a follow-up video?"));

        // Add comments to video3
        video3.AddComment(new Comment("Grace", "AI is the future."));
        video3.AddComment(new Comment("Henry", "Very informative."));
        video3.AddComment(new Comment("Isla", "Excited to see what’s next!"));

        // Store videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display each video with comments
        foreach (Video video in videos)
        {
            video.Display();
        }
    }
}
