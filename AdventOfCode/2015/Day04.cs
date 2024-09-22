using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

public class Day04 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day04-input.txt";
    private static readonly string inputText = "ckczppom";

    private static string GetLowestNZeroHash(int nZeroes)
    {
        int value = 0;
        byte[] hashBytes = [];
        string zeroes = new('0', nZeroes);

        while (value < int.MaxValue) {
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{++value}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..5].Equals(zeroes)) 
            {
                return $"the value {value} produces {nZeroes} leading zeroes in the hash {result[..10]} ";
            }
        }

        return $"no integer value was found that produces {nZeroes} leading zeroes in the hash ";
    }

    private static string GetLowestNZeroHashLong(int nZeroes, long start = 0)
    {
        long value = start;
        byte[] hashBytes;
        string zeroes = new('0', nZeroes);

        while (value < long.MaxValue) {
            value++;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{value}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..nZeroes].Equals(zeroes)) 
            {
                return $"the value {value} produces {nZeroes} leading zeroes in the hash {result[..16]} ";
            }
        }

        return $"no long value was found that produces {nZeroes} leading zeroes in the hash ";
    }

    public string Answer()
    {
        string answer = string.Empty;

        // part 1
        answer += $"{GetLowestNZeroHash(5)}";

        // part 2
        answer += $"{GetLowestNZeroHashLong(6, 0L)}";


        return answer;
    }

}
