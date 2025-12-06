namespace AdventOfCode._2025;

public class Day01 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2025", "Day01-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);

    private static readonly List<(bool isRight, int turn)> rotations = InitTurn();

    private static List<(bool isRight, int turn)> InitTurn()
    {
        string[] tokens = inputText.Split(Environment.NewLine);
        List<(bool isRight, int turn)> newTurn = [];

        foreach (string token in tokens)
        {
            newTurn.Add((token.StartsWith('R'), int.Parse(token[1..])));
        }

        return newTurn;
    }

    /// <summary>
    /// Counts the number of times a safe dial lands at 0 after every turn.
    /// </summary>
    private static int CountZeroPoints(int dialSize = 100, int startPosition = 50)
    {
        int countZero = 0;
        int currentPosition = startPosition;

        foreach ((bool isRight, int turn) in rotations)
        {
            currentPosition += (isRight ? 1 : -1) * turn;
            currentPosition = currentPosition.Mod(dialSize);
            if (currentPosition == 0) 
                countZero++;
        }
        return countZero;
    }

    /// <summary>
    /// Counts the number of times a safe dial "clicks" to 0 during every turn.
    /// </summary>
    private static int CountZeroClicks(int dialSize = 100, int startPosition = 50)
    {
        int countZero = 0;
        int currentPosition = startPosition;

        foreach ((bool isRight, int turn) in rotations)
        {
            int previousPosition = currentPosition;
            currentPosition += (isRight ? 1 : -1) * turn;
            
            countZero += Math.Abs(currentPosition) / 100;

            if (turn != 0 && currentPosition == 0)
                countZero++;

            if (previousPosition != 0 && currentPosition < 0)
                countZero++;

            currentPosition = currentPosition.Mod(dialSize);
        }
        return countZero;
    }

    public string Answer()
    {
        // part 1
        int password1 = CountZeroPoints();

        // part 1
        int password2 = CountZeroClicks();

        return $"The password to open the door is {password1} ; if the password method is 0x434C49434B, the password to open the door is {password2}";
    }
}