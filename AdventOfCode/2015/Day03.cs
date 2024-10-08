namespace AdventOfCode._2015;

public class Day03 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2015", "Day03-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);

    private static (int, int) GetDeliveredHouses()
    {
        // part 1
        var santaCoord1 = (x: 0, y:0);
        var houses1 = new Dictionary<(int x, int y), int>
        {
            { (0, 0), 1}
        };

        // part 2
        var santaCoord2 = (x: 0, y:0);
        var robotCoord2 = (x: 0, y:0);
        int count = 0;
        var houses2 = new Dictionary<(int x, int y), int>
        {
            { (0, 0), 1}
        };

        foreach (char c in inputText)
        {
            // part 1
            santaCoord1 = GetNewCoord(santaCoord1, c);

            if (houses1.TryGetValue(santaCoord1, out int values1))
            {
                houses1[santaCoord1]++;
            }
            else
            {
                houses1[santaCoord1] = 1;
            }

            // part 2
            ref (int x, int y) coord = ref santaCoord2;

            if (count++ % 2 == 1)
                coord = ref robotCoord2;

            coord = GetNewCoord(coord, c);

            if (houses2.TryGetValue(coord, out int values2))
            {
                houses2[coord]++;
            }
            else
            {
                houses2[coord] = 1;
            }
        }

        return (houses1.Count, houses2.Count);
    }

    private static (int x, int y) GetNewCoord((int x, int y) coord, char c)
    {
        return c switch
        {
            '^' => (coord.x, coord.y+1),
            'v' => (coord.x, coord.y-1),
            '>' => (coord.x+1, coord.y),
            '<' => (coord.x-1, coord.y),
            _ => (coord.x, coord.y)
        };
    }

    public string Answer()
    {
        // part 1, part 2
        (int houses1, int houses2) = GetDeliveredHouses();
        
        return $"Santa visits {houses1} unique houses; Santa + Robo-Santa visit {houses2} unique houses";
    }
}
