namespace AdventOfCode;

public class Program
{
    static void Main(string[] args)
    {
        RunSolutions();
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

    static void RunSolutions()
    {
        ISolution[] solutions = { new _2015.Day1(), new _2015.Day2(), new _2015.Day3(), new _2015.Day4(), new _2015.Day5() };
        foreach (var solution in solutions)
        {
            RunSolution(solution);
        }
    }

    
}

interface ISolution {
    public string Answer();
}