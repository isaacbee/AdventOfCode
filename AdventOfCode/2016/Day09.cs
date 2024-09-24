using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016;

public partial class Day09 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day09-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    /// <summary>
    /// Gets the value of a decompressed input, using v1 of the compression format.
    /// </summary>
    /// <returns>A string whose value is the decompressed input after a single pass.</returns>
    private static string DecompressTextV1(string input)
    {
        StringBuilder result = new();
        StringBuilder marker = new();
        StringBuilder repeat = new();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '(')
            {
                marker.Clear();
                while (c != ')')
                {
                    marker.Append(c);
                    c = input[++i];
                }
                string[] tokens = marker.ToString().Split(['(', ')', 'x'], StringSplitOptions.RemoveEmptyEntries);
                int length = int.Parse(tokens[0]);
                int n = int.Parse(tokens[1]);

                repeat.Clear();
                for (int j = 0; j < length; j++)
                {
                    repeat.Append(input[++i]);
                }

                string repeated = repeat.ToString();
                for (int j = 0; j < n; j++)
                {
                    result.Append(repeated);
                }
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Represents a decompressed string using version 2 of the compression format. Note that only length and inner compression lengths are preserved.
    /// </summary>
    private class V2
    {
        private List<V2> Inner { get; set; } = [];
        private int Value { get; set; } = 0;
        private int Repeat { get; set; } = 0;

        public V2(string s) : this(s, 1) {}

        private V2(string s, int n)
        {
            Repeat = n;

            StringBuilder marker = new();
            StringBuilder next = new();

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '(')
                {
                    marker.Clear();
                    while (c != ')')
                    {
                        marker.Append(c);
                        c = s[++i];
                    }
                    string[] tokens = marker.ToString().Split(['(', ')', 'x'], StringSplitOptions.RemoveEmptyEntries);
                    int length = int.Parse(tokens[0]);
                    int repeat = int.Parse(tokens[1]);

                    next.Clear();
                    for (int j = 0; j < length; j++)
                    {
                        next.Append(s[++i]);
                    }

                    Inner.Add(new V2(next.ToString(), repeat));
                }
                else
                {
                    Value++;
                }
            }
        }
        /// <summary>
        /// Gets the length of the V2 object by decompressing the input, using version 2 of the compression format.
        /// </summary>
        /// <returns>A long whose value is the length of the fully decompressed input.</returns>
        public long GetLength()
        {
            if (Inner.Count == 0)
            {
                return Value * Repeat;
            }
            else
            {
                long length = 0;
                foreach (V2 innie in Inner)
                {
                    length += innie.GetLength();
                }
                return (length + Value) * Repeat;
            }
        }
    }

    /// <summary>
    /// Gets the number of characters in the decompressed input, using v2 of the compression format. 
    /// </summary>
    /// <remarks>
    /// This only tracks the length of the decompressed string because trying to store the full text of the decompressed <see cref="inputText"/> results in the string exceeding max length: <c>System.OutOfMemoryException: "Insufficient memory to continue the execution of the program."</c>
    /// </remarks>
    /// <returns>The number of characters in the decompressed input.</returns>
    private static long FullDecompressLength(string input)
    {
        V2 v2 = new(input);

        return v2.GetLength();
    }

    public string Answer()
    {
        // remove all whitespace
        string input = WhitespaceRegex().Replace(inputText, string.Empty);

        // part 1
        string decompress1 = DecompressTextV1(input);
        
        // part 2
        long decompress2Length = FullDecompressLength(input);

        return $"the decompressed length of the file = {decompress1.Length}; and the decompressed length of the file using the improved format = {decompress2Length}";
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}