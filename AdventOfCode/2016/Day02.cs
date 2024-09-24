namespace AdventOfCode._2016;

public class Day02 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day02-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly string[] instructions = inputText.Split(Environment.NewLine);

    private static string GetBathroomCode(char[,] numPad)
    {
        int codeLength = instructions.Length;
        char[] code = new char[codeLength];

        int x = 1;
        int y = 1;

        for (int i = 0; i < codeLength; i++)
        {
            foreach (char c in instructions[i])
            {
                (int dx, int dy) = DirectionToCoord(c);

                int ix = x + dx;
                int iy = y + dy;

                if (ix >= 0 && ix < numPad.GetLength(0) && iy >= 0 && iy < numPad.GetLength(0) && numPad[ix, iy] != '\0')
                {
                    (x, y) = (ix, iy);
                }

            }
            code[i] = numPad[x, y];
        }

        return string.Concat(code);
    }

    private static (int x, int y) DirectionToCoord(char direction)
    {
        (int x, int y)[] moveToCoord =
        [
            (0, 1),     // R
            (1, 0),     // D
            (0, -1),    // L
            (-1, 0)     // U
        ];

        return direction switch 
        {
            'R' => moveToCoord[0],
            'D' => moveToCoord[1],
            'L' => moveToCoord[2],
            'U' => moveToCoord[3],
            _ => (0, 0)
        };
    }

    public string Answer()
    {
        // part 1
        char[,] numPad1 = {
            {'1', '2', '3'},
            {'4', '5', '6'},
            {'7', '8', '9'}
        };
        string code1 = GetBathroomCode(numPad1);

        // part 2
        char[,] numPad2 = {
            {'\0', '\0', '1', '\0', '\0'},
            {'\0', '2' , '3', '4' , '\0'},
            {'5' , '6' , '7' , '8' , '9'},
            {'\0', 'A' , 'B' , 'C' , '\0'},
            {'\0', '\0', 'D' , '\0', '\0'}
        };
        string code2 = GetBathroomCode(numPad2);

        return $"the expected bathroom code = {code1}; and the actual bathroom code = {code2}";
    }
}
