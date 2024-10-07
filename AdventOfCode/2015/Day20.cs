namespace AdventOfCode._2015;

public class Day20 : ISolution
{
    private static int CalculateMinimumDeliveriesHouse(int minPresents, int presentsMult, int limit = int.MaxValue)
    {
        int minLength = minPresents / 10;
        int[] houses = new int[minLength];

        for (int step = 1; step <= minLength; step++)
        {
            int count = 0;
            for (int i = step - 1; i < minLength; i += step)
            {
                houses[i] += step * presentsMult;

                count++;
                if (count >= limit)
                {
                    break;
                }
            }
        }

        for (int i = 0; i < minLength; i++)
        {
            if (houses[i] >= minPresents)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public string Answer()
    {
        int presents = 33100000;

        // part 1
        int house1 = CalculateMinimumDeliveriesHouse(presents, 10);

        // part 2
        int limit = 50;
        int house2 = CalculateMinimumDeliveriesHouse(presents, 11, limit);

        return $"the first house to recieve at least {presents} presents = {house1}; and the first house to recieve at least {presents} presents with a delivery limit of {limit} houses = {house2}";
    }
}
