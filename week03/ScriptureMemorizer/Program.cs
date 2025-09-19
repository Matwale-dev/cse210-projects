using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public override string ToString()
    {
        return _startVerse == _endVerse
            ? $"{_book} {_chapter}:{_startVerse}"
            : $"{_book} {_chapter}:{_startVerse}-{_endVerse}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        return _isHidden ? new string('_', _text.Length) : _text;
    }
}

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public void HideRandomWords(int count)
    {
        Random rand = new Random();
        var visibleWords = _words.Where(w => !w.IsHidden()).ToList();
        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            int index = rand.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(w => w.IsHidden());
    }

    public string GetDisplayText()
    {
        string words = string.Join(" ", _words.Select(w => w.GetDisplayText()));
        return $"{_reference}\n{words}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Load scriptures from file
        List<Scripture> scriptures = LoadScripturesFromFile("scripture.txt");
        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found in scripture.txt.");
            return;
        }

        // Choose a random scripture from the library
        Random rand = new Random();
        Scripture scripture = scriptures[rand.Next(scriptures.Count)];

        // Main memorization loop
        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to continue or type 'quit' to exit:");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
                break;

            scripture.HideRandomWords(3);

            if (scripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                break;
            }
        }
    }

    static List<Scripture> LoadScripturesFromFile(string filename)
    {
        List<Scripture> scriptures = new List<Scripture>();

        if (!File.Exists(filename))
            return scriptures;

        foreach (var line in File.ReadAllLines(filename))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // Expected format: Book|Chapter|StartVerse|EndVerse|Text
            string[] parts = line.Split('|');
            if (parts.Length < 5) continue;

            string book = parts[0];
            int chapter = int.Parse(parts[1]);
            int startVerse = int.Parse(parts[2]);
            int endVerse = int.Parse(parts[3]);
            string text = parts[4];

            Reference reference = (startVerse == endVerse)
                ? new Reference(book, chapter, startVerse)
                : new Reference(book, chapter, startVerse, endVerse);

            scriptures.Add(new Scripture(reference, text));
        }

        return scriptures;
    }
}
