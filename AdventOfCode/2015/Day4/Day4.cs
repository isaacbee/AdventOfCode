using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AdventOfCode._2015;

public class Day4 : ISolution
{
    // private static readonly string filePath = $"lib\\2015\\Day4\\input.txt";
    private static readonly string inputText = "ckczppom";
    // private static readonly int numThreads = 4;
    // private static readonly int numTasks = 10000000;

    private static string GetLowestNZeroHash(int nZeroes)
    {
        int value = 0;
        byte[] hashBytes = [];
        string zeroes = new('0', nZeroes);

        while (value < int.MaxValue) {
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{++value}");
            hashBytes = MD5.HashData(inputBytes);

            string result = Convert.ToHexString(hashBytes);
            if (result[..5].Equals(zeroes)) 
            {
                return $"the value {value} produces {nZeroes} leading zeroes in the hash {result[..10]} ";
            }
        }

        return $"no integer value was found that produces {nZeroes} leading zeroes in the hash ";
    }

    private static string GetLowestNZeroHashLong(int nZeroes, long start = 0)
    {
        long value = start;
        byte[] hashBytes = [];
        string zeroes = new('0', nZeroes);
        Stopwatch sw = Stopwatch.StartNew();
        int currentLineCursor = Console.CursorTop;

        try {
            while (value < long.MaxValue) {
                value++;
                // if (value % 10000000 == 0)
                // {
                //     Console.Write($"\rOn {MethodBase.GetCurrentMethod()?.DeclaringType?.FullName}. Checking {value} which is {(float)value/int.MaxValue} times a max int and {(float)value/long.MaxValue*100}% of a max long. Running for {(float)sw.ElapsedMilliseconds/1000} s     ");
                // }

                byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{value}");
                hashBytes = MD5.HashData(inputBytes);

                string result = Convert.ToHexString(hashBytes);
                if (result[..nZeroes].Equals(zeroes)) 
                {
                    return $"the value {value} produces {nZeroes} leading zeroes in the hash {result[..16]} ";
                }
            }

            return $"no long value was found that produces {nZeroes} leading zeroes in the hash ";
        }
        finally
        {
            // Console.Write($"\r{new string(' ', Console.WindowWidth)}");
            // Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    // // multi thread/task solution
    // private static (bool isSuccess, long value, string hash) GetLowestNZeroHash(int nZeroes, long start, long count, CancellationTokenSource cts, CancellationToken token)
    // {
    //     long value = start;
    //     string zeroes = new('0', nZeroes);
    //     try {
    //         while (value-start < count) {
    //             if (!token.IsCancellationRequested)
    //             {
    //                 byte[] inputBytes = Encoding.ASCII.GetBytes($"{inputText}{++value}");
    //                 byte[] hashBytes = MD5.HashData(inputBytes);
    //                 string result = Convert.ToHexString(hashBytes);
    //                 if (result[..5].Equals(zeroes)) 
    //                 {
    //                     return (true, value, result);
    //                 }
    //             }
    //             else
    //                 break;
    //         }
    //         return (false, value, "");
    //     }
    //     finally 
    //     {
    //         cts.Cancel();
    //     }
    // }

    public string Answer()
    {
        string answer = string.Empty;

        // part 1
        answer += $"{GetLowestNZeroHash(5)}";

        // part 2 - single thread
        answer += $"{GetLowestNZeroHashLong(6, 0L)}";

        // // part 2 - multi-Thread
        // int leadingZeroes = 5;
        // List<Thread> threads = [];
        // List<(bool isSuccess, long value, string hash)> results = [];
        // List<CancellationToken> tokens = [];
        // List<CancellationTokenSource> sources = [];
        // long count = long.MaxValue / numThreads;
        // (long lowestValue, string lowestHash) = (long.MaxValue, string.Empty);
        // sources.Add(new CancellationTokenSource());
        // tokens.Add(sources[0].Token);
        // threads.Add(new Thread(() => 
        //     results.Add(GetLowestNZeroHash(leadingZeroes, 0, count, sources[0], tokens[0]))
        // ));
        // for (int i = 1; i < numThreads; i++) 
        // {
        //     int j = i;
        //     sources.Add(new CancellationTokenSource());
        //     tokens.Add(sources[j].Token);
        //     long start = count * j;
        //     threads.Add(new Thread(() => 
        //         results.Add(GetLowestNZeroHash(leadingZeroes, start, count, sources[j-1], tokens[j]))
        //     ));
        // }
        // foreach (Thread thread in threads)
        // {
        //     thread.Start();
        // }
        // foreach (Thread thread in threads)
        // {
        //     thread.Join();
        // }
        // foreach (var (isSuccess, value, hash) in results)
        // {
        //     if (isSuccess && value < lowestValue)
        //     {
        //         lowestValue = value;
        //         lowestHash = hash;
        //     }
        // }
        // if (lowestValue < long.MaxValue)
        // {
        //     answer += $"the value {lowestValue} produces {leadingZeroes} leading zeroes in the hash {lowestHash[..10]} ";
        // }
        // else 
        // {
        //     answer += $"no integer value was found that produces {leadingZeroes} leading zeroes in the hash ";
        // }

        // // part 2 - multi-Task
        // int leadingZeroes = 6;
        // Task[] tasks = new Task[numTasks];
        // List<(bool isSuccess, long value, string hash)> results = [];
        // List<CancellationToken> tokens = [];
        // List<CancellationTokenSource> sources = [];
        // long count = long.MaxValue / numTasks;
        // (long lowestValue, string lowestHash) = (long.MaxValue, string.Empty);
        // sources.Add(new CancellationTokenSource());
        // tokens.Add(sources[0].Token);
        // tasks[0] = Task.Factory.StartNew(() => 
        //     results.Add(GetLowestNZeroHash(leadingZeroes, 0, count, sources[0], tokens[0])), TaskCreationOptions.PreferFairness
        // );
        // for (int i = 1; i < numTasks; i++) 
        // {
        //     int j = i;
        //     sources.Add(new CancellationTokenSource());
        //     tokens.Add(sources[j].Token);
        //     long start = count * j;
        //     tasks[j] = Task.Factory.StartNew(() => 
        //         results.Add(GetLowestNZeroHash(leadingZeroes, start, count, sources[j-1], tokens[j])), TaskCreationOptions.PreferFairness
        //     );
        // }
        // Task.WaitAll(tasks);
        // foreach (var (isSuccess, value, hash) in results)
        // {
        //     if (isSuccess && value < lowestValue)
        //     {
        //         lowestValue = value;
        //         lowestHash = hash;
        //     }
        // }
        // if (lowestValue < long.MaxValue)
        // {
        //     answer += $"the value {lowestValue} produces {leadingZeroes} leading zeroes in the hash {lowestHash[..10]} ";
        // }
        // else 
        // {
        //     answer += $"no integer value was found that produces {leadingZeroes} leading zeroes in the hash ";
        // }

        return answer;
    }

}
