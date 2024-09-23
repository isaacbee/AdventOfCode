using System;

namespace AdventOfCode._2016;

public class Day03 : ISolution
{
    private static readonly string filePath = $"lib\\2016\\Day03-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<(int a, int b, int c)> triList1 = InitTriangles1();
    private static readonly List<(int a, int b, int c)> triList2 = InitTriangles2();

    private static List<(int a, int b, int c)> InitTriangles1()
    {
        List<(int a, int b, int c)> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            list.Add((int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2])));
        }

        return list;
    }

    private static List<(int a, int b, int c)> InitTriangles2()
    {
        List<(int a, int b, int c)> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        for (int i = 0; i < lines.Length; i += 3)
        {
            string lineA = lines[i];
            string lineB = lines[i + 1];
            string lineC = lines[i + 2];

            string[] tokensA = lineA.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] tokensB = lineB.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] tokensC = lineC.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            list.Add((int.Parse(tokensA[0]), int.Parse(tokensB[0]), int.Parse(tokensC[0])));
            list.Add((int.Parse(tokensA[1]), int.Parse(tokensB[1]), int.Parse(tokensC[1])));
            list.Add((int.Parse(tokensA[2]), int.Parse(tokensB[2]), int.Parse(tokensC[2])));
        }

        return list;
    }

    private static int GetValidTriangleCount(List<(int a, int b, int c)> list)
    {
        int triangles = 0;

        foreach ((int a, int b, int c) in list)
        {
            if (IsValidTriangle(a, b, c))
            {
                triangles++;
            }
        }

        return triangles;
    }

    private static bool IsValidTriangle(int a, int b, int c)
    {
        if (a < b + c && b < c + a && c < a + b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string Answer()
    {
        int triangles1 = GetValidTriangleCount(triList1);

        int triangles2 = GetValidTriangleCount(triList2);

        return $"the number of triangles that are possible horizontally = {triangles1}; the number of triangles that are possible vertically = {triangles2}";
    }
}
