using System;

class Program
{
    static void Main(string[] args)
    {
        var rnd = new Random();
        int magicNumber = rnd.Next(1, 101); // random number in [1,100]
        int guess = -1;
        int attempts = 0;

        Console.WriteLine("I'm thinking of a number between 1 and 100.");
        // Uncomment the next line while testing to reveal the secret:
        // Console.WriteLine($"(Debug) Magic number is: {magicNumber}");

        while (guess != magicNumber)
        {
            Console.Write("What is your guess? ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out guess))
            {
                Console.WriteLine("Please enter a valid integer.");
                continue;
            }

            attempts++;

            if (guess < magicNumber)
            {
                Console.WriteLine("Higher");
            }
            else if (guess > magicNumber)
            {
                Console.WriteLine("Lower");
            }
            else // guess == magicNumber
            {
                Console.WriteLine("You guessed it!");
                Console.WriteLine($"It took you {attempts} attempt{(attempts == 1 ? "" : "s")}.");
            }
        }
    }
}
