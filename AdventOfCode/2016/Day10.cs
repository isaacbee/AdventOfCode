namespace AdventOfCode._2016;

public class Day10 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day10-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static readonly Dictionary<string, Bot> bots = InitBots();

    private class Bot
    {
        public string? Low { get; set; }
        public string? High { get; set; }
        public int[] Values { get; set; } = new int[2];
    }

    private static Dictionary<string, Bot> InitBots()
    {
        Dictionary<string, Bot> bots = [];

        

        return bots;
    }

    public string Answer()
    {
        return $"";
    }
}