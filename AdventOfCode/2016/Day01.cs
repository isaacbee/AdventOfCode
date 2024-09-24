namespace AdventOfCode._2016;

public class Day01 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day01-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(bool isRight, int blocks)> directions = InitDirections();

    private static List<(bool isRight, int blocks)> InitDirections()
    {
        string[] tokens = inputText.Split(", ");
        List<(bool isRight, int blocks)> newDirections = [];

        foreach (string token in tokens)
        {
            newDirections.Add((token.StartsWith('R'), int.Parse(token[1..])));
        }

        return newDirections;
    }

    private static int CalculateBlocksAway(bool isFirstCrossing = false)
    {
        DirectionWrapper d = new(Direction.North);
        (int x, int y) = (0, 0);
        HashSet<(int x, int y)> memo = [];

        foreach((bool isRight, int blocks) in directions)
        {
            d += isRight ? 1 : -1;
            (int dx, int dy) = d.ToCoord();

            for (int i = 0; i < blocks; i++)
            {
                x += dx;
                y += dy;

                if (isFirstCrossing)
                {
                    if (memo.Contains((x,y)))
                    {
                        return Math.Abs(x) + Math.Abs(y);
                    }
                    else
                    {
                        memo.Add((x,y));
                    }
                }
                
            }
        }

        return Math.Abs(x) + Math.Abs(y);
    }

    private enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    private class DirectionWrapper(Direction direction)
    {
        public Direction Direction { get; set; } = direction;

        public static DirectionWrapper operator +(DirectionWrapper direction, int steps)
        {
            int newDirection = (int)direction.Direction + steps;
            int mod = newDirection.Mod(4);
            return new DirectionWrapper((Direction)mod);
        }

        private static readonly (int x, int y)[] directionToCoord =
        [
            (0, 1),     // North
            (1, 0),     // East
            (0, -1),    // South
            (-1, 0)     // West
        ];

        public (int x, int y) ToCoord()
        {
            return directionToCoord[(int)Direction];
        }
    }

    public string Answer()
    {
        // part 1
        int blocks1 = CalculateBlocksAway();

        // part 2
        int blocks2 = CalculateBlocksAway(true);

        return $"the final destination is {blocks1} blocks away; and the first crossing is {blocks2} blocks away";
    }
}