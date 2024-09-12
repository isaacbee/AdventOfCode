using System;
using System.Numerics;

namespace AdventOfCode._2015;

public class Day17 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day17-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static List<int> InitContainers()
    {
        List<int> containers = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            containers.Add(int.Parse(line));
        }

        return containers;
    }

    private static (int, int, int) CalculateContainerCombinations(List<int> containers, int targetSum)
    {
        int targetCombinations = 0;
        int allCombinations = 1 << containers.Count;
        int[] setOfCombinations = new int[containers.Count];
        int minNCombinations = 0;
        int minCombinations = 0;

        for (int i = 0; i < allCombinations; i++)
        {
            int setSum = 0;
            int activeBits = BitOperations.PopCount((uint) i);
            for (int j = 0; j < containers.Count; j++)
            {
                // if the jth bit is in i, add to sum
                if ((i & (1 << j)) is not 0)
                {
                    setSum += containers[j];
                }
            }
            if (setSum == targetSum)
            {
                // part 1
                targetCombinations++;

                // part 2
                setOfCombinations[activeBits]++;
            }
        }

        for (int i = 0; i < containers.Count; i++)
        {
            if (setOfCombinations[i] > 0)
            {
                minNCombinations = i;
                minCombinations = setOfCombinations[i];
                break;
            }
        }

        return (targetCombinations, minCombinations, minNCombinations);
    }

    public string Answer()
    {
        List<int> containers = InitContainers();
        int liters1 = 150;
        (int targetcombinations, int minCombinations, int minNCombinations) = CalculateContainerCombinations(containers, liters1);

        return $"the number of different combinations of containers that fits all {liters1} L = {targetcombinations}, and the combinations of minimum containers ({minNCombinations}) that fits all {liters1} L = {minCombinations}";
    }
}
