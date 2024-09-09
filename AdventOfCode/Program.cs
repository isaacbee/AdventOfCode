namespace AdventOfCode;

public class Program
{
    static void Main(string[] args)
    {
        RunAllSolutions();
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
            new _2015.Day9()
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