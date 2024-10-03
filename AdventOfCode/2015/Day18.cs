using System.Text;

namespace AdventOfCode._2015;

public class Day18 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2015", "Day18-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);

    private static bool[,] InitLights()
    {
        int gridSize = 100;
        bool[,] lights = new bool[gridSize, gridSize];
        int t = lights.GetLength(1);
        string[] lines = inputText.Split(Environment.NewLine);

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                lights[i, j] = lines[i][j] switch
                {
                    '#' => true,
                    '.' => false,
                    _ => false
                };
            }
        }

        return lights;
    }

    private static (int count, bool[,] newLights) CalculateOnLights(bool[,] originalLights, int remSteps, bool isCornersFaulty = false)
    {
        (int x, int y)[] allAdjacent = [
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1)
        ];

        int xLength = originalLights.GetLength(0);
        int yLength = originalLights.GetLength(1);
        bool[,] curLights = new bool[xLength, yLength];
        bool[,] newLights = new bool[xLength, yLength];
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                newLights[i, j] = originalLights[i, j];
                if (isCornersFaulty && IsCorner(i, j, xLength, yLength))
                {
                    newLights[i, j] = true;
                }
            }
        }
        
        while (remSteps > 0)
        {
            // PrintLights(newLights);
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    curLights[i, j] = newLights[i, j];
                }
            }
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    if (isCornersFaulty && IsCorner(i, j, xLength, yLength))
                    {
                        newLights[i, j] = true;
                    }
                    else {
                        int neighbors = 0;
                        for (int k = 0; k < allAdjacent.Length; k++)
                        {
                            int m = i + allAdjacent[k].x;
                            int n = j + allAdjacent[k].y;

                            // if neighbor is inbounds and is on
                            if ((m >= 0 && m < xLength && n >= 0 && n < yLength) && curLights[m, n] is true)
                            {
                                neighbors++;
                            }
                        }

                        if (curLights[i, j] is true)
                        {
                            newLights[i, j] = neighbors is 2 || neighbors is 3;
                        }
                        else
                        {
                            newLights[i, j] = neighbors is 3;
                        }
                    }
                }
            }
            remSteps--;
        }

        int count = 0;
        foreach (bool light in newLights)
        {
            count += light ? 1 : 0;
        }

        return (count, newLights);
    }

    private static bool IsCorner(int x, int y, int xMax, int yMax)
    {
        return (x, y) == (0, 0) || 
            (x, y) == (0, xMax - 1) ||
            (x, y) == (yMax - 1, 0) || 
            (x, y) == (yMax - 1, xMax - 1);
    }

    private static string LightsToString(bool[,] lights)
    {
        int xLength = lights.GetLength(0);
        int yLength = lights.GetLength(1);
        StringBuilder sb = new();
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                sb.Append(lights[i, j] ? '#' : '.');
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public string Answer()
    {
        bool[,] lights = InitLights();
        int steps = 100;

        // part 1
        (int count1, bool[,] lights1) = CalculateOnLights(lights, steps);

        // part 2
        (int count2, bool[,] lights2) = CalculateOnLights(lights, steps, true);

        return $"{count1} lights are on after {steps} steps; and {count2} lights are on after {steps} steps when the corners are stuck on";
    }
}
