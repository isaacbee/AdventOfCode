using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode._2016;

public class Day17 : ISolution
{
    private static readonly string inputText = "rrrbmfta";
    private static readonly bool[,] rooms = new bool[4,4];

    private static string GetPath(bool isShortest = true)
    {
        Queue<(string, (int, int))> q = new();
        q.Enqueue((string.Empty, (0, 0)));
        string longestPath = string.Empty;

        while (q.Count > 0) 
        {
            (string path, (int v, int h)) = q.Dequeue();
            
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{path}");
            byte[] hashBytes = MD5.HashData(inputBytes);
            string result = Convert.ToHexString(hashBytes).ToLower()[..4];
            for (int i = 0; i < result.Length; i++)
            {
                char direction = directions[i];
                (int dv, int dh) = DirectionToCoord(direction);
                (int iv, int ih) = (v + dv, h + dh);

                if (iv >= 0 && iv < rooms.GetLength(0) && ih >= 0 && ih < rooms.GetLength(1))
                {
                    char c = result[i];
                    if (!char.IsAsciiDigit(c) && c != 'a')
                    {
                        if ((iv, ih) == (3, 3))
                        {
                            if (isShortest)
                            {
                                return string.Concat(path, direction);
                            }
                            else
                            {
                                longestPath = string.Concat(path, direction);
                                continue;
                            }
                        }
                        q.Enqueue((string.Concat(path, direction), (iv, ih)));
                    }
                }
            }
        }

        if (!isShortest) return longestPath;
        throw new Exception("No path found to vault");
    }

    private static readonly char[] directions =
    [
        // swap up and down to match the orientation of the rooms
        'U',    // U
        'D',    // D
        'L',    // L
        'R'     // R
    ];

    private static (int v, int h) DirectionToCoord(char direction)
    {
        (int v, int h)[] moveToCoord =
        [
            // swap up and down to match the orientation of the rooms
            (-1, 0),    // U
            (1, 0),     // D
            (0, -1),    // L
            (0, 1)      // R
        ];

        return direction switch 
        {
            'U' => moveToCoord[0],
            'D' => moveToCoord[1],
            'L' => moveToCoord[2],
            'R' => moveToCoord[3],
            _ => (0, 0)
        };
    }

    public string Answer()
    {
        // part 1
        string path1 = GetPath();

        // part 2
        string path2 = GetPath(false);

        return $"the shortest path to reach the vault = {path1}; and the length of the longest path to reach the vault = {path2.Length}";
    }
}