namespace AdventOfCode._2015;

public class Day02 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2015", "Day02-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);

    private static (int paper, int ribbon) GetWrappingPaperAndRibbon()
    {
        int paper = 0;
        int ribbon = 0;

        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] values = line.Split('x');
    
            int l = int.Parse(values[0]);
            int w = int.Parse(values[1]);
            int h = int.Parse(values[2]);

            // part 1
            List<int> areas = CalculateAreas(l, w, h);

            paper += CalculateSmallestArea(areas);

            foreach (int area in areas)
            {
                paper += area*2;
            }

            // part 2
            int shortestPerimeter = CalculateShortestPerimeter(l, w, h);
            ribbon += shortestPerimeter + (l*w*h);
        }

        return (paper, ribbon);
    }

    private static List<int> CalculateAreas(params int[] values)
    {
        List<int> areas = [];
        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                areas.Add(values[i] * values[j]);
            }
        }
        return areas;
    }

    private static int CalculateShortestPerimeter(params int[] values)
    {
        int perimeter = -1;
        for (int i = 0; i < values.Length; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                int p = (values[i] * 2) + (values[j] * 2);
                if (perimeter < 0 || p < perimeter)
                    perimeter = p;
            }
        }
        return perimeter;
    }

    private static int CalculateSmallestArea(List<int> areas)
    {
        int smallest = areas[0];
        foreach (int area in areas) 
        {
            if (area < smallest)
                smallest = area;
        }
        
        return smallest;
    }

    public string Answer()
    {
        // part 1, part 2
        (int paper, int ribbon) = GetWrappingPaperAndRibbon();

        return $"{paper} ft² of wrapping paper; and {ribbon} ft of ribbon";
    }
}
