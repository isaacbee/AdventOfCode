using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015;

public class Day08 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day08-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private static string GetExtraCharCount()
    {
        int codeLength = 0;
        int charLength = 0;
        int encodedLength = 0;

        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            codeLength += line.Length;

            // part 1
            string escapedLine = Regex.Unescape(line);
            if (escapedLine.StartsWith('"') && escapedLine.EndsWith('"'))
            {
                // Remove the starting and ending quotes
                escapedLine = escapedLine[1..^1];
            }
            charLength += escapedLine.Length;

            // part 2
            string encodedLine = Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(line, true);
            encodedLength += encodedLine.Length;
        }

        return $"{codeLength - charLength} length difference between the original strings and the decoded strings and {encodedLength - codeLength} length difference between the encoded strings and the original strings";
    }

    public string Answer()
    {
        return GetExtraCharCount();
    }
}
