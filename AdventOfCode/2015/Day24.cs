using System;

namespace AdventOfCode._2015;

public class Day24 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day24-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<int> allPackages = InitPackages();

    private static List<int> InitPackages()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        List<int> packages = [];

        foreach (string line in lines)
        {
            packages.Add(int.Parse(line));
        }

        return packages;
    }

    private static long SolveBestSleighBalance(int groups)
    {
        long bestQE = long.MaxValue;
        int fullWeight = 0;

        foreach (int weight in allPackages)
        {
            fullWeight += weight;
        }
        int balancedWeight = fullWeight / groups;

        List<List<int>> allCombos = [];
        FindCombos(allPackages, balancedWeight, [], 0, allCombos);

        int minLength = allCombos.Min(x => x.Count);
        List<List<int>> shortestCombos = allCombos.Where(x => x.Count == minLength).ToList();

        // not neccessary, as there are no sets of numbers summing to the balancedWeight that will make it impossible to fill the remaining groups (given that it is possible to make 3 balanced groups). but leaving here in case that assumption can't be made
        // shortestCombos.RemoveAll(x => !CanMakeRemainingGroups(x, allCombos, 2));

        foreach (List<int> combo in shortestCombos)
        {
            long qe = 1;
            foreach (int present in combo)
            {
                qe *= present;
            }
            if (qe < bestQE)
            {
                bestQE = qe;
            }
        }

        return bestQE;
    }

    private static bool CanMakeRemainingGroups(List<int> combo, List<List<int>> allCombos, int remainingGroups)
    {
        bool result = false;

        if (remainingGroups > 0)
        {
            List<List<int>> allOtherCombos = allCombos.Where(list => !list.Any(item => combo.Contains(item))).ToList();

            if (allOtherCombos.Count > 0 is not true)
            {
                return result;
            }
            else {
                result = true;
            }

            if (remainingGroups > 1)
            {
                foreach (List<int> nextCombo in allOtherCombos)
                {
                    if (CanMakeRemainingGroups(nextCombo, allOtherCombos, remainingGroups - 1) is true)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
        }

        return result;
    }

    private static void FindCombos(List<int> list, int target, List<int> current, int index, List<List<int>> results)
    {
        if (target == 0)
        {
            results.Add([.. current]);
            return;
        }

        for (int i = index; i < list.Count; i++)
        {
            int x = list[i];
            if (x > target) 
                continue;
            
            current.Add(x);
            FindCombos(list, target - x, current, i + 1, results);
            current.RemoveAt(current.Count - 1);
        }
    }

    public string Answer()
    {
        long qe1 = SolveBestSleighBalance(3);

        long qe2 = SolveBestSleighBalance(4);

        return $"the quantum entanglement of the smallest group of packages in 3 groups = {qe1}; and the quantum entanglement of the smallest group of packages in 4 groups = {qe2}";
    }
}
