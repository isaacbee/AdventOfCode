using MathNet.Numerics;

namespace AdventOfCode._2016;

public class Day23 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day23-input.txt");
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
            string[] args = split[1].Split(" ");
            newProgram.Add((instruction, args));
        }
        return newProgram;
    }

    /// <summary>
    /// Same as <see cref="Day12"/>, except there is a new <c>tgl x</c> instruction that toggles the instruction <c>x</c> away.
    /// </summary>
    /// <remarks>one-argument instructions, <c>inc</c> becomes <c>dec</c>, and all other one-argument instructions become <c>inc</c>. For two-argument instructions, <c>jnz</c> becomes <c>cpy</c>, and all other two-instructions become <c>jnz</c>. The arguments of a toggled instruction are not affected. If an attempt is made to toggle an instruction outside the program, nothing happens. If toggling produces an invalid instruction (like <c>cpy 1 2</c>) and an attempt is later made to execute that instruction, skip it instead. If <c>tgl</c> toggles itself (for example, if <c>a</c> is 0, <c>tgl a</c> would target itself and become <c>inc a</c>), the resulting instruction is not executed until the next time it is reached.
    /// </remarks>
    /// <returns>The value of the registers (a, b, c, d) as a tuple.</returns>
    private static (int a, int b, int c, int d) RunKeypadCode(int a = 0, int b = 0, int c = 0, int d = 0, int start = 0)
    {
        List<(string instruction, string[] args)> modifiedProgram = [.. program];
        Dictionary<string, int> regs = new()
        {
            ["a"] = a,
            ["b"] = b,
            ["c"] = c,
            ["d"] = d
        };

        for (int i = start; i < modifiedProgram.Count; i++)
        {
            (string instruction, string[] args) = modifiedProgram[i];
            int result;

            switch (instruction)
            {
                case "cpy":
                    if (!int.TryParse(args[1], out _))
                    {
                        if (!int.TryParse(args[0], out result))
                        {
                            result = regs[args[0]];
                        }
                        regs[args[1]] = result;
                    }
                    break;
                case "inc":
                    regs[args[0]] += 1; 
                    break;
                case "dec":
                    regs[args[0]] -= 1;
                    break;
                case "jnz":
                    if (!int.TryParse(args[0], out result)) 
                    {
                        result = regs[args[0]];
                    }
                    if (result != 0) 
                    {
                        if (!int.TryParse(args[1], out result))
                        {
                            result = regs[args[1]];
                        }
                        i += result - 1;
                    }
                    break;
                case "tgl":
                    if (!int.TryParse(args[0], out result))
                    {
                        result = regs[args[0]];
                    }
                    if (i + result < modifiedProgram.Count && i + result >= 0)
                    {
                        modifiedProgram[i + result] = (modifiedProgram[i + result].instruction switch
                        {
                            "cpy" => "jnz",
                            "inc" => "dec",
                            "dec" => "inc",
                            "jnz" => "cpy",
                            "tgl" => "inc",
                            _ => throw new Exception("Instruction not supported."),
                        }, modifiedProgram[i + result].args);
                    }
                    break;
                default:
                    throw new Exception("Instruction not supported.");
            }
        }
        return (regs["a"], regs["b"], regs["c"], regs["d"]);
    }

    private static (string instruction, string[] args) ToggleInstuction((string instruction, string[] args) line)
    {
        return (line.instruction switch
            {
                "cpy" => "jnz",
                "inc" => "dec",
                "dec" => "inc",
                "jnz" => "cpy",
                "tgl" => "inc",
                _ => throw new Exception("Instruction not supported."),
            }, line.args);
    }

    public string Answer()
    {
        // part 1
        int eggs1 = 7;
        (int a1, _, _, _) = RunKeypadCode(eggs1, 0, 0, 0);

        // part 2
        int eggs2 = 12;
        // Warning: Slow
        // (int a2, _, _, _) = RunKeypadCode(eggs2, 0, 0, 0);
        
        // Sped up version by skipping the slow repeated addition. This is done by following the following steps: 1) b = a - 1; 2) a = a * b; 3) b--; 4) go back to (2) until b == 0; 5) 'tgl' every even instruction after index 16; 6) run the instuctions starting with index 17 until the end with the new register values.
        // a = 12! (yes, that is 12 factorial), b = 0, c = 0, d = 0
        for (int i = 18; i < program.Count; i+=2)
        {
            program[i] = ToggleInstuction(program[i]);
        }
        (int a2, _, _, _) = RunKeypadCode(Convert.ToInt32(SpecialFunctions.Factorial(eggs2)), 0, 0, 0, 17);

        return $"the value that should be sent to the safe if there are {eggs1} eggs = {a1}; and the value that should be sent to the safe when there are actually {eggs2} eggs = {a2}";
    }
}