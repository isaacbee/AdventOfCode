using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public class Day10 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day10\\input.txt";
    private static readonly string inputText = "3113322113";


    private static string LookAndSay(string input, int iterations)
    {
        StringBuilder sb = new();
        char last = input[0];
        int lastCount = 0;

        foreach(char c in input)
        {
            if (last == c)
            {
                lastCount++;
            }
            else
            {
                sb.Append(lastCount);
                sb.Append(last);
                last = c;
                lastCount = 1;
            }
        }

        sb.Append(lastCount);
        sb.Append(last);

        if (iterations > 1)
        {
            return LookAndSay(sb.ToString(), iterations - 1);
        }
        else 
        {
            return sb.ToString();
        }
    }

    public string Answer()
    {
        string part1 = LookAndSay(inputText, 40);

        string part2 = LookAndSay(inputText, 50);

        return $"a length of {part1.Length} after 40 iterations and a length of {part2.Length} after 50 iterations";
    }
}
