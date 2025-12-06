using CommunityToolkit.HighPerformance;
using Microsoft.CodeAnalysis;

namespace AdventOfCode._2025;

public class Day04 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2025", "Day04-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly string[] inputTextLines = inputText.Split(Environment.NewLine);
    private static readonly bool[,] paperLocations = InitPaperRolls();
    private static readonly bool[,] paperUpdatedLocations = InitPaperRolls();
    private static (int x, int y) dimensions;

    /// <summary>
    /// Initialize the paper roll locations. <c>true</c> is a roll of paper, while <c>false</c> is an open space.
    /// </summary>
    private static bool[,] InitPaperRolls()
    {
        string[] lines = inputText.Split(Environment.NewLine);

        dimensions = (inputTextLines[0].Length, inputTextLines.Length);

        bool[,] p = new bool[dimensions.x, dimensions.y];
        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                p[i, j] = lines[i][j] switch
                {
                    '@' => true,
                    '.' => false,
                    _ => false
                };
            }
        }
        return p;
    }

    /// <summary>
    /// Counts the number of paper rolls that can be accessed. A roll is inaccessable if it surrounded by more than the limit in the eight adjacent positions. Part 1 needs just the initial access count. Part 2 <b>removes</b> all accessable paper rolls and repeats until no more can be removed.
    /// </summary>
    private static int CountPaperRollAccess(bool updateAndRepeat = false, int limit = 3)
    {
        int count = 0;
        int prevCount;

        do 
        {
            prevCount = count;

            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (paperLocations[i, j] == true && AdjacentCount((i, j)) <= limit)
                    {
                        count++;
                        if (updateAndRepeat) paperUpdatedLocations[i, j] = false;
                    }
                }
            }

            if (updateAndRepeat)
            {
                for (int i = 0; i < dimensions.x; i++)
                {
                    for (int j = 0; j < dimensions.y; j++)
                    {
                        paperLocations[i, j] = paperUpdatedLocations[i, j];
                    }
                }
            }
        } while (updateAndRepeat is true && prevCount != count);

        return count;
    }

    /// <summary>
    /// Reworked adjacency checker from 2015.18. Counts the number of "neighbors" that are <c>true</c>. Neighbors are the eight adjacent positions.
    /// </summary>
    private static int AdjacentCount((int x, int y) coords)
    {
        int neighbors = 0;

        (int dx, int dy)[] allAdjacent = [
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1)
        ];

        for (int k = 0; k < allAdjacent.Length; k++)
        {
            int m = coords.x + allAdjacent[k].dx;
            int n = coords.y + allAdjacent[k].dy;

            // if neighbor is inbounds and is true
            if ((m >= 0 && m < dimensions.x && n >= 0 && n < dimensions.y) && paperLocations[m, n] is true)
            {
                neighbors++;
            }
        }

        return neighbors;
    }

    public string Answer()
    {
        // part 1
        int accessCount1 = CountPaperRollAccess();

        // part 2
        int accessCount2 = CountPaperRollAccess(true);

        return $"{accessCount1} rolls of paper can be accessed by a forklift initially; {accessCount2} rolls of paper can be accessed by a forklift in total";
    }
}