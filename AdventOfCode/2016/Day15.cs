namespace AdventOfCode._2016;

public class Day15 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day15-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<Disc> discs = InitDiscs();

    private class Disc(int positions, int start)
    {
        public int MaxPositions { get; private set; } = positions;
        public int StartPosition { get; private set; } = start;
    }

    private static List<Disc> InitDiscs()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        List<Disc> discs = [];

        foreach (string line in lines)
        {
            string[] tokens = line.Split([' ', '.'], StringSplitOptions.RemoveEmptyEntries);
            int positions = int.Parse(tokens[3]);
            int start = int.Parse(tokens[11]);
            discs.Add(new Disc(positions, start));
        }

        return discs;
    }

    private static int GetCapsule(List<Disc> discs)
    {
        int time = 0;

        while(time < int.MaxValue)
        {
            bool isSuccess = true;

            for (int i = 0; i < discs.Count; i++)
            {
                int maxPosition = discs[i].MaxPositions;
                int start = discs[i].StartPosition;
                if ((start + time + i + 1) % maxPosition != 0)
                {
                    isSuccess = false;
                    break;
                }
            }

            if (isSuccess) return time;
            time++;
        }

        throw new Exception("Not possible to get a capsule before running out of time.");
    }

    public string Answer()
    {
        // part 1
        int time1 = GetCapsule(discs);

        // part 1
        discs.Add(new Disc(11, 0));
        int time2 = GetCapsule(discs);

        return $"the first time you can press the button to get a capsule = {time1}; and with the extra disc, the first time you can press the button to get another capsule = {time2}";
    }
}