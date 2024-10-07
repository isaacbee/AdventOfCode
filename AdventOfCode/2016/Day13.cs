using CommunityToolkit.HighPerformance;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode._2016;

public class Day13 : ISolution
{
    private static readonly int input = 1364;

    /// <summary>
    /// Maze size maximum. For the program input, 42 is the minimum size to fit the full solution without the traversal hitting a wall. Initially set to much higher to guarantee a solution.
    /// </summary>
    private const int maxSize = 42;
    private static readonly bool[,] maze = InitMaze(maxSize);

    /// <summary>
    /// Initialize the maze. <c>true</c> is a wall, while <c>false</c> is an open space.
    /// </summary>
    /// <param name="size">Maximum maze size.</param>
    /// <returns>The maze, represented by a bool[,].</returns>
    private static bool[,] InitMaze(int size)
    {
        bool[,] m = new bool[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int n = i*i + 3*i + 2*i*j + j + j*j + input;
                n = CountBits(n);
                if (n % 2 != 0)
                {
                    m[i, j] = true;
                }
            }
        }
        return m;
    }

    /// <summary>
    /// Counts the number of bits in an integer that are a <c>1</c>.
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <returns>The number of bits that are <c>1</c>.</returns>
    private static int CountBits(int n)
    {
        int count = 0;
        while (n != 0)
        {
            count += n & 1;
            n >>= 1;
        }
        return count;
    }

    /// <summary>
    /// Program entry to determine the steps required to traverse the shortest-length path between <paramref name="start"/> and <paramref name="end"/>. This is accomplished via a BFS traversal of the maze.
    /// </summary>
    /// <param name="start">The start coordinates.</param>
    /// <param name="end">The end coordinates.</param>
    /// <returns>The steps required to traverse the shortest path.</returns>
    private static int FindShortestPath((int x, int y) start, (int x, int y) end)
    {
        Queue<((int x, int y), int steps)> q = [];
        q.Enqueue((start, 0));

        Dictionary<(int, int), int> visited = [];

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
        throw new Exception($"No path found from {start} to {end}");
    }

    /// <summary>
    /// Program entry to determine the count of distinct locations visited in a traversal of the maze after at most <paramref name="maxSteps"/>. This is accomplished via a BFS traversal of the maze. 
    /// </summary>
    /// <remarks>(Optional) Print the traversal.</remarks>
    /// <param name="start">The start coordinates.</param>
    /// <param name="maxSteps">The maximum steps to traverse.</param>
    /// <param name="end">The end coordinates.</param>
    /// <param name="doPrint">Print the traversal after finishing.</param>
    /// <returns></returns>
    private static int CountDistinctLocations((int x, int y) start, int maxSteps, (int, int) end = default, bool doPrint = false)
    {
        Queue<((int x, int y), int steps)> q = [];
        Dictionary<(int, int), int> visited = [];

        q.Enqueue((start, 0));
        visited.Add(start, 0);
        int count = 1;

        while (q.Count > 0)
        {
            ((int x, int y), int steps) = q.Dequeue();

            if (steps + 1 <= maxSteps)
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    (int x, int y) next = (x, y).Add(directions[i]);

                    if (IsOpenSpace(next.x, next.y))
                    {
                        if (visited.ContainsKey((next.x, next.y)) == false)
                        {
                            count++;
                            visited.Add(next, steps + 1);
                            q.Enqueue((next, steps + 1));
                        }
                    }
                }
            }
        }
        if (doPrint) PrintTraversal(start, end, visited, maxSteps);
        return count;
    }

    /// <summary>
    /// Prints the maze traversal. <c>(space)</c> for wall, <c>*</c> for start, <c>#</c> for path, <c>?</c> for every point reached on path after exactly <paramref name="maxSteps"/> steps, <c>!</c> for end (if reached), and <c>¡</c> for end (if not reached).
    /// </summary>
    /// <param name="start">The start coordinates.</param>
    /// <param name="end">The end coordinates.</param>
    /// <param name="visited">The collection containing all visited locations.</param>
    /// <param name="maxSteps">The maximum steps taken in the traversal.</param>
    private static void PrintTraversal((int x, int y) start, (int x, int y) end, Dictionary<(int, int), int> visited, int maxSteps)
    {
        Console.Write(' ');
        for (int j = 0; j < maxSize; j++)
        {
            Console.Write('▄');
        }
        Console.WriteLine(' ');

        for (int i = 0; i < maxSize; i++)
        {
            Console.Write('▐');
            for (int j = 0; j < maxSize; j++)
            {
                if ((j,i) == start) Console.Write('*');
                else if (visited.ContainsKey((j,i)))
                {
                    if ((j,i) == end) Console.Write('!');
                    else if (visited[(j,i)] == maxSteps) Console.Write('?');
                    else Console.Write('#');
                }
                else if ((j,i) == end) Console.Write('¡');
                else if (maze[j,i] == false) Console.Write('·');
                else Console.Write(' ');
            }
            Console.WriteLine('▌');
        }

        Console.Write(' ');
        for (int j = 0; j < maxSize; j++)
        {
            Console.Write('▀');
        }
        Console.WriteLine(' ');
    }

    /// <summary>
    /// All (dx, dy) possible next steps when traversing a maze.
    /// </summary>
    private static readonly (int, int)[] directions =
    [
        (0, 1), (1, 0), (0, -1), (-1, 0)
    ];

    /// <summary>
    /// Checks if the input (x,y) coordinate is inside the maze and and not a wall.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns><c>true</c> if the input coordinate is a valid open-space location; otherwise, <c>false</c></returns>
    private static bool IsOpenSpace(int x, int y)
    {
        if (x >= 0 && x < maxSize && y >= 0 && y < maxSize)
        {
            if (maze[x, y] == false)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Prints the full maze. <c>(space)</c> for open space, <c>*</c> for start, <c>█</c> for wall, <c>!</c> for end.
    /// </summary>
    /// <param name="start">The start coordinates.</param>
    /// <param name="end">The end coordinates.</param>
    private static void PrintFullMaze((int x, int y) start, (int x, int y) end)
    {
        Console.Write(' ');
        for (int j = 0; j < maxSize; j++)
        {
            Console.Write('▄');
        }
        Console.WriteLine(' ');

        for (int i = 0; i < maxSize; i++)
        {
            Console.Write('▐');
            for (int j = 0; j < maxSize; j++)
            {
                if ((j,i) == start) Console.Write('*');
                else if ((j,i) == end) Console.Write('!');
                else Console.Write(maze[j,i] ? '█' : ' ');
            }
            Console.WriteLine('▌');
        }

        Console.Write(' ');
        for (int j = 0; j < maxSize; j++)
        {
            Console.Write('▀');
        }
        Console.WriteLine(' ');
    }

    public string Answer()
    {
        (int, int) start = (1, 1);

        // part 1
        (int, int) end = (31, 39);
        int steps = FindShortestPath(start, end);

        // // show the full maze
        // PrintFullMaze(start, end);

        // show the traversal for part 1
        _ = CountDistinctLocations(start, steps, end, true);

        // part 2
        int maxSteps = 50;
        int locations = CountDistinctLocations(start, maxSteps, end, false);

        return $"the fewest number of steps required to reach {end} = {steps}; and the number of distinct locations that can be reached within {maxSteps} steps = {locations}";
    }
}