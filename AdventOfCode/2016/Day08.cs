using System.Text;
using CommunityToolkit.HighPerformance;

namespace AdventOfCode._2016;

public class Day08 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day08-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(Operation op, int A, int B)> opList = InitOpList();

    private enum Operation
    {
        rect,
        rotateRow,
        rotateCol
    }

    private static List<(Operation op, int A, int B)> InitOpList()
    {
        List<(Operation op, int A, int B)> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] tokens = line.Split([' ', 'x', 'y', '='], StringSplitOptions.RemoveEmptyEntries);

            if (tokens[0] == "rect")
            {
                list.Add((Operation.rect, int.Parse(tokens[1]), int.Parse(tokens[2])));
            }
            else if (tokens[0] == "rotate")
            {
                Operation op = tokens[1] switch 
                {
                    "row" => Operation.rotateRow,
                    "column" => Operation.rotateCol,
                    _ => throw new Exception("Invalid operation syntax")
                };
                
                list.Add((op, int.Parse(tokens[2]), int.Parse(tokens[4])));
            }
        }

        return list;
    }

    private static (int count, string display) LitPixels(int columns, int rows)
    {
        int count = 0;
        Span2D<bool> lights = new(new bool[columns, rows]);

        foreach((Operation op, int a, int b) in opList)
        {
            if (op == Operation.rect)
            {
                int width = a;
                int height = b;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        lights[i, j] = true;
                    }
                }
            }
            else if (op == Operation.rotateRow)
            {
                int rowNumber = a;
                int n = b;
                var row = lights.GetRow(rowNumber).ToArray();
                var newRow = row.RotateArrayRight(n);
                
                for (int j = 0; j < row.Length; j++)
                {
                    lights[rowNumber, j] = newRow[j];
                }
            }
            else if (op == Operation.rotateCol)
            {
                int colNumber = a;
                int n = b;
                var col = lights.GetColumn(colNumber).ToArray();
                var newCol = col.RotateArrayRight(n);
                
                for (int i = 0; i < col.Length; i++)
                {
                    lights[i, colNumber] = newCol[i];
                }
            }
        }

        StringBuilder display = new();

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (lights[i, j] is true)
                {
                    count++;
                }
                display.Append(lights[i, j] ? '#' : ' ');
            }
            display.Append(Environment.NewLine);
        }

        return (count, display.ToString());
    }

    public string Answer()
    {
        int rows = 50;
        int columns = 6;

        // part 1, part 2
        (int count, string display) = LitPixels(columns, rows);

        return $"the number of pixels that should be lit = {count}; and the display looks like this{Environment.NewLine}{display}";
    }
}