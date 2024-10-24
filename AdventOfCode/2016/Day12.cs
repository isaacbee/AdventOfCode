namespace AdventOfCode._2016;

public class Day12 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day12-input.txt");
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
    /// Runs the assembunny code from the boot sequence for the monorail control systems.
    /// </summary>
    /// <remarks>
    /// The boot sequence makes use of only a few instructions: 1) <c>cpy x y</c> copies <c>x</c> (either an integer or the value of a register) into register <c>y</c>. 2) <c>inc x</c> increases the value of register <c>x</c> by one. 3) <c>dec x</c> decreases the value of register <c>x</c> by one. 4) <c>jnz x y</c> jumps to an instruction <c>y</c> away (positive means forward; negative means backward), but only if <c>x</c> is not zero.
    /// </remarks>
    /// <returns>The value of the registers (a, b, c, d) as a tuple.</returns>
    private static (int a, int b, int c, int d) RunBootSequence(int a = 0, int b = 0, int c = 0, int d = 0)
    {
        Dictionary<string, int> regs = new()
        {
            ["a"] = a,
            ["b"] = b,
            ["c"] = c,
            ["d"] = d
        };

        for (int i = 0; i < program.Count; i++)
        {
            (string instruction, string[] args) = program[i];

            switch (instruction)
            {
                case "cpy":
                    if (int.TryParse(args[0], out int result))
                        regs[args[1]] = result;
                    else
                        regs[args[1]] = regs[args[0]];
                    break;
                case "inc":
                    regs[args[0]] += 1; 
                    break;
                case "dec":
                    regs[args[0]] -= 1;
                    break;
                case "jnz":
                    if (int.TryParse(args[0], out result)) {}
                    else result = regs[args[0]];
                    if (result != 0) 
                        i += int.Parse(args[1]) - 1;
                    break;
                default:
                    throw new Exception("Instruction not supported");
            }
        }

        return (regs["a"], regs["b"], regs["c"], regs["d"]);
    }

    public string Answer()
    {
        // part 1
        (int a1, int b1, int c1, int d1) = RunBootSequence(0, 0, 0, 0);

        // part 2
        (int a2, int b2, int c2, int d2) = RunBootSequence(0, 0, 1, 0);

        return $"after executing the assembunny code, the value in a = {a1}; and if c is initialized to be 1 instead, the value in a = {a2}";
    }
}
