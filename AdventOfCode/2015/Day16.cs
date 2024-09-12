using System;

namespace AdventOfCode._2015;

public class Day16 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day16-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly Dictionary<string, int> MFCSAM = new() {
        { "children", 3 },
        { "cats", 7 },
        { "samoyeds", 2 },
        { "pomeranians", 3 },
        { "akitas", 0 },
        { "vizslas", 0 },
        { "goldfish", 5 },
        { "trees", 3 },
        { "cars", 2 },
        { "perfumes", 1 }
    };

    private class AuntSue(int index, Dictionary<string, int> properties)
    {
        public int Index { get; init; } = index;
        public Dictionary<string, int> Properties { get; init; } = properties;
    }

    private static List<AuntSue> Init()
    {
        List<AuntSue> aunts = [];
        string[] lines = inputText.Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string[] parts = line.Split([": "], 2, StringSplitOptions.None);
            int index = int.Parse(parts[0].Split(' ')[1]);
            string[] keyValuePairs = parts[1].Split(',');

            Dictionary<string, int> properties = [];

            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Trim().Split(':');
                string key = keyValue[0].Trim();
                int value = int.Parse(keyValue[1].Trim());

                properties.Add(key, value);
            }

            aunts.Add(new AuntSue(index, properties));
        }
        return aunts;
    }

    private static int FindSue(List<AuntSue> sues, Dictionary<string, int> match, bool isOutdated = false)
    {
        List<AuntSue> sueMatches = [];

        foreach (AuntSue sue in sues)
        {
            bool isMatch = true;
            foreach ((string k, int v) in match)
            {
                if (sue.Properties.TryGetValue(k, out int value))
                {
                    if (isMatch is true)
                    {
                        if (isOutdated is false)
                        {
                            isMatch = v == value;
                        }
                        else
                        {
                            isMatch = k switch
                            {
                                "cats" or "trees" => v < value,
                                "pomeranians" or "goldfish" => v > value,
                                _ => v == value
                            };
                        }
                    }
                    
                }
            }

            if (isMatch)
                sueMatches.Add(sue);
        }
        
        return sueMatches[0].Index;
    }

    public string Answer()
    {
        List<AuntSue> sues = Init();

        // part 1
        int index1 = FindSue(sues, MFCSAM);

        // part 2
        int index2 = FindSue(sues, MFCSAM, true);

        return $"the matching Sue is Sue {index1} unless it has an outdated retroencabulator, in which case the matching Sue is Sue {index2}";
    }

}
