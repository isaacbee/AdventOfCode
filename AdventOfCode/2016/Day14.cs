using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode._2016;

public class Day14 : ISolution
{
    private static readonly string inputText = "zpqevtbw";

    private static int GetNextCharPassword(int keyCount, int extraHash = 0, int checkNext = 1000)
    {
        int step = 0;
        List<(char c, int l)> triplets = [];
        List<(int l, string s)> keys = [];
        int endAt = int.MaxValue;
        
        // Checks if we have done enough loops to get the first {keyCount} keys
        while (step <= endAt) {

            // Takes the MD5 hash of the input. 1 time for part 1 and {extraHash} more times for part 2.
            string r = $"{inputText}{step}";
            for (int i = 0; i <= extraHash; i++)
            {
                byte[] hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(r));
                r = Convert.ToHexString(hashBytes).ToLower();
            }
            
            // Checks if a potential triplet is no longer valid as a key.
            if (triplets.Count > 0 && step > triplets[0].l + checkNext) 
            {
                triplets.RemoveAt(0);
            }

            // Checks if a hash contains a quint in the same letter as one of the potential triplets. If yes, the triplet was a valid key.
            if (triplets.Count > 0)
            {
                for (int i = 0; i < r.Length - 4; i++)
                {
                    char c = r[i];

                    if (c == r[i + 1] && c == r[i + 2] && c == r[i + 3] && c == r[i + 4])
                    {
                        for (int j = triplets.Count - 1; j >= 0 ; j--)
                        {
                            if (triplets[j].c == c)
                            {
                                keys.Add((triplets[j].l, r));
                                triplets.RemoveAt(j);
                            }
                            if (keys.Count == keyCount)
                            {
                                endAt = step + checkNext;
                            }
                        }
                    }
                }
            }
            
            // Checks if a hash contains a triplet and add it to the list of possible keys.
            for (int i = 0; i < r.Length - 2; i++)
            {
                char c = r[i];
                if (c == r[i + 1] && c == r[i + 2])
                {
                    triplets.Add((c, step));
                    break;
                }
            }

            step++;
        }

        if (keys.Count >= keyCount)
        {
            // Checks the valid keys for the {keyCount}th generated key and returns its index.
            keys.Sort();
            return keys[keyCount - 1].l;
        }
        else
        {
            throw new Exception($"Was not able to generate {keys} one-time pad keys before reaching the max index");
        }
    }

    public string Answer()
    {
        int keyCount = 64;

        // part 1
        int index1 = GetNextCharPassword(keyCount);

        // part 2
        int extraHash = 2016;
        int index2 = GetNextCharPassword(keyCount, extraHash);

        return $"the index that produces the {keyCount}th one-time pad key = {index1}; and the index that produces your {keyCount}th one-time pad key using {extraHash} extra MD5 calls of key stretching = {index2}";
    }
}