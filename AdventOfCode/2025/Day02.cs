using System.Text.RegularExpressions;

namespace AdventOfCode._2025;

public partial class Day02 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2025", "Day02-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(long start, long end)> productRanges = InitProductRanges();

    private static List<(long start, long end)> InitProductRanges()
    {
        string[] tokens = inputText.Split(",");
        List<(long start, long end)> newProductRanges = [];

        foreach (string token in tokens)
        {
            string[] range = token.Split("-");
            newProductRanges.Add((long.Parse(range[0]), long.Parse(range[1])));
        }

        return newProductRanges;
    }

    /// <summary>
    /// Sums the IDs in the provided ranges that are invalid.
    /// </summary>
    /// <param name="isOnlyTwice">True if the invalid IDs contain a sequence repeated exactly twice. False if the invalid IDs contain a sequence repeated at least twice.</param>
    private static long InvalidSum(bool isOnlyTwice = true)
    {
        long invalidSum = 0;

        foreach ((long start, long end) in productRanges)
        {
            for (long i = start; i <= end; i++)
            {
                if (isOnlyTwice && IsRepeatedTwice(i.ToString()))
                    invalidSum += i;

                if (!isOnlyTwice && IsRepeatedMultiple(i.ToString()))
                    invalidSum += i;
            }
        }

        return invalidSum;
    }

    /// <summary>
    /// Checks if the string is only made up of a sequence of characters that is repeated at exactly twice.
    /// </summary>
    private static bool IsRepeatedTwice(string id)
    {
        // The string must have an even length to be a repeated sequence
        if (id.Length % 2 != 0)
        {
            return false;
        }

        int half = id.Length / 2;
        string firstHalf = id[..half];
        string secondHalf = id[half..];

        return firstHalf == secondHalf;
    }

    /// <summary>
    /// Checks if the string is only made up of a sequence, or "chunk", of characters that is repeated at least twice.
    /// </summary>
    private static bool IsRepeatedMultiple(string id)
    {
        int len = id.Length;

        for (int chunkSize = 1; chunkSize <= len / 2; chunkSize++)
        {
            // Only consider chunk sizes that evenly divide the string
            if (len % chunkSize != 0) continue;

            string chunk = id[..chunkSize];
            bool allMatch = true;

            for (int i = chunkSize; i < len; i += chunkSize)
            {
                if (id.Substring(i, chunkSize) != chunk)
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch) return true;
        }
        
        return false;
    }

    /// <summary>
    /// Unused Regex solution to Part 1 (slower than manual substring check).
    /// </summary>
    private static bool IsRepeatedTwiceRegex(string input)
    {
        return RepeatedTwice().IsMatch(input);
    }

    /// <summary>
    /// Unused Regex solution to Part 2 (slower than manual substring check).
    /// </summary>
    private static bool IsRepeatedMultipleRegex(string input)
    {
        return RepeatedMultiple().IsMatch(input);
    }

    public string Answer()
    {
        // part 1
        long twiceInvalidIDSum = InvalidSum();

        // part 2
        long multipleInvalidIDSum = InvalidSum(false);

        return $"The sum of all invalid IDs is {twiceInvalidIDSum} (when there is a sequence of digits repeated twice); the sum of all invalid IDs is {multipleInvalidIDSum} (when there is a sequence of digits repeated at least twice)";
    }

    /// <summary>
    /// Regex explanation:
    ///    ^        → start of string
    ///    (\d+)    → capture one or more digits
    ///    \1       → match the same captured group again
    ///    $        → end of string
    /// </summary>
    [GeneratedRegex(@"^(\d+)\1$")]
    private static partial Regex RepeatedTwice();

    /// <summary>
    /// Regex explanation:
    ///    ^        → start of string
    ///    (\d+)    → capture one or more digits
    ///    (\1)+    → match the same captured group one or more times
    ///    $        → end of string
    /// </summary>
    [GeneratedRegex(@"^(\d+)(\1)+$")]
    private static partial Regex RepeatedMultiple();
}