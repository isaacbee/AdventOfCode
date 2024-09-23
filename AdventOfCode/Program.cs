using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        RunSolution(new _2016.Day05());
    }

    static void RunSolution(ISolution Solve, bool showTime = true, bool isPrintPaused = false, bool isStopwatchReset = false)
    {
        if (isStopwatchReset is true) sw.Reset();
        sw.Start();

        string answer = Solve.Answer();

        if (isPrintPaused) sw.Stop();
        TimeSpan ts = sw.Elapsed;

        if (showTime) Console.Write($"[{string.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds)}] ");

        try
        {
            Console.WriteLine($"The solution to {Solve} is {answer}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when running {Solve}: {e}");
        }

        sw.Stop();
    }

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
            RunSolution(solution);
        }
    }

    static void Run2016Solutions()
    {
        ISolution[] _2016solutions = [ 
            new _2016.Day01(), 
            new _2016.Day02(), 
            new _2016.Day03(), 
            new _2016.Day04(), 
            new _2016.Day05(), 
            // new _2016.Day06(), 
            // new _2016.Day07(), 
            // new _2016.Day08(), 
            // new _2016.Day09(), 
            // new _2016.Day10(), 
            // new _2016.Day11(), 
            // new _2016.Day12(), 
            // new _2016.Day13(), 
            // new _2016.Day14(), 
            // new _2016.Day15(), 
            // new _2016.Day16(), 
            // new _2016.Day17(), 
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

    static void RunAllSolutions()
    {
        Run2015Solutions();
        Run2016Solutions();
    }
}

interface ISolution {
    public string Answer();
}

public static class Extensions
{
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }

    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    public static int Mod(this int k, int n) 
    {  
        return ((k %= n) < 0) ? k + n : k;  
    }

}

