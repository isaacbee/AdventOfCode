using MathNet.Numerics;

namespace AdventOfCode._2025;

public class Day03 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2025", "Day03-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<string> batteryBanks = InitBatteryBanks();

    private static List<string> InitBatteryBanks()
    {
        string[] banks = inputText.Split(Environment.NewLine);

        return [.. banks];
    }

    /// <summary>
    /// Given a number of batteries to select from each battery bank, sum the maximum joltage of every battery bank.
    /// </summary>
    private static long TotalJoltage(int batteries)
    {
        long sum = 0;

        foreach (string bank in batteryBanks)
        {
            sum += FindMaximumJoltage(bank, batteries);
        }

        return sum;
    }

    /// <summary>
    /// Given a number of batteries to select from each battery bank, calculate the maximum joltage of a given battery bank. The maximum joltage is the maximum value when reading digits equal to the number of batteries selected from left to right. (You can skip digits.)
    /// </summary>
    private static long FindMaximumJoltage(string bank, int batteries)
    {
        long joltage = 0;
        int lastBattery = -1;

        for (int i = batteries; i > 0; i--)
        {
            (int digit, int pos) = FindLargestDigit(bank[++lastBattery..^(i - 1)]);

            joltage = joltage * 10 + digit;
            lastBattery += pos;
        }

        return joltage;
    }

    /// <summary>
    /// Find the largest digit in a string of digits.
    /// </summary>
    /// <returns><c>(int, int)</c> representing the largest digit and its index in the string.</returns>
    private static (int digit, int pos) FindLargestDigit(string input)
    {
        int maxDigit = -1;
        int maxPos = -1;

        for (int i = 0; i < input.Length; i++)
        {
            int digit = input[i] - '0';
            if (digit > maxDigit)
            {
                maxDigit = digit;
                maxPos = i;
            }

            if (digit == 9) break;
        }

        return (maxDigit, maxPos);
    }

    public string Answer()
    {
        // part 1
        int batteries1 = 2;
        long totalJoltage1 = TotalJoltage(batteries1);

        // part 2
        int batteries2 = 12;
        long totalJoltage2 = TotalJoltage(batteries2);

        return $"Using the maximum joltage possible from each bank with {batteries1} batteries, the total output joltage is {totalJoltage1} ; using the maximum joltage possible from each bank with {batteries2} batteries, the total output joltage is {totalJoltage2}";
    }
}