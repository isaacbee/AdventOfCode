using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public partial class Day11 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day11-input.txt";
    private static readonly string inputText = "hepxcrrq";

    private static string ChangePassword(string input)
    {
        string newPassword = new(input);

        while (true)
        {
            newPassword = IncrementString(newPassword);

            if (ContainsIncreasingStraight(newPassword) && DoesNotContainAmbiguousChar(newPassword) && ContainsAtLeastTwoPairs(newPassword))
            {
                return newPassword;
            }
        }
    }

    private static string IncrementString(string input) 
    {
        char[] modified = input.ToCharArray();
        for (int i = modified.Length - 1; i >= 0; i--)
        {
            if (modified[i] == 'z')
            {
                // If the current character is 'z', wrap around to 'a' and continue the loop
                modified[i] = 'a';
            }
            else
            {
                // Increment the current character and stop the loop
                modified[i]++;
                break;
            }
        }

        return new string(modified);
    }

    private static bool ContainsIncreasingStraight(string input) 
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            if (input[i+1] == (input[i] + 1) && input[i+2] == (input[i] + 2))
            {
                return true;
            }
        }
        return false;
    }

    private static bool DoesNotContainAmbiguousChar(string input) 
    {
        Regex ambiguousRegex = AmbiguousRegex();
        return !ambiguousRegex.IsMatch(input);
    }

    private static bool ContainsAtLeastTwoPairs(string input) 
    {
        Regex pairRegex = PairRegex();
        MatchCollection matches = pairRegex.Matches(input);
        return matches.Count >= 2;
    }

    public string Answer()
    {
        string part1 = ChangePassword(inputText);

        string part2 = ChangePassword(part1);

        return $"the next password that meets the 3 requirments = {part1} and the password after that = {part2}";
    }

    [GeneratedRegex("[ilo]")]
    private static partial Regex AmbiguousRegex();
    [GeneratedRegex(@"([a-z])\1")]
    private static partial Regex PairRegex();
}
