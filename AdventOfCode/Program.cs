using System.Diagnostics;

namespace AdventOfCode;

public class Program
{
    public static readonly Stopwatch sw = new();

    static void Main(string[] args)
    {
        sw.Start();

        // // Run all solutions
        // RunAllSolutions();

        // // Run an event's solutions
        // Run2016Solutions();

        // Run individual solutions
        RunSolution(new _2016.Day17(), false);
    }

    /// <summary>
    /// Runs the solution for the given day of the Advent of Code event and prints the result to the <see cref="Console"/>
    /// </summary>
    /// <param name="Solve">The input day of the Advent of Code event</param>
    /// <param name="showTime">If true, prefix the result with the current runtime</param>
    /// <param name="isPrintPaused">If true, pause the <see cref="Stopwatch"/> while printing the result</param>
    /// <param name="isStopwatchReset">If true, reset the <see cref="Stopwatch"/> before running the solution</param>
    static void RunSolution(ISolution solve, bool catchExceptions = true, bool showTime = true, bool isPrintPaused = false, bool isStopwatchReset = false)
    {
        if (isStopwatchReset) sw.Reset();
        sw.Start();

        if (catchExceptions)
        {
            try
            {
                string answer = solve.Answer();
                PrintTimer(showTime, isPrintPaused);
                Console.WriteLine($"The solution to {solve}: {answer}");
            }
            catch (Exception e)
            {
                PrintTimer(showTime, isPrintPaused);
                Console.WriteLine($"Error when running {solve}: {e}");
            }
        }
        else
        {
            string answer = solve.Answer();
            PrintTimer(showTime, isPrintPaused);
            Console.WriteLine($"The solution to {solve}: {answer}");
        }

        sw.Stop();
    }

    /// <summary>
    /// Print program elapsed time
    /// </summary>
    /// <param name="showTime">If true, print the current runtime</param>
    /// <param name="isPrintPaused">If true, pause the <see cref="Stopwatch"/> while printing</param>
    private static void PrintTimer(bool showTime = true, bool isPrintPaused = false)
    {
        if (isPrintPaused) sw.Stop();
        TimeSpan ts = sw.Elapsed;

        if (showTime) Console.Write($"[{string.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds)}] ");
    }

    /// <summary>
    /// Runs the solution for every day of the 2015 Advent of Code event
    /// </summary>
    static void Run2015Solutions()
    {
        ISolution[] _2015solutions = [ 
            new _2015.Day01(), 
            new _2015.Day02(), 
            new _2015.Day03(), 
            new _2015.Day04(), 
            new _2015.Day05(), 
            new _2015.Day06(), 
            new _2015.Day07(), 
            new _2015.Day08(), 
            new _2015.Day09(), 
            new _2015.Day10(), 
            new _2015.Day11(), 
            new _2015.Day12(), 
            new _2015.Day13(), 
            new _2015.Day14(), 
            new _2015.Day15(), 
            new _2015.Day16(), 
            new _2015.Day17(), 
            new _2015.Day18(), 
            new _2015.Day19(), 
            new _2015.Day20(), 
            new _2015.Day21(), 
            new _2015.Day22(), 
            new _2015.Day23(), 
            new _2015.Day24(), 
            new _2015.Day25()
        ];
        
        foreach (var solution in _2015solutions)
        {
            RunSolution(solution, false);
        }
    }

    /// <summary>
    /// Runs the solution for every day of the 2016 Advent of Code event
    /// </summary>
    static void Run2016Solutions()
    {
        ISolution[] _2016solutions = [ 
            new _2016.Day01(), 
            new _2016.Day02(), 
            new _2016.Day03(), 
            new _2016.Day04(), 
            new _2016.Day05(), 
            new _2016.Day06(), 
            new _2016.Day07(), 
            new _2016.Day08(), 
            new _2016.Day09(), 
            new _2016.Day10(), 
            new _2016.Day11(), 
            new _2016.Day12(), 
            new _2016.Day13(), 
            new _2016.Day14(), 
            new _2016.Day15(), 
            new _2016.Day16(), 
            new _2016.Day17(), 
            // new _2016.Day18(), 
            // new _2016.Day19(), 
            // new _2016.Day20(), 
            // new _2016.Day21(), 
            // new _2016.Day22(), 
            // new _2016.Day23(), 
            // new _2016.Day24(), 
            // new _2016.Day25()
        ];
        
        foreach (var solution in _2016solutions)
        {
            RunSolution(solution);
        }
    }

    /// <summary>
    /// Runs the solution for every Advent of Code event
    /// </summary>
    static void RunAllSolutions()
    {
        Run2015Solutions();
        Run2016Solutions();
    }
}

public interface ISolution {
    /// <summary>
    /// Runs all parts of the solution for a day of the Advent of Code event. Note that some solutions may print text to the <see cref="Console"/> separate from the solution itself
    /// </summary>
    /// <returns>A string representation of the solution</returns>
    public string Answer();
}

public static class Extensions
{
    /// <summary>
    /// Swaps the values in the input <see cref="IList{T}"/> at the indicated indices
    /// </summary>
    /// <typeparam name="T">The type of the input <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The input <see cref="IList{T}"/>.</param>
    /// <param name="indexA">The first index.</param>
    /// <param name="indexB">The second index.</param>
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }

    /// <summary>
    /// Shuffles all of the values in the input <see cref="IList{T}"/> using the <see href="http://en.wikipedia.org/wiki/Fisher-Yates_shuffle">Fisher-Yates Shuffle Algorithm</see>
    /// </summary>
    /// <typeparam name="T">The type of the input <see cref="IList{T}"/>.</typeparam>
    /// <param name="list">The input <see cref="IList{T}"/>.</param>
    /// <param name="random">An instance object of <see cref="Random"/></param>
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    /// <summary>
    /// Computes the modulus of <see cref="this"/> by <see cref="n"/>. 
    /// </summary>
    /// <remarks>
    /// There is a difference between modulus (Euclidean division) and remainder (C#'s <c>%</c> operator): 
    /// <code>
    /// -21 mod 4 = 3 because -21 + (4 x 6) = 3.
    /// But -21 % 4 = -1 because -21 / 4 = -5 (remainder -1).
    /// </code>
    /// </remarks>
    /// <param name="a">The dividend (also <see cref="this"/>).</param>
    /// <param name="n">The divisor.</param>
    /// <returns>The modulus of <see cref="this"/> by a specified number.</returns>
    public static int Mod(this int a, int n) 
    {
        return ((a %= n) < 0) ? a + n : a;  
    }

    /// <summary>
    /// Shifts right the values in the input <typeparamref name="T"/> array instance by the specified number of indicies, wrapping back to the left side of the array.
    /// </summary>
    /// <typeparam name="T">The type of the input array.</typeparam>
    /// <param name="array">The input <typeparamref name="T"/> array.</param>
    /// <param name="shift">The amount to shift right by.</param>
    /// <returns>A <typeparamref name="T"/> array instance with the applied right shift</returns>
    public static T[] RotateArrayRight<T>(this T[] array, int shift)
    {
        int n = array.Length;
        T[] rotated = new T[n];

        for (int i = 0; i < n; i++)
        {
            int newIndex = (i + shift).Mod(n);
            rotated[newIndex] = array[i];
        }

        return rotated;
    }

    public static (int, int) Add(this (int, int) tuple1, (int, int) tuple2)
    {
        return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
    }

}

