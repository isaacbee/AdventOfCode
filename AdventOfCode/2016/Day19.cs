namespace AdventOfCode._2016;

public class Day19 : ISolution
{
    private const int elfCount = 3004953;

    /// <summary>
    /// Keeps track of all the presents as each Elf in the circle takes from the next Elf that has presents remaining. 
    /// </summary>
    /// <returns>The Elf index that holds all the presents at the end.</returns>
    private static int TakeNextPresents()
    {
        int[] elves = new int[elfCount];
        Array.Fill(elves, 1);

        while (true)
        {
            for (int i = 0; i < elfCount; i++)
            {
                if (elves[i] > 0)
                {
                    int next = i + 1 < elfCount ? i + 1 : 0;
                    while (elves[next] == 0)
                    {
                        next = next + 1 < elfCount ? next + 1 : 0;
                    }
                    if (next == i)
                    {
                        return i + 1;
                    }
                    elves[i] += elves[next];
                    elves[next] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Rather than store the number of presents like part 1, this version just keeps track of the original index of each Elf because we don't actually need to track presents part way through the party. We also use a <see cref="LinkedList{T}"/> because it takes O(1) to remove a node instead of O(n) from a <see cref="List{T}"/> or Array. We also keep track of the index of the "opposite" Elf throughout the party, rather than calculating it from scratch: if the previous count was even, advance 1, otherwise advance 2.
    /// </summary>
    /// <returns>The Elf index that holds all the presents at the end.</returns>
    private static int TakeOppositePresents()
    {
        LinkedList<int> elves = [];
        for (int i = 0; i < elfCount; i++)
        {
            elves.AddLast(i + 1);
        }

        var current = elves.First;
        var next = current;
        for (int i = 0; i < elves.Count / 2; i++)
        {
            next = next!.Next;
        }

        while (true)
        {
            if (next == current)
            {
                return elves.First!.Value;
            }
            var remove = next;
            if (elves.Count % 2 == 0)
            {
                next = elves.GetCircularNext(next);
            }
            else
            {
                next = elves.GetCircularNext(next);
                next = elves.GetCircularNext(next);
            }
            elves.Remove(remove!);
            current = elves.GetCircularNext(current);
        }
    }

    public string Answer()
    {
        // part 1
        int elf1 = TakeNextPresents();

        // part 1
        int elf2 = TakeOppositePresents();

        return $"Elf # {elf1} gets all the presents when taking from the next Elf; Elf # {elf2} gets all the presents when taking from the opposite Elf";
    }
}