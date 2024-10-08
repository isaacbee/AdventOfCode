using System.Text;

namespace AdventOfCode._2016;

public class Day16 : ISolution
{
    private static readonly string input= "11011110011011101";

    private static string GetChecksum(string input, int discSize)
    {
        string checksum = FillDisc(input, discSize);
        StringBuilder sb = new();

        // while even length, repeatedly calculate the next checksum by comparting pairs of digits of the previous checksum
        while (checksum.Length % 2 == 0)
        {
            sb.Clear();
            for (int i = 0; i < checksum.Length; i+=2)
            {
                sb.Append(checksum[i] == checksum[i + 1] ? '1' : '0');
            }
            checksum = sb.ToString();
        }

        return checksum;
    }

    private static string FillDisc(string input, int discSize)
    {
        string a = input;
        while (true)
        {
            int length = a.Length;

            if (length <= discSize)
            {
                // b = reverse a
                char[] b = a.ToCharArray().Reverse().ToArray();

                // b = inverse b
                for (int i = 0; i < length; i++)
                {
                    b[i] = b[i] == '0' ? '1' : '0';
                }

                // concat (a, 0, b)
                a = string.Concat(a, "0", new string(b));
            }
            else 
            {
                // return data that fits on the disc
                for (int i = 0; i < length - discSize; i++)
                {
                    a = a[..discSize];
                }
                return a;
            }
        }
    }

    public string Answer()
    {
        // part 1
        int discSize1 = 272;
        string checksum1 = GetChecksum(input, discSize1);

        // part 2
        int discSize2 = 35651584;
        string checksum2 = GetChecksum(input, discSize2);

        return $"after filling a disc of length {discSize1}, the resulting checksum = {checksum1}; and after filling a disc of length {discSize2}, the resulting checksum = {checksum2}";
    }
}