using System;

namespace AdventOfCode._2015;

public class Day23 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day23-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(string instruction, string[] args)> program = InitProgram();

    private static List<(string, string[])> InitProgram()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        List<(string instruction, string[] args)> newProgram = [];

        foreach (string line in lines)
        {
            string[] split = line.Split(' ', 2);
            string instruction = split[0];
            string[] args = split[1].Split(", ");
            newProgram.Add((instruction, args));
        }

        return newProgram;
    }

    private static (int a, int b) RunExampleProgram(int a = 0, int b = 0)
    {
        Dictionary<string, int> regs = new()
        {
            ["a"] = a,
            ["b"] = b
        };

        for (int i = 0; i < program.Count; i++)
        {
            (string instruction, string[] args) = program[i];

            switch (instruction)
            {
                case "hlf":
                    regs[args[0]] /= 2; 
                    break;
                case "tpl":
                    regs[args[0]] *= 3;
                    break;
                case "inc":
                    regs[args[0]] += 1;
                    break;
                case "jmp":
                    i += int.Parse(args[0]) - 1;
                    break;
                case "jie":
                    if (regs[args[0]] % 2 == 0)
                    {
                        i += int.Parse(args[1]) - 1;
                    }
                    break;
                case "jio":
                    if (regs[args[0]] == 1)
                    {
                        i += int.Parse(args[1]) - 1;
                    }
                    break;
                default:
                    throw new Exception("Instruction not supported");
            }
        }

        return (regs["a"], regs["b"]);
    }

    public string Answer()
    {
        (int a1, int b1) = RunExampleProgram();

        (int a2, int b2) = RunExampleProgram(1);

        return $"the value of the register b after running the program: b = {b1}; the value of the register b after running the program when the register a = 1: b = {b2}";
    }
}
