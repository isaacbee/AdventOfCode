using System;

namespace AdventOfCode._2015;

public class Day25 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day25-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly (int row, int column) lookup = InitRowColumn();

    private static (int, int) InitRowColumn()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        string line = lines[0].Replace(",", "").Replace(".", "");
        string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return (int.Parse(tokens[15]), int.Parse(tokens[17]));
    }

    private static long CalculateCell(int row, int column)
    {
        int tableSize = row + column - 1;
        long[,] table = new long[tableSize,tableSize];
        int r = 0;
        int c = 0;
        table[r, c] = 20151125;
        long prev;

        while (true)
        {
            prev = table[r, c];
            if (r == 0)
            {
                r = c + 1;
                c = 0;
            }
            else
            {
                r--;
                c++;
            }
            table[r, c] = prev * 252533 % 33554393;

            if (r == row - 1 && c == column - 1)
            {
                break;
            }
        }

        return table[row - 1, column - 1];
    }

    public string Answer()
    {
        long value = CalculateCell(lookup.row, lookup.column);

        return $"the value at ({lookup.row},{lookup.column}) = {value}";
    }
}
