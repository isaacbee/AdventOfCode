using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public partial class Day6 : ISolution
{
	private static readonly string filePath = $"lib\\2015\\Day6\\input.txt";
	private static readonly string inputText = File.ReadAllText(filePath);

    // part 1
	private static string GetLights()
	{
        int gridSize = 1000;
        bool[,] lights = new bool[gridSize, gridSize];
		
		int count = 0;
        string[] lines = inputText.Split('\n');

        foreach (string line in lines)
        {
            (int a, int b, int c, int d) = ExtractCoords(line);

            for (int i = int.Min(a, c); i <= int.Max(a,c); i++)
            {
                for (int j = int.Min(b, d); j <= int.Max(b, d); j++)
                {
                    if (line.StartsWith("turn on", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lights[i, j] = true;
                    }
                    else if (line.StartsWith("turn off", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lights[i, j] = false;
                    }
                    else if (line.StartsWith("toggle", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lights[i, j] = !lights[i, j];
                    }
                }
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (lights[i, j])
                    count++;
            }
        }

		return $"{count} lights are lit";
	}

    private static (int, int, int, int) ExtractCoords(string line)
    {
        Regex coordinatePairsRegex = CoordinatePairsRegex();
        MatchCollection matches = coordinatePairsRegex.Matches(line);

        string[] firstPair = matches[0].Value.Split(',');
        string[] secondPair = matches[1].Value.Split(',');

        return (int.Parse(firstPair[0]), int.Parse(firstPair[1]), int.Parse(secondPair[0]), int.Parse(secondPair[1]));
    }

    // part 2
    private static string GetBrightness()
	{
        int gridSize = 1000;
        int[,] brightness = new int[gridSize, gridSize];
        
		int totalBrightness = 0;
        string[] lines = inputText.Split('\n');

        foreach (string line in lines)
        {
            (int a, int b, int c, int d) = ExtractCoords(line);

            for (int i = int.Min(a, c); i <= int.Max(a,c); i++)
            {
                for (int j = int.Min(b, d); j <= int.Max(b, d); j++)
                {
                    if (line.StartsWith("turn on", StringComparison.CurrentCultureIgnoreCase))
                    {
                        brightness[i, j]++;
                    }
                    else if (line.StartsWith("turn off", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (--brightness[i, j] < 0)
                            brightness[i, j] = 0;
                    }
                    else if (line.StartsWith("toggle", StringComparison.CurrentCultureIgnoreCase))
                    {
                        brightness[i, j] += 2;
                    }
                }
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                totalBrightness += brightness[i, j];
            }
        }

		return $"the total brightness {totalBrightness}";
	}

	public string Answer()
	{
		return $"{GetLights()} and {GetBrightness()}";
	}

    [GeneratedRegex(@"\d+,\d+")]
    private static partial Regex CoordinatePairsRegex();
}
