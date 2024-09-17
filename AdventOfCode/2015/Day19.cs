using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public partial class Day19 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day19-input.txt"; //$"lib\\2015\\Day19-input2.txt" is an alternate set of inputs with a less optimal solution
    private static readonly string inputText = File.ReadAllText(filePath);
    private static (Dictionary<string, List<string>> replacements, string medicine, List<(string key, string value)> replacementsOrdered) input = InitMedicine();
    private static Dictionary<(string, string, int), IEnumerable<string>>? cache;

    private static (Dictionary<string, List<string>> replacements, string medicine, List<(string, string)> replacementsSorted) InitMedicine()
    {
        Dictionary<string, List<string>> replacements = [];
        List<(string k, string v)> replacementsSorted = [];
        string[] lines = NormalizeInput(inputText).Split(Environment.NewLine);

        for (int i = 0; i < lines.Length-2; i++) 
        {
            string[] tokens = lines[i].Split(" => ");
            _ = replacements.TryAdd(tokens[0], []);
            replacements[tokens[0]].Add(tokens[1]);
        }

        int maxLength = 0;

        foreach ((string k, List<string> list) in replacements)
        {
            foreach (string v in list)
            {
                if (v.Length > maxLength)
                {
                    replacementsSorted.Add((k, v));
                }
            }
        }
        replacementsSorted.Sort((x,y) => y.v.Length.CompareTo(x.v.Length));

        return (replacements, lines.Last(), replacementsSorted);
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
        cache = [];
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
        if (cache!.TryGetValue((start, end, depth), out IEnumerable<string>? returnValue))
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

    static void Shuffle<T>(List<T> list, Random random)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            // Swap elements at index i and j
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    // Entry point of the greedy-random ordered search for the medicine molecule given a starting molecule. First applies a greedy search for the solution given an optimal ordering of {input.replacementsOrdered}. Then shuffles {input.replacementsOrdered} and tries again. Assumes that the first found solution is also the best solution. The depth is equal to the number of total replacements. 
    private static (int depth, int loop) CalculateMedicineMolecule(string start, string medicine)
    {
        cache = [];
        Random r = new();
        for (int attempt = 0; ; attempt++)
        {
            (var isFound, int depth, string smallestTerminal) = GenerateReduction(start, medicine, 1, 0);
            if (isFound is true)
            {
                return (depth, attempt);
            }
            Shuffle(input.replacementsOrdered, r);
        }
    }

    // Shrinks a molecule chain by iteratively applying the next {branches} number of valid reductions in the ordered list of reductions until no reductions can be made
    private static (bool isFound, int depth, string smallestTerminal) GenerateReduction(string start, string end, int branches, int depth)
    {
        int maxDepth = depth;
        string smallestTerminal = end;

        if (start == end)
        {
            return (true, depth, end);
        } 
        else if (branches == 0)
        { 
            return (false, depth, end);
        }
        else
        {
            int branchesRemaining = branches;
            HashSet<string> newReductions = [];
            foreach (var (key, value) in input.replacementsOrdered)
            {
                Regex rx = new(value);
                MatchCollection mc = rx.Matches(end);
                
                foreach (Match m in mc)
                {
                    if (branchesRemaining > 0)
                    {
                        string reduction = string.Concat(end.AsSpan(0, m.Index), key, end.AsSpan(m.Index + m.Length));
                        newReductions.Add(reduction);

                        var nextReduction = GenerateReduction(start, reduction, branchesRemaining, depth + 1);
                        if (nextReduction.depth > maxDepth)
                        {
                            maxDepth = nextReduction.depth;
                        }
                        if (nextReduction.smallestTerminal.Length < smallestTerminal.Length)
                        {
                            smallestTerminal = nextReduction.smallestTerminal;
                        }
                        if (nextReduction.isFound is true)
                        {
                            return (true, maxDepth, smallestTerminal);
                        }
                        branchesRemaining--;
                    }
                }
            }
        }
        return (false, maxDepth, smallestTerminal);
    }

    public string Answer()
    {
        // part 1
        (HashSet<string> distinctList1, _) = MoleculeBuild(input.medicine);

        // part 2 (my solution)
        (int depth, int attempt) = CalculateMedicineMolecule("e", input.medicine);
        return $"{distinctList1.Count} molecules can be made after one replacement of the medicine and \"e\" was turned into the medicine after {depth} steps (and {attempt} reordering of rules)";

        // // part 2 (adapted solution)
        // int steps = GenerateMedicineMolecule("e", input.medicine);
        // return $"{distinctList1.Count} molecules can be made after one replacement of the medicine and \"e\" was turned into the medicine after {steps} steps ({cache!.Count} unique molecules generated in the process)";
    }

    [GeneratedRegex(@"[A-Z][a-z]")]
    private static partial Regex MoleculeRegex();
}
