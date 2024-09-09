using System;

namespace AdventOfCode._2015;

public class Day9 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day9-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private class City(string name)
    {
        public string Name { get; init; } = name;
        public Dictionary<string, int> Connections { get; set; } = [];
    }

    private static Dictionary<string, City> Init()
    {
        Dictionary<string, City> cities = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');

            string city1 = tokens[0];
            string city2 = tokens[2];
            int distance = int.Parse(tokens[4]);

            if (!cities.ContainsKey(city1))
            {
                cities.Add(city1, new City(city1));
            }

            if (!cities.ContainsKey(city2))
            {
                cities.Add(city2, new City(city2));
            }

            if (!cities[city1].Connections.ContainsKey(city2))
            {
                cities[city1].Connections.Add(city2, distance);
            }

            if (!cities[city2].Connections.ContainsKey(city1))
            {
                cities[city2].Connections.Add(city1, distance);
            }
        }
        return cities;
    }

    private static (int, List<string>) CalculateHamiltonianConnectedPath(Dictionary<string, City> cities, bool isMinimum)
    {
        int savedDistance;
        if (isMinimum) // part 1
        {
            savedDistance = int.MaxValue;
        }
        else // part 2
        {
            savedDistance = 0;
        }
        List<string> savedPath = [];

        string[] lines = inputText.Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');
            string start = tokens[0];
            string end = tokens[2];

            List<string> notStartOrEnd = new(cities.Keys);
            notStartOrEnd.RemoveAll(c => c == start || c == end);
            
            (int distance, List<string> path) = FindPath(cities, notStartOrEnd, end, 0, start, isMinimum);

            if (distance < savedDistance && isMinimum) // part 1
            {
                savedDistance = distance;
                savedPath = path;
            }
            else if (distance > savedDistance && !isMinimum) // part 2
            {
                savedDistance = distance;
                savedPath = path;
            }
        }
        return (savedDistance, savedPath);
    }

    private static (int, List<string>) FindPath(Dictionary<string, City> cities, List<string> remainingCities, string current, int currDistance, string end, bool isMinimum)
    {
        int savedDistance;
        if (isMinimum) // part 1
        {
            savedDistance = int.MaxValue;
        }
        else // part 2
        {
            savedDistance = 0;
        }
        List<string> savedPath = [];

        if (remainingCities.Count <= 0)
        {
            currDistance += cities[current].Connections[end];
            return (currDistance, new List<string>{ end, current });
        }
        else 
        {
            foreach (string otherCity in remainingCities)
            {
                List<string> newRemainingCities = new(remainingCities);
                newRemainingCities.Remove(otherCity);

                int addDistance = cities[current].Connections[otherCity];

                (int distance, List<string> path) = FindPath(cities, newRemainingCities, otherCity, currDistance + addDistance, end, isMinimum);

                path.Add(current);

                if (distance < savedDistance && isMinimum) // part 1
                {
                    savedDistance = distance;
                    savedPath = path;
                }
                else if (distance > savedDistance && !isMinimum) // part 2
                {
                    savedDistance = distance;
                    savedPath = path;
                }
            }
            return (savedDistance, savedPath);
        }
    }

    private static string PathToString(List<string> path)
    {
        string fullPath = path[0];

        foreach (string s in path) 
        {
            if (fullPath != s)
            {
                fullPath += $" -> {s}";
            }
        }

        return fullPath;
    }

    public string Answer()
    {
        Dictionary<string, City> cities = Init();

        (int shortestDistance, List<string> shortestPath) = CalculateHamiltonianConnectedPath(cities, true);

        (int longestDistance, List<string> longestPath) = CalculateHamiltonianConnectedPath(cities, false);

        return $"the shortest path {PathToString(shortestPath)} = {shortestDistance} and the longest path {PathToString(longestPath)} = {longestDistance}";
    }
}
