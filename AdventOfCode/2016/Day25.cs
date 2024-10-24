namespace AdventOfCode._2016;

public class Day25 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day25-input.txt");
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
            string[] args = split[1].Split(' ');
            newProgram.Add((instruction, args));
        }
        return newProgram;
    }

    /// <summary>
    /// Runs the assembunny code for generating a signal. If the signal is a repeating clock signal, return the first <c>a</c> register that produced it.
    /// </summary>
    /// <returns>The lowest positive integer that produces a clock signal.</returns>
    private static int FindClockInitReg()
    {
        // set a maximum value to prevent integer overflow
        int maxReg = int.MaxValue / 2;

        for (int i = 1; i <= maxReg; i++)
        {
            if (RunClockCode(i, 0, 0, 0))
            {
                return i;
            }
        }
        throw new Exception($"No register creates a clock signal from 1 to {maxReg}");
    }

    /// <summary>
    /// Same as <see cref="Day23"/> (and <see cref="Day12"/>), except there is no "tgl" and instead has the new <c>out x</c> instruction that can transmit <c>x</c> (either an integer or the value of a register) as the next value for the clock signal.
    /// </summary>
    /// <returns><c>true</c> if the clock signal is 0, 1, 0, 1... repeating forever; otherwise, <c>false</c></returns>
    private static bool RunClockCode(int a = 0, int b = 0, int c = 0, int d = 0, int start = 0)
    {
        List<(string instruction, string[] args)> modifiedProgram = [.. program];
        Dictionary<string, int> regs = new()
        {
            ["a"] = a,
            ["b"] = b,
            ["c"] = c,
            ["d"] = d
        };

        int previousOut = 1;
        int clockCycles = 0;

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
                case "out":
                    if (!int.TryParse(args[0], out result))
                    {
                        result = regs[args[0]];
                    }
                    if (result == (previousOut ^ 1))
                    {
                        previousOut = result;
                        clockCycles++;
                    }
                    else 
                    {
                        return false;
                    }
                    break;
                default:
                    throw new Exception("Instruction not supported.");
            }

            // wait an arbitrary amount of time to wait for a stable clock
            if (clockCycles > 100)
            {
                return true;
            }
        }
        return false;
    }

    public string Answer()
    {
        // part 1
        int a = FindClockInitReg();

        // part 2 = finish all other parts in AdventOfCode._2016

        return $"the lowest positive integer that can be used to initialize register a and cause the code to output a clock signal of 0, 1, 0, 1... repeating forever = {a}";
    }
}