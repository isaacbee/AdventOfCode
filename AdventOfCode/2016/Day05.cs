using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode._2016;

public class Day05 : ISolution
{
    // private static readonly string filePath = $"lib\\2016\\Day05-input.txt";
    private static readonly string inputText = "reyedfim";

    private static string GetPassword(int length, bool isPart2 = false)
    {
        char[] result = new char[length];
        Array.Fill(result, '_');
        long start = 0;

        for (int i = 0; i < length; i++)
        {
            (char r, start, int index) = GetNextCharPassword(5, start, isPart2);

            if (!isPart2)
            {
                result[i] = r;
            }
            else if (index >= 0 && index < length && result[index] == '_')
            {
                result[index] = r;
            }
            else
            {
                i--;
            }
            start++;

            // cinematic "decrypting" animation
            Console.Write(string.Concat(result).ToLower() + "\r");
        }

        return string.Concat(result).ToLower();
    }

    private static (char c, long end, int index) GetNextCharPassword(int nZeroes, long start = 0, bool isPart2 = false)
    {
        long step = start;
        byte[] hashBytes;
        string zeroes = new('0', nZeroes);

        while (step < int.MaxValue) {
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{++step}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..5].Equals(zeroes)) 
            {
                if (isPart2 && int.TryParse(result[5].ToString(), out int number))
                {
                    return (result[6], step, number);
                }
                else
                {
                    return (result[5], step, -1);
                }
            }
        }

        throw new Exception($"No hash found that starts with {nZeroes} leading zeroes starting from {start}");
    }

    public string Answer()
    {
        int passwordLength = 8;

        // part 1
        string password1 = GetPassword(passwordLength);

        // part 2
        string password2 = GetPassword(passwordLength, true);

        return $"the password for the first door = {password1}; and the password for the second door = {password2}";
    }
}
