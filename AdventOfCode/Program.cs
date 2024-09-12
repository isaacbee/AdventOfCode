namespace AdventOfCode;

public class Program
{
    static void Main(string[] args)
    {
        // // Run all solutions
        // RunAllSolutions();

        // Run individual solutions
        RunSolution(new _2015.Day18());
    }

    static void RunSolution(ISolution Solve)
    {
        try
        {
            Console.WriteLine($"The solution to {Solve} is {Solve.Answer()}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when running {Solve}: {e}");
        }

    }

    static void RunAllSolutions()
    {
        ISolution[] _2015solutions = [ 
            new _2015.Day1(), 
            new _2015.Day2(), 
            new _2015.Day3(), 
            new _2015.Day4(), 
            new _2015.Day5(), 
            new _2015.Day6(), 
            new _2015.Day7(), 
            new _2015.Day8(), 
            new _2015.Day9(), 
            new _2015.Day10(), 
            new _2015.Day11(), 
            new _2015.Day12(), 
            new _2015.Day13(), 
            new _2015.Day14(), 
            new _2015.Day15(), 
            new _2015.Day16(), 
            new _2015.Day17(), 
            new _2015.Day18(), 
            // new _2015.Day19(), 
            // new _2015.Day20(), 
            // new _2015.Day21(), 
            // new _2015.Day22(), 
            // new _2015.Day23(), 
            // new _2015.Day24(), 
            // new _2015.Day25()
        ];
        
        foreach (var solution in _2015solutions)
        {
            RunSolution(solution);
        }
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
}