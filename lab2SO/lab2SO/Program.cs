
namespace lab2SO;

using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        bool positiveOnly = args.Contains("--positive-only");

        string input = Console.In.ReadToEnd();
        string[] words = input.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (int.TryParse(word, out int number))
            {
                if (!positiveOnly || (positiveOnly && number > 0))
                {
                    Console.WriteLine(number);
                }
            }
        }
    }
}

