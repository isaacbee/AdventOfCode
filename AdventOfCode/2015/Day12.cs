using System.Text.Json.Nodes;

namespace AdventOfCode._2015;

public class Day12 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2015", "Day12-input.txt");
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

        // part 1
        int sum1 = SumAllIntegers(root);

        // part 2
        int sum2 = SumAllIntegers(root, true);

        return $"the sum of all numbers in the document = {sum1}; and the sum of all non-red numbers in the document = {sum2}";
    }

}
