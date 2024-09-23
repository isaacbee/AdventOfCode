using System;

namespace AdventOfCode._2016;

public class Day04 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day04-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(string name, int id, string checksum)> roomList = InitRooms();

    private static List<(string name, int id, string checksum)> InitRooms()
    {
        List<(string name, int id, string checksum)> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            int index = line.LastIndexOf('-');
            string name = line[..index];
            int id = int.Parse(line[(index + 1)..^7]);
            string checksum = line[index..][^6..^1];

            list.Add((name, id, checksum));
        }

        return list;
    }

    private static int RealRoomSum(bool isPart2 = false)
    {
        int sum = 0;
        List<(string name, int id)> realRoomList = [];

        foreach ((string name, int id, string checksum) in roomList)
        {
            var letterFrequency = name.ToLower()
                                      .Where(char.IsLetter)
                                      .GroupBy(c => c)
                                      .ToDictionary(g => g.Key, g => g.Count());

            var top5letters = string.Concat
            (
                letterFrequency.OrderByDescending(kv => kv.Value)
                               .ThenBy(kv => kv.Key)
                               .Take(5)
                               .ToDictionary()
                               .Keys
            );

            if (top5letters == checksum)
            {
                realRoomList.Add((name, id));
                sum += id;
            }
        }

        if (isPart2)
        {
            foreach ((string name, int id) in realRoomList)
            {
                string shift = ShiftCipher(name, id);
                if (shift.Contains("north"))
                {
                    return id;
                }
            }
        }

        return sum;
    }

    private static string ShiftCipher(string input, int shift)
    {
        char[] result = new char[input.Length];
        shift %= 26;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (char.IsLetter(c))
            {
                char baseChar = char.IsUpper(c) ? 'A' : 'a';

                result[i] = (char)(((c - baseChar + shift) % 26) + baseChar);
            }
            else
            {
                result[i] = ' ';
            }
        }

        return string.Concat(result);
    }

    public string Answer()
    {
        int sum = RealRoomSum();

        int room = RealRoomSum(true);

        return $"the sum of real room sector IDs = {sum}; and the sector ID of the room where North Pole objects are stored = {room}";
    }
}
