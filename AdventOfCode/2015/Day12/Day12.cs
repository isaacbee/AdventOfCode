using System;
using System.Text.Json.Nodes;

namespace AdventOfCode._2015;

public partial class Day12 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day12\\input.json";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static JsonNode? Init()
    {
        return JsonNode.Parse(inputText);
    }

    private static int SumAllIntegers(JsonNode? node, bool ignoreRed = false)
    {
        int sum = 0;

        if (node is JsonObject jsonObject)
        {
            foreach (var property in jsonObject)
            {
                if (ignoreRed && property.Value is JsonValue jsonValue && jsonValue.TryGetValue(out string? stringValue) && stringValue == "red")
                {
                    return 0;
                }

                sum += SumAllIntegers(property.Value!, ignoreRed);
            }
        }
        else if (node is JsonArray jsonArray)
        {
            foreach (var element in jsonArray)
            {
                sum += SumAllIntegers(element!, ignoreRed);
            }
        }
        else if (node is JsonValue jsonValue)
        {
            if (jsonValue.TryGetValue(out int intValue))
            {
                sum += intValue;
            }
        }

        return sum;
    }

    public string Answer()
    {
        JsonNode? root = Init();

        int part1 = SumAllIntegers(root);

        int part2 = SumAllIntegers(root, true);

        return $"the sum of all numbers in the document = {part1} and the sum of all non-red numbers in the document = {part2}";
    }

}
