namespace AdventOfCode._2016;

public class Day21 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day21-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static List<(string operation, string[] args)> instructions = InitInstructions();

    /// <summary>
    /// Convert the plain text input into a list of operations and their arguments.
    /// </summary>
    private static List<(string operation, string[] args)> InitInstructions()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        List<(string operation, string[] args)> instructions = [];
        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');
            switch (tokens[0])
            {
                case "swap":
                    if (tokens[1] == "position")
                    {
                        instructions.Add(("swapP", [tokens[2], tokens[5]]));
                    }
                    else
                    {
                        instructions.Add(("swapL", [tokens[2], tokens[5]]));
                    }
                    break;
                case "rotate":
                    if (tokens[1] == "left")
                    {
                        instructions.Add(("rotateL", [tokens[2]]));
                    }
                    else if (tokens[1] == "right")
                    {
                        instructions.Add(("rotateR", [tokens[2]]));
                    }
                    else
                    {
                        instructions.Add(("rotateP", [tokens[6]]));
                    }
                    break;
                case "reverse":
                    instructions.Add(("reverse", [tokens[2], tokens[4]]));
                    break;
                case "move":
                    instructions.Add(("move", [tokens[2], tokens[5]]));
                    break;
                default:
                    throw new Exception("Not a supported instruction.");
            }
        }
        return instructions;
    }

    /// <summary>
    /// The scrambling function is a series of operations (the exact list is provided in your puzzle input). Starting with the password to be scrambled, apply each operation in succession to the string.
    /// </summary>
    private static string ScramblePassword(string password)
    {
        foreach ((string operation, string[] args) in instructions)
        {
            password = operation switch
            {
                "swapP" => SwapPositions(password, int.Parse(args[0]), int.Parse(args[1])),
                "swapL" => SwapLetters(password, args[0].First(), args[1].First()),
                "rotateR" => RotateLettersRight(password, int.Parse(args[0])),
                "rotateL" => RotateLettersLeft(password, int.Parse(args[0])),
                "rotateP" => RotateLettersRightBased(password, args[0].First()),
                "reverse" => ReverseSpan(password, int.Parse(args[0]), int.Parse(args[1])),
                "move" => MovePosition(password, int.Parse(args[0]), int.Parse(args[1])),
                _ => throw new Exception("Not a supported instruction."),
            };
        }
        return password;
    }

    /// <summary>
    /// <c>swap position X with position Y</c> means that the letters at indexes X and Y (counting from 0) should be swapped.
    /// </summary>
    private static string SwapPositions(string str, int x, int y)
    {
        char[] charArray = str.ToCharArray();
        (charArray[x], charArray[y]) = (charArray[y], charArray[x]);
        return new string(charArray);
    }

    /// <summary>
    /// <c>swap letter X with letter Y</c> means that the letters X and Y should be swapped (regardless of where they appear in the string).
    /// </summary>
    private static string SwapLetters(string str, char x, char y)
    {
        char[] charArray = str.ToCharArray();
        int a = str.IndexOf(x);
        int b = str.IndexOf(y);
        (charArray[a], charArray[b]) = (charArray[b], charArray[a]);
        return new string(charArray);
    }

    /// <summary>
    /// <c>rotate right X steps</c> means that the whole string should be rotated; for example, one right rotation would turn abcd into dabc.
    /// </summary>
    private static string RotateLettersRight(string str, int x)
    {
        return str[^x..] + str[..^x];
    }

    /// <summary>
    /// <c>rotate left X steps</c> means that the whole string should be rotated; for example, one left rotation would turn abcd into bcda.
    /// </summary>
    private static string RotateLettersLeft(string str, int x)
    {
        return str[x..] + str[..x];
    }

    /// <summary>
    /// <c>rotate based on position of letter X</c> means that the whole string should be rotated to the right based on the index of letter X (counting from 0) as determined before this instruction does any rotations. Once the index is determined, rotate the string to the right one time, plus a number of times equal to that index, plus one additional time if the index was at least 4.
    /// </summary>
    private static string RotateLettersRightBased(string str, char x)
    {
        int a = str.IndexOf(x);
        if (a >= 4) a++;
        a++;
        a %= str.Length;
        return str[^a..] + str[..^a];
    }

    /// <summary>
    /// <c>reverse positions X through Y</c> means that the span of letters at indexes X through Y (including the letters at X and Y) should be reversed in order.
    /// </summary>
    private static string ReverseSpan(string str, int x, int y)
    {
        char[] charArray = str.ToCharArray();
        for (int i = 0; i < ((y - x + 1) / 2); i++)
        {
            (charArray[x + i], charArray[y - i]) = (charArray[y - i], charArray[x + i]);
        }
        return new string(charArray);
    }

    /// <summary>
    /// <c>move position X to position Y</c> means that the letter which is at index X should be removed from the string, then inserted such that it ends up at index Y.
    /// </summary>
    private static string MovePosition(string str, int x, int y)
    {
        var charList = str.ToList();
        char temp = charList[x];
        charList.RemoveAt(x);
        charList.Insert(y, temp);
        return new string([.. charList]);
    }

    /// <summary>
    /// Inverse of <see cref="ScramblePassword(string)"/>
    /// </summary>
    private static string UnscramblePassword(string scramble)
    {
        for (int i = instructions.Count - 1; i >= 0; i--)
        {
            (string operation, string[] args) = instructions[i];
            scramble = operation switch
            {
                "swapP" => SwapPositions(scramble, int.Parse(args[1]), int.Parse(args[0])),
                "swapL" => SwapLetters(scramble, args[1].First(), args[0].First()),
                "rotateR" => RotateLettersLeft(scramble, int.Parse(args[0])),
                "rotateL" => RotateLettersRight(scramble, int.Parse(args[0])),
                "rotateP" => InverseRotateLettersRightBased(scramble, args[0].First()),
                "reverse" => ReverseSpan(scramble, int.Parse(args[0]), int.Parse(args[1])),
                "move" => MovePosition(scramble, int.Parse(args[1]), int.Parse(args[0])),
                _ => throw new Exception("Not a supported instruction."),
            };
        }
        return scramble;
    }

    /// <summary>
    /// Inverse of <see cref="RotateLettersRightBased"/>
    /// </summary>
    /// <remarks>
    /// 12345678
    /// 1: 81234567 (index 1 means left shift 1),
    /// 2: 78123456 (index 3 means left shift 2),
    /// 3: 67812345 (index 5 means left shift 3),
    /// 4: 56781234 (index 7 means left shift 4),
    /// 5: 34567812 (index 2 means left shift 6),
    /// 6: 23456781 (index 4 means left shift 7),
    /// 7: 12345678 (index 6 means left shift 0),
    /// 8: 81234567 (index 0 means left shift 1)
    /// </remarks>
    private static string InverseRotateLettersRightBased(string str, char x)
    {
        int a = str.IndexOf(x);
        if (a % 2 == 1)
        {
            a = (a + 1) / 2;
        }
        else 
        {
            a = ((a-2).Mod(str.Length) / 2 + 6) % str.Length;
        }
        return str[a..] + str[..a];
    }

    public string Answer()
    {
        // part 1
        string password1 = "abcdefgh";
        string scramble1 = ScramblePassword(password1);

        // part 1
        string scramble2 = "fbgdceah";
        string password2 = UnscramblePassword(scramble2);

        return $"the result of scrambling {password1} = {scramble1}; the un-scrambled version of the scrambled password {scramble2} = {password2}";
    }
}