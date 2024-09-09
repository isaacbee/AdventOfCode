using System;

namespace AdventOfCode._2015;

public class Day1 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day1-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static string GetFloor()
    {
        int floor = 0;
        int basementPos = 0;
        int count = 0;

        foreach (char c in inputText)
        {
            // part 1
            switch (c)
            {
                case '(':
                    floor++;
                    break;
                case ')':
                    floor--;
                    break;
                default:
                    // do nothing
                    break;
            }

            // part 2
            count++;
            if (basementPos == 0 && floor < 0)
                basementPos = count;
        }

        return $"floor {floor} and basement position {basementPos}";
    }

    public string Answer()
    {
        return GetFloor();
    }
}
