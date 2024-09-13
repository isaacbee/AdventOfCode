using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public partial class Day19 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day19-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static (Dictionary<string, List<string>> replacements, string medicine) input = InitPlant();
    private static Dictionary<(string, string, int), IEnumerable<string>> cache = [];

    private static (Dictionary<string, List<string>> replacements, string medicine) InitPlant()
    {
        Dictionary<string, List<string>> replacements = [];
        string[] lines = NormalizeInput(inputText).Split(Environment.NewLine);

        for (int i = 0; i < lines.Length-2; i++) 
        {
            string[] tokens = lines[i].Split(" => ");
            _ = replacements.TryAdd(tokens[0], []);
            replacements[tokens[0]].Add(tokens[1]);
        }

        return (replacements, lines.Last());
    }

    static string NormalizeInput(string text)
    {
        var regex = MoleculeRegex();
        var matches = regex.Matches(text);

        HashSet<string> distinctPairs = [];
        foreach (Match match in matches)
        {
            distinctPairs.Add(match.Value);
        }

        int i = 0;
        foreach (string pair in distinctPairs)
        {
            if (i > 9)
            {
                throw new InvalidOperationException("More than 10 two-character tokens");
            }
            text = text.Replace(pair, i.ToString());
            i++;
        }

        return text;
    }

    // Extends out a molecule chain by iteratively applying every possible replacement. Warning: Slow.
    private static (HashSet<string> uniqueMolecules, int steps) MoleculeBuild(string startMolecule, string endMolecule = "", bool isRunOnce = true, int maxSteps = 8)
    {
        HashSet<string> set = [];
        Queue<(string current, int steps)> queue = [];
        queue.Enqueue((startMolecule, 0));
        int endLength = endMolecule.Length;

        do
        {
            (string currMolecule, int steps) = queue.Dequeue();

            if (steps > maxSteps) break;

            foreach ((string from, List<string> replacementList) in input.replacements)
            {
                foreach (string to in replacementList) 
                {
                    for (int i = 0; i <= currMolecule.Length - from.Length; i++)
                    {
                        if (currMolecule.Substring(i, from.Length) == from)
                        {
                            string newMolecule = string.Concat(currMolecule.AsSpan(0, i), to, currMolecule.AsSpan(i + from.Length));

                            if (newMolecule == endMolecule)
                            {
                                return (set, steps);
                            }
                            else if ((newMolecule.Length <= endLength || isRunOnce is true) && set.Contains(newMolecule) is false)
                            {
                                set.Add(newMolecule);
                                queue.Enqueue((newMolecule, steps + 1));
                            }
                        }
                    }
                }
            }
        } while (queue.Count > 0 && isRunOnce is false);
        return (set, 1);
    }

    // Shrinks a molecule chain by iteratively applying every possible replacement in reverse. Warning: Slow.
    private static (HashSet<string> uniqueMolecules, int steps) MoleculeReduce(string startMolecule, string endMolecule, bool isRunOnce = true)
    {
        HashSet<string> set = [];
        Queue<(string current, int steps)> queue = [];
        queue.Enqueue((endMolecule, 0));

        do
        {
            (string currMolecule, int steps) = queue.Dequeue();

            foreach ((string to, List<string> replacementList) in input.replacements)
            {
                foreach (string from in replacementList) 
                {
                    for (int i = 0; i <= currMolecule.Length - from.Length; i++)
                    {
                        if (currMolecule.Substring(i, from.Length) == from)
                        {
                            string newMolecule = string.Concat(currMolecule.AsSpan(0, i), to, currMolecule.AsSpan(i + from.Length));

                            if (newMolecule == startMolecule)
                            {
                                return (set, steps);
                            }
                            else if (set.Contains(newMolecule) is false)
                            {
                                set.Add(newMolecule);
                                queue.Enqueue((newMolecule, steps + 1));
                            }
                        }
                    }
                }
            }
        } while (queue.Count > 0 && isRunOnce is false);
        return (set, 1);
    }

    // Entry point of the exhaustive search for the medicine molecule given a starting molecule. The depth is equal to the number of total replacements. Also works as a general solution for any end string and set of replacements as long as it is formatted to contain only single character replacements. Heavily inspired by the python solution here: https://github.com/HeWeMel/adventofcode/blob/main/2015/19/day19_part_b_isolated.py
    private static int GenerateMedicineMolecule(string start, string medicine)
    {
        for (int depth = 0; ; depth++)
        {
            foreach (var prefix in GeneratePossibleMolecules(start, medicine, depth))
            {
                if (prefix == medicine)
                {
                    return depth;
                }
            }
        }
    }

    // Performs an exhaustive search for every possible replacement up until the given depth, which is also the number of replacements. Performs heavy caching of partial results. Heavily inspired by the python solution here: https://github.com/HeWeMel/adventofcode/blob/main/2015/19/day19_part_b_isolated.py
    private static IEnumerable<string> GeneratePossibleMolecules(string start, string end, int depth) {
        // Check if result is cached
        if (cache.TryGetValue((start, end, depth), out IEnumerable<string>? returnValue))
        {
            return returnValue;
        }

        // If no steps are allowed, check if start is a prefix of goal
        if (depth == 0)
        {
            if (end.StartsWith(start))
            {
                return [start];
            }
            return [];
        }

        HashSet<string> prefixes = [];

        if (start.Length == 1)
        {
            // If start is one character, apply each rule and recurse
            if (input.replacements.TryGetValue(start, out List<string>? replacementList))
            {
                foreach (string to_c in replacementList)
                {
                    prefixes.UnionWith(GeneratePossibleMolecules(to_c, end, depth - 1));
                }
            }
        }
        else
        {
            string start_left_part = start[..1];
            string start_right_part = start[1..];

            // Try all possible depth splits between the left and right parts
            for (int depth_left = 0; depth_left <= depth; depth_left++)
            {
                int depth_right = depth - depth_left;

                // Get possible prefixes for the left part
                foreach (string prefix_left in GeneratePossibleMolecules(start_left_part, end, depth_left))
                {
                    // Get possible prefixes for the right part
                    foreach (string prefix_right in GeneratePossibleMolecules(start_right_part, end[prefix_left.Length..], depth_right))
                    {
                        // Combine the left and right prefixes
                        prefixes.Add(prefix_left + prefix_right);
                    }
                }
            }
        }

        // Cache the result and return
        cache[(start, end, depth)] = prefixes;
        return prefixes;
    }

    public string Answer()
    {
        // part 1
        (HashSet<string> distinctList1, _) = MoleculeBuild(input.medicine);

        // part 2
        int steps = GenerateMedicineMolecule("e", input.medicine);

        return $"{distinctList1.Count} molecules can be made after one replacement of the long molecule and \"e\" was turned into the long molecule after {steps} steps ({cache.Count} unique molecules generated in the process)";
    }

    [GeneratedRegex(@"[A-Z][a-z]")]
    private static partial Regex MoleculeRegex();
}
