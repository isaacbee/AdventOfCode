using System.Collections;

namespace AdventOfCode._2016;

public class Day20 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day20-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private const long maxIP = 4294967295;
    private static readonly LargeBitArray validIPs = InitIPs();
    
    private class LargeBitArray
    {
        private const int MaxBitsPerArray = int.MaxValue;
        private readonly BitArray[] bitArrays;
        private readonly long totalLength;
        public long SmallestTrueIndex { get; set; }
        public long TrueCount { get; set; }

        public LargeBitArray(long size, bool defaultValue)
        {
            totalLength = size;

            int numOfArrays = (int)(size / MaxBitsPerArray) + 1;
            bitArrays = new BitArray[numOfArrays];

            for (int i = 0; i < numOfArrays; i++)
            {
                // Create a BitArray of the necessary size for each chunk
                int chunkSize = (int)Math.Min(MaxBitsPerArray, size - (long)i * MaxBitsPerArray);
                bitArrays[i] = new BitArray(chunkSize, defaultValue);
            }

            TrueCount = defaultValue ? totalLength : 0;
            SmallestTrueIndex = defaultValue ? 0 : -1;
        }

        public bool this[long index]
        {
            get
            {
                if (index < 0 || index >= totalLength)
                    throw new IndexOutOfRangeException();

                int arrayIndex = (int)(index / MaxBitsPerArray);
                int bitIndex = (int)(index % MaxBitsPerArray);

                return bitArrays[arrayIndex][bitIndex];
            }
            set
            {
                if (index < 0 || index >= totalLength)
                    throw new IndexOutOfRangeException();

                int arrayIndex = (int)(index / MaxBitsPerArray);
                int bitIndex = (int)(index % MaxBitsPerArray);

                bitArrays[arrayIndex][bitIndex] = value;
            }
        }

        public long Length => totalLength;
    }

    private class Interval(long start, long end)
    {
        public long Start { get; set; } = start;
        public long End { get; set; } = end;
    }

    private static List<Interval> MergeAndSortIntervals(List<Interval> intervals)
    {
        // Sort the intervals by the start of the range.
        intervals = [.. intervals.OrderBy(i => i.Start)];
        List<Interval> result = [];
        Interval currentInterval = intervals[0];

        foreach (var interval in intervals)
        {
            // If the next interval has overlap with or extends the current interval, merge them. Otherwise, move to the next interval.
            if (interval.Start <= currentInterval.End + 1)
            {
                currentInterval.End = Math.Max(currentInterval.End, interval.End);
            }
            else
            {
                result.Add(currentInterval);
                currentInterval = interval;
            }
        }
        result.Add(currentInterval);
        return result;
    }

    private static LargeBitArray InitIPs()
    {
        // There are more blacklisted IPs than not, so start by assuming they are all blacklisted and only check for when an IP is not on the blacklist
        LargeBitArray validIPs = new(maxIP + 1, false);
        string[] lines = inputText.Split(Environment.NewLine);
        List<Interval> intervals = [];
        foreach (string line in lines)
        {
            string[] tokens = line.Split('-', 2);
            intervals.Add(new Interval(long.Parse(tokens[0]), long.Parse(tokens[1])));
        }

        // Merge and sort the intervals to reduce the redundancy of checking each interval when looking for non-blacklisted IPs
        intervals = MergeAndSortIntervals(intervals);

        // Iterate through the intervals and IP list at the same time. If we hit an IP that the start of an interval, skip all IPs in that interval, otherwise remove it from the blacklist.
        var intervalEnumerator = intervals.GetEnumerator();
        intervalEnumerator.MoveNext();
        for (long i = 0; i <= maxIP; i++)
        {
            var currentInterval = intervalEnumerator.Current;
            if (currentInterval != null && i == currentInterval.Start)
            {
                i += currentInterval.End - currentInterval.Start;
                intervalEnumerator.MoveNext();
            }
            else 
            {
                validIPs[i] = true;

                // Store the smallest valid IP.
                if (validIPs.SmallestTrueIndex < 0)
                {
                    validIPs.SmallestTrueIndex = i;
                }
                // Store the valid IP count.
                validIPs.TrueCount++;
            }
        }
        return validIPs;
    }

    private static long GetLowestValidIP(bool isStored = true)
    {
        if (isStored is false)
        {
            for (long i = 0; i < validIPs.Length; i++)
            {
                if (validIPs[i])
                {
                    return i;
                }
            }
            throw new Exception("No valid IP address found in range.");
        }
        else
        {
            // Rather than iterate through the LargeBitArray again, store the value during initialization.
            return validIPs.SmallestTrueIndex;
        }
    }

    private static long GetValidIPCount(bool isStored = true)
    {
        if (isStored is false)
        {
            long count = 0;
            for (long i = 0; i < validIPs.Length; i++)
            {
                if (validIPs[i])
                {
                    count++;
                }
            }
            return count;
        }
        else
        {
            // Rather than iterate through the LargeBitArray again, store the value during initialization.
            return validIPs.TrueCount;
        }
    }

    public string Answer()
    {
        // part 1
        long lowestValidIP = GetLowestValidIP();

        // part 2
        long validIPCount = GetValidIPCount();

        return $"the lowest-valued IP that is not blocked = {lowestValidIP}; and the count of non-blocked IP addresses = {validIPCount}";
    }
}