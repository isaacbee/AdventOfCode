namespace AdventOfCode._2015;

public class Day13 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day13-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private class DinnerGuest(string name)
    {
        public string Name { get; init; } = name;
        public Dictionary<string, int> Neighbors { get; set; } = [];

    }

    private static Dictionary<string, DinnerGuest> Init()
    {
        Dictionary<string, DinnerGuest> guests = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            string[] tokens = line[..^1].Split(' ');

            string guest = tokens[0];
            string neighbor = tokens[10];
            int happiness = int.Parse(tokens[3]);
            if (tokens[2] == "lose" /* tokens[2] != "gain" */)
            {
                happiness *= -1;
            }

            if (!guests.ContainsKey(guest))
            {
                guests.Add(guest, new DinnerGuest(guest));
            }
            guests[guest].Neighbors.Add(neighbor, happiness);
        }
        return guests;
    }

    private static (int, List<string>) CalculateBestSeating(Dictionary<string, DinnerGuest> guests)
    {
        int bestHappiness = int.MinValue;
        List<string> bestSeating = [];

        List<string> guestList = [];
        foreach (var guest in guests)
        {
            guestList.Add(guest.Key);
        }

        int guestCount = guestList.Count;

        foreach (var perm in GetPermutations(guestList, guestCount))
        {
            List<string> arrangement = perm.ToList();
            int count = arrangement.Count;
            int happiness = 0;
            string currentGuest;
            string previousGuest = arrangement[count-1];

            for (int i = 0; i < guestCount; i++)
            {
                currentGuest = arrangement[i];
                happiness += guests[currentGuest].Neighbors[previousGuest];
                happiness += guests[previousGuest].Neighbors[currentGuest];
                previousGuest = currentGuest;
            }

            if (happiness > bestHappiness)
            {
                bestHappiness = happiness;
                bestSeating = arrangement;
            }
        }

        return (bestHappiness, bestSeating);
    }

    // Version 1 of GetPermutations uses recursive swapping of List elements "in place" to generate all permutations. Not thread safe.
    private static List<List<string>> GetPermutations(List<string> guestList, int left, int right)
    {
        if (left == right)
        {
            return [[.. guestList]];
        }
        else
        {
            List<List<string>> permutations = [];

            for (int i = left; i <= right; i++)
            {
                guestList.Swap(left, i);

                permutations.AddRange(GetPermutations(guestList, left + 1, right));

                guestList.Swap(left, i);
            }

            return permutations;
        }
    }

    // Version 2 of GetPermutations uses recursive transform functions to generate permutations of smaller lists and combining them with the remaining elements. Thread safe.
    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(List<T> list, int length)
    {
        if (length == 1)
        {
            return list.Select(t => new T[] { t });
        }
        else
        {
            return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat([t2]));
        }
    }

    private static void AddDinnerGuest(Dictionary<string, DinnerGuest> guests, string name, int initHappiness = 0)
    {
        var originalGuestList = guests.Keys.ToList();
        foreach ((_, DinnerGuest guest) in guests)
        {
            guest.Neighbors.Add(name, 0);
        }
        guests.Add(name, new DinnerGuest(name));
        foreach (string guest in originalGuestList)
        {
            guests[name].Neighbors.Add(guest, initHappiness);
        }
    }

    public string Answer()
    {
        // part 1
        Dictionary<string, DinnerGuest> guests1 = Init();
        (int happiness1, List<string> bestSeating1) = CalculateBestSeating(guests1);

        // part 2
        Dictionary<string, DinnerGuest> guests2 = Init();
        AddDinnerGuest(guests2, "me");
        (int happiness2, List<string> bestSeating2) = CalculateBestSeating(guests2);

        return $"the best seating arrangement {string.Join(", ", bestSeating1)} = {happiness1}; and changes to {string.Join(", ", bestSeating2)} = {happiness2} after including myself";
    }

}
