namespace AdventOfCode._2015;

public class Day01 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day01-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static (int floor, int basementIndex) GetFloor()
    {
        int floor = 0;
        int basementIndex = -1;
        int count = 0;

        foreach (char c in inputText)
        {
            // part 1
            floor = c switch
            {
                '(' => floor + 1,
                ')' => floor - 1,
                _ => floor
            };

            // part 2
            count++;
            if (basementIndex < 0 && floor < 0)
                basementIndex = count;
        }

        return (floor, basementIndex);
    }

    public string Answer()
    {
        // part 1, part 2
        (int floor, int basementIndex) = GetFloor();
        
        return $"the final floor = {floor}; and the index when the basement is first entered = {basementIndex}";
    }
}
