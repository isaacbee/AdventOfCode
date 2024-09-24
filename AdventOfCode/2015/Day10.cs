using System.Text;

namespace AdventOfCode._2015;

public class Day10 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day10-input.txt";
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
        // part 1
        string result1 = LookAndSay(inputText, 40);

        // part 2
        string result2 = LookAndSay(inputText, 50);

        return $"a length of {result1.Length} after 40 iterations; and a length of {result2.Length} after 50 iterations";
    }
}
