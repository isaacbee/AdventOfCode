using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public partial class Day05 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day05-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static string GetNiceStringsCountV1()
    {
        // part 1
        int count = 0;
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            Regex vowelRegex = VowelRegex();
            Regex consecutiveRegex = ConsecutiveRegex();
            Regex badPairRegex = BadPairRegex();

            if (vowelRegex.Matches(line).Count > 2) 
            {
                if (consecutiveRegex.IsMatch(line)) 
                {
                    if (!badPairRegex.IsMatch(line))
                    {
                        count++;
                    }
                }
            }
        }

        return $"{count} nice strings using v1";
    }

    private static string GetNiceStringsCountV2()
    {
        // part 2
        int count = 0;
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            Regex vowelRegex = VowelRegex();
            Regex consecutiveRegex = ConsecutiveRegex();
            Regex badPairRegex = BadPairRegex();
            Regex multiPairRegex = MultiPairRegex();
            Regex sandwichRegex = SandwichRegex();

            if (multiPairRegex.IsMatch(line))
            {
                if (sandwichRegex.IsMatch(line)) 
                {
                    count++;
                }
            }
        }

        return $"{count} nice strings using v2";
    }

    public string Answer()
    {
        return $"{GetNiceStringsCountV1()} and {GetNiceStringsCountV2()}";
    }

    [GeneratedRegex(@"[aeiou]", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex VowelRegex();
    [GeneratedRegex(@"(.)\1")]
    private static partial Regex ConsecutiveRegex();
    [GeneratedRegex(@"(ab|cd|pq|xy)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex BadPairRegex();
    [GeneratedRegex(@"(\w\w).*\1")]
    private static partial Regex MultiPairRegex();
    [GeneratedRegex(@"(\w).\1")]
    private static partial Regex SandwichRegex();
}
