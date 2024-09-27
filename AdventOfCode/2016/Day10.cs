using System.Security.Cryptography;
using Microsoft.CodeAnalysis;

namespace AdventOfCode._2016;

public class Day10 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day10-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly Dictionary<string, Bot> bots = InitBots();

    private class Bot
    {
        public string? LowSend { get; set; }
        public string? HighSend { get; set; }
        public (int Low, int High) Values { get; private set; } = (0, 0);

        public void Add(int n)
        {
            if (Values.Low > 0)
            {
                throw new Exception($"Bot already contains 2 values: ({Values.Low}, {Values.High})");
            }
            var oldValue = Values.High;
            if (oldValue < n)
            {
                Values = (oldValue, n);
            }
            else
            {
                Values = (n, oldValue);
            }
        }

        public bool Contains(int a, int b)
        {
            int low = a < b ? a : b;
            int high = a > b ? a : b;

            return Values.Low == low && Values.High == high;
        }
    }

    private class Output
    {
        public int Value { get; private set; } = 0;

        public void Add(int n)
        {
            if (Value > 0)
            {
                throw new Exception($"Output already contains a value: ({Value})");
            }
            Value = n;
        }
    }

    private static Dictionary<string, Bot> InitBots()
    {
        Dictionary<string, Bot> bots = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');
            string name;
            switch (tokens[0])
            {
                case "bot":
                    name = string.Join(' ', tokens[0], tokens[1]);
                    string low = string.Join(' ', tokens[5], tokens[6]);
                    string high = string.Join(' ', tokens[10], tokens[11]);
                    if (bots.ContainsKey(name) is false)
                    {
                        bots.Add(name, new Bot());
                    }
                    bots[name].HighSend = high;
                    bots[name].LowSend = low;
                    break;
                case "value":
                    name = string.Join(' ', tokens[4], tokens[5]);
                    int newValue = int.Parse(tokens[1]);
                    if (bots.ContainsKey(name) is false)
                    {
                        bots.Add(name, new Bot());
                    }
                    bots[name].Add(newValue);
                    break;
                default:
                    throw new Exception($"{tokens[0]} is not a supported instruction");
            };
        }

        return bots;
    }

    private static (string? botHoldingValues, int outputsProduct) FindBotByValues((int a, int b) values, string[] multiplyOutputs)
    {
        Queue<string> readyBots = [];
        Dictionary<string, int> outputs = [];
        string? holdingInputs = null;
        do
        {
            if (readyBots.Count > 0)
            {
                while (readyBots.Count > 0)
                {
                    var name = readyBots.Dequeue();
                    var bot = bots[name];
                    var (low, high) = bot.Values;
                    var lowSend = bot.LowSend!;
                    if (lowSend.StartsWith("bot")) 
                    {
                        bots[lowSend].Add(low);
                    }
                    else 
                    {
                        outputs.Add(lowSend, low);
                    }
                    var highSend = bot.HighSend!;
                    if (highSend.StartsWith("bot")) 
                    {
                        bots[highSend].Add(high);
                    }
                    else 
                    {
                        outputs.Add(highSend, high);
                    }
                    bots.Remove(name);
                }
            }
            foreach (var bot in bots)
            {
                var (High, Low) = bot.Value.Values;
                if (Low > 0 && High > 0)
                {
                    if (bot.Value.Contains(values.a, values.b))
                    {
                        holdingInputs = bot.Key;
                    }
                    readyBots.Enqueue(bot.Key);
                }
            }
        }
        while (readyBots.Count > 0);

        int product = 1;
        foreach (string output in multiplyOutputs)
        {
            product *= outputs[output];
        }

        return (holdingInputs, product);
    }

    public string Answer()
    {
        (int a, int b) values = (61, 17);
        string[] outputs = ["output 0", "output 1", "output 2"];

        var (botHoldingValues, outputsProduct) = FindBotByValues(values, outputs);

        return $"the bot that holds/sorts values {values.a} and {values.b} is {botHoldingValues}; the values of one chip in each of outputs 0, 1, and 2 multiplied together = {outputsProduct}";
    }
}