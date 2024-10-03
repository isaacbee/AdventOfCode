namespace AdventOfCode._2015;

public class Day14 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2015", "Day14-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);

    private class Reindeer(string name, int speed, int flyingSeconds, int restingSeconds)
    {
        public string Name { get; init; } = name;
        public int Speed { get; init; } = speed;
        public int TotalFlyingSeconds { get; init; } = flyingSeconds;
        public int TotalRestingSeconds { get; init; } = restingSeconds;
        public bool IsFlying { get; set; } = true;
        public int ActiveSeconds { get; set; } = 0;
        public int Distance { get; set; } = 0;
        public int Points { get; set; } = 0;
    }

    private static Dictionary<string, Reindeer> Init()
    {
        Dictionary<string, Reindeer> reindeer = [];
        string[] lines = inputText.Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ');

            string name = tokens[0];
            int speed = int.Parse(tokens[3]);
            int flyingTime = int.Parse(tokens[6]);
            int restingTime = int.Parse(tokens[13]);

            reindeer.Add(name, new Reindeer(name, speed, flyingTime, restingTime));
        }
        return reindeer;
    }

    private static (string, int) CalculateFastestReindeer(Dictionary<string, Reindeer> reindeer, int seconds)
    {
        string fastestReindeer = string.Empty;
        int winningDistance = 0;

        foreach ((_, Reindeer r) in reindeer)
        {
            int distance = CalculateReindeerTravel(r, seconds);

            if (distance > winningDistance)
            {
                winningDistance = distance;
                fastestReindeer = r.Name;
            }
        }

        return (fastestReindeer, winningDistance);
    }

    private static int CalculateReindeerTravel(Reindeer reindeer, int totalSeconds)
    {
        int ellapsedSeconds = 0;
        int speed = reindeer.Speed;
        int flyingSeconds = reindeer.TotalFlyingSeconds;
        int restingSeconds = reindeer.TotalRestingSeconds;

        while (ellapsedSeconds < totalSeconds)
        {
            bool isFlying = reindeer.IsFlying;
            int activeSeconds = reindeer.ActiveSeconds;

            if ((isFlying is true && activeSeconds < flyingSeconds) || (isFlying is false && activeSeconds < restingSeconds))
            {
                if (isFlying) reindeer.Distance += speed;
            }
            else
            {
                reindeer.IsFlying = !isFlying;
                reindeer.ActiveSeconds = 0;
                continue;
            }

            reindeer.ActiveSeconds++;
            ellapsedSeconds++;
        }

        return reindeer.Distance;
    }

    private static (string, int) CalculateHighestScoringReindeer(Dictionary<string, Reindeer> reindeer, int totalSeconds)
    {
        string winningReindeer = string.Empty;
        int winningPoints = 0;
        int ellapsedSeconds = 0;

        while (ellapsedSeconds < totalSeconds)
        {
            ellapsedSeconds++;
            CalculateNextDeltaReindeerLeader(reindeer);
        }

        foreach ((_, Reindeer r) in reindeer)
        {
            int points = r.Points;

            if (points > winningPoints)
            {
                winningPoints = points;
                winningReindeer = r.Name;
            }
        }

        return (winningReindeer, winningPoints);
    }

    private static void CalculateNextDeltaReindeerLeader(Dictionary<string, Reindeer> reindeer)
    {
        string leader = string.Empty;
        int leadingDistance = 0;

        foreach ((_, Reindeer r) in reindeer)
        {
            int speed = r.Speed;
            int flyingSeconds = r.TotalFlyingSeconds;
            int restingSeconds = r.TotalRestingSeconds;

            Start:
            bool isFlying = r.IsFlying;
            int activeSeconds = r.ActiveSeconds;

            if ((isFlying is true && activeSeconds < flyingSeconds) || (isFlying is false && activeSeconds < restingSeconds))
            {
                if (isFlying) r.Distance += speed;
            }
            else
            {
                r.IsFlying = !isFlying;
                r.ActiveSeconds = 0;
                goto Start;
            }

            if (r.Distance > leadingDistance)
            {
                leadingDistance = r.Distance;
            }

            r.ActiveSeconds++;
        }

        foreach ((_, Reindeer r) in reindeer)
        {
            if (r.Distance == leadingDistance)
            {
                r.Points++;
                leader = r.Name;
            }
        }
    }

    public string Answer()
    {
        int seconds = 2503;

        // part 1
        Dictionary<string, Reindeer> reindeer = Init();
        (string fastestReindeer, int bestDistance) = CalculateFastestReindeer(reindeer, seconds);

        // part 2
        reindeer = Init();
        (string highestScoringReindeer, int highScore) = CalculateHighestScoringReindeer(reindeer, seconds);

        return $"the fastest reindeer {fastestReindeer} traveled {bestDistance} km in {seconds} seconds; and the highest scoring reindeer {highestScoringReindeer} got {highScore} points in {seconds} seconds";
    }

}
