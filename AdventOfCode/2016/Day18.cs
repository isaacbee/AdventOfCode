namespace AdventOfCode._2016;

public class Day18 : ISolution
{
    private static readonly string inputText = @"^^^^......^...^..^....^^^.^^^.^.^^^^^^..^...^^...^^^.^^....^..^^^.^.^^...^.^...^^.^^^.^^^^.^^.^..^.^";
    private static readonly bool[] inputTraps = InitTraps();

    private static bool[] InitTraps()
    {
        bool[] traps = new bool[inputText.Length];

        for (int i = 0; i < inputText.Length; i++)
        {
            traps[i] = inputText[i] != '.';
        }

        return traps;
    }

    /// <summary>
    /// Counts the number of safe tiles there are in a given number of rows of the map. The rules of how to calculate the next row of traps is given below.
    /// </summary>
    /// <remarks>
    /// A new tile is a trap only in one of the following situations:
    /// 1. Its left and center tiles are traps, but its right tile is not;
    /// 2. Its center and right tiles are traps, but its left tile is not;
    /// 3. Only its left tile is a trap;
    /// 4. Only its right tile is a trap.
    /// In any other situation, the new tile is safe.
    /// </remarks>
    /// <param name="rows"></param>
    /// <returns></returns>
    private static int GetTotalSafeCount(int rows)
    {
        int count = GetRowSafeCount(inputTraps);
        bool[] currentRow = (bool[])inputTraps.Clone();
        bool[] nextRow = new bool[currentRow.Length];

        for (int i = 1; i < rows; i++)
        {
            for (int j = 0; j < currentRow.Length; j++)
            {
                bool left = j - 1 >= 0 && currentRow[j - 1];
                bool center = currentRow[j];
                bool right = j + 1 < currentRow.Length && currentRow[j + 1];

                if ((left && center && !right) || (center && right && !left) || (left && !center && !right) || (right && !left && !center))
                {
                    nextRow[j] = true;
                }
                else
                {
                    nextRow[j] = false;
                    count++;
                }
            }
            currentRow = (bool[]) nextRow.Clone();
        }
        return count;
    }

    private static int GetRowSafeCount(bool[] row)
    {
        int count = 0;
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] == false) 
            {
                count++;
            }
        }
        return count;
    }

    public string Answer()
    {
        // part 1
        int rows1 = 40;
        int safeTiles1 = GetTotalSafeCount(rows1);

        // part 2
        int rows2 = 400000;
        int safeTiles2 = GetTotalSafeCount(rows2);

        return $"in the first {rows1} rows of the map, there are {safeTiles1} safe tiles; and in the first {rows2} rows of the map, there are {safeTiles2} safe tiles";
    }
}