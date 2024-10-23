namespace AdventOfCode._2016;

public class Day24 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day24-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly char[,] maze = InitAirDucts();

    private static char[,] InitAirDucts()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        char[,] m = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                m[i, j] = lines[i][j];
            }
        }
        return m;
    }

    private static (int x, int y) FindLocation(char c)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == c)
                {
                    return (j, i);
                }
            }
        }
        throw new Exception($"'{c}' not found in the maze.");
    }

    /// <summary>
    /// All (dx, dy) possible next steps when traversing a maze.
    /// </summary>
    private static readonly (int, int)[] directions =
    [
        (0, 1), (1, 0), (0, -1), (-1, 0)
    ];

    private static int FindShortestPath(char a, char b)
    {
        Queue<((int x, int y), int steps)> q = [];
        (int x, int y) start = FindLocation(a);
        (int x, int y) end = FindLocation(b);

        if (a == b)
        {
            return 0;
        }

        q.Enqueue((start, 0));
        Dictionary<(int x, int y), int> visited = new()
        {
            { start, 0 }
        };

        while (q.Count > 0)
        {
            ((int x, int y), int steps) = q.Dequeue();

            for (int i = 0; i < directions.Length; i++)
            {
                (int x, int y) next = (x, y).Add(directions[i]);

                if (next == end)
                {
                    return steps + 1;
                }

                if (IsOpenSpace(next.x, next.y))
                {
                    if (visited.ContainsKey((next.x, next.y)) == false)
                    {
                        visited.Add(next, steps + 1);
                        q.Enqueue((next, steps + 1));
                    }
                }
            }
        }
        throw new Exception($"No path found from '{a}' to '{b}'");
    }

    private static int FindShortestPath(int a, int b)
    {
        char firstChar = '0';
        return FindShortestPath((char) (a + firstChar), (char) (b + firstChar));
    }

    /// <summary>
    /// Checks if the input (x,y) coordinate is inside the maze and and not a wall.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns><c>true</c> if the input coordinate is a valid open-space location; otherwise, <c>false</c></returns>
    private static bool IsOpenSpace(int x, int y)
    {
        if (x >= 0 && x < maze.GetLength(1) && y >= 0 && y < maze.GetLength(0))
        {
            if (maze[y, x] != '#')
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the input (x,y) coordinate is a point of interest.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="l">The char representing the location of interest.</param>
    /// <returns><c>true</c> if the input coordinate is a point of interest, <see cref="location"/> contains the point of interest; otherwise, <c>false</c></returns>
    private static bool TryGetLocation(int x, int y, out char location)
    {
        location = '\0';
        if (x >= 0 && x < maze.GetLength(1) && y >= 0 && y < maze.GetLength(0))
        {
            location = maze[y, x];
            if (char.IsAsciiDigit(location))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Uses a version of the TSP to compute the shortest path that visits every location of interest in the maze. We precompute the individual distances from every node to every other node to reduce the runtime to O(n!).
    /// </summary>
    /// <param name="start"></param>
    /// <param name="count"></param>
    /// <param name="returnToStart"></param>
    /// <returns></returns>
    private static int FindShortestCompletePath(int start, int count, bool returnToStart = false)
    {
        // weightedGraph is a matrix which contains the number of steps required to reach each node j from each node i.
        int[,] weightedGraph = new int[count + 1, count + 1];
        for (int i = 0; i < count + 1; i++)
        {
            for (int j = i; j < count + 1; j++)
            {
                weightedGraph[i, j] = weightedGraph[j, i] = FindShortestPath(i, j);
            }
        }

        // cost is a matrix whose entries are all infinity except when the mask only includes itself.
        int[,] cost = new int[1 << count, count];
        for (int mask = 0; mask < (1 << count); mask++)
        {
            for (int i = 0; i < count; i++)
            {
                if (1 << i == mask)
                {
                    // Adds the weight for visiting each single node first.
                    cost[mask, i] = weightedGraph[0, i + 1];
                }
                else
                {
                    cost[mask, i] = int.MaxValue;
                }
            }
        }
        
        // Iterate over all remaining sets of visited nodes.
        for (int mask = 1; mask < (1 << count); mask++)
        {
            // Iterate over every starting node u
            for (int u = 0; u < count; u++)
            {
                if ((mask & (1 << u)) == 0) continue; // Skip node u if it is not in the mask
                
                // Try to extend the path from node u to every other node v
                for (int v = 0; v < count; v++)
                {
                    if ((mask & (1 << v)) != 0) continue; // Skip node v if it is already in the mask
                    
                    int newMask = mask | (1 << v); // Add node v to the mask
                    cost[newMask, v] = Math.Min(cost[newMask, v], cost[mask, u] + weightedGraph[u + 1, v + 1]); // Compute the new shortest distance for the new mask
                }
            }
        }

        // The final answer is the shortest path that visits all nodes.
        int result = int.MaxValue;
        for (int u = 0; u < count; u++)
        {
            if (u != start)
            {
                if (returnToStart)
                {
                    // Part 2 only: Add the steps required to return to start.
                    cost[(1 << count) - 1, u] += weightedGraph[u + 1, start];
                }
                result = Math.Min(result, cost[(1 << count) - 1, u]);
            }
        }

        return result;
    }

    public string Answer()
    {
        int start = 0;
        int count = 7;

        // part 1
        int result1 = FindShortestCompletePath(start, count);

        // part 2
        int result2 = FindShortestCompletePath(start, count, true);

        return $"starting from location 0, the fewest number of steps required to visit every non-0 number marked on the map at least once = {result1}; and starting from location 0, the fewest number of steps required to visit every non-0 number marked on the map at least once, and then return to 0 = {result2}";
    }
}