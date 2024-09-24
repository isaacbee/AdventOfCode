namespace AdventOfCode._2016;

public class Day06 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day06-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<string> messages = InitMessages();

    private static List<string> InitMessages()
    {
        List<string> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            list.Add(line);
        }

        return list;
    }

    private static string ErrorCorrectMessage(bool isMostFrequent = true)
    {
        int length = messages[0].Length;
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            Dictionary<char, int> charCount = [];

            foreach (string s in messages)
            {
                char c = s[i];

                if (charCount.TryGetValue(c, out int value))
                {
                    charCount[c] = ++value;
                }
                else
                {
                    charCount[c] = 1;
                }
            }

            if (isMostFrequent)
            {
                result[i] = charCount.OrderByDescending(kv => kv.Value)
                                     .ThenBy(kv => kv.Key)
                                     .First().Key;
            }
            else
            {
                result[i] = charCount.OrderBy(kv => kv.Value)
                                     .ThenBy(kv => kv.Key)
                                     .First().Key;
            }
        }

        return string.Concat(result);
    }

    public string Answer()
    {
        // part 1
        string message1 = ErrorCorrectMessage();

        // part 2
        string message2 = ErrorCorrectMessage(false);

        return $"the error-corrected version of the message being sent = \"{message1}\"; the original message that Santa is trying to send = \"{message2}\"";
    }
}