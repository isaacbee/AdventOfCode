using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

public class Day04 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day04-input.txt";
    private static readonly string inputText = "ckczppom";

    private static (string hash, int index) GetLowestNZeroHash(int nZeroes, int start = 0)
    {
        int index = start;
        byte[] hashBytes;
        string zeroes = new('0', nZeroes);

        while (index < int.MaxValue) {
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{++index}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..5].Equals(zeroes)) 
            {
                return (result, index);
            }
        }

        throw new Exception($"No integer value was found that produces {nZeroes} leading zeroes in the hash");
    }

    private static (string hash, long index) GetLowestNZeroHashLong(int nZeroes, long start = 0L)
    {
        long index = start;
        byte[] hashBytes;
        string zeroes = new('0', nZeroes);

        while (index < long.MaxValue) {
            index++;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{index}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..nZeroes].Equals(zeroes)) 
            {
                return (result, index);
            }
        }

        throw new Exception($"No long value was found that produces {nZeroes} leading zeroes in the hash");
    }

    public string Answer()
    {
        // part 1
        int n1 = 5;
        var (hash1, index1) = GetLowestNZeroHash(n1);

        // part 2
        int n2 = 6;
        var (hash2, index2) = GetLowestNZeroHashLong(n2);


        return $"the value {index1} produces {n1} leading zeroes in the hash {hash1[..16]}...; and the value {index2} produces {n2} leading zeroes in the hash {hash2[..16]}...";
    }

}
