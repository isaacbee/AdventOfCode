using System.Text.RegularExpressions;

namespace AdventOfCode._2016;

public partial class Day07 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day07-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<string> ipAddresses = InitAddresses();

    private static List<string> InitAddresses()
    {
        List<string> list = [];
        string[] lines = inputText.Split(Environment.NewLine);

        foreach (string line in lines)
        {
            list.Add(line);
        }

        return list;
    }

    private static int IPsSupportTLSCount()
    {
        int count = 0;

        foreach (string ip in ipAddresses)
        {
            bool isTLSSupported = true;

            var bracketMatches = BracketRegex().Matches(ip);
            foreach (Match match in bracketMatches)
            {
                if (ABBARegex().IsMatch(match.Value))
                {
                    isTLSSupported = false;
                    break;
                }
            }

            if (isTLSSupported) 
            {
                if (ABBARegex().IsMatch(ip)) count++;
            }
        }

        return count;
    }

    private static int IPsSupportSLSCount()
    {
        int count = 0;

        foreach (string ip in ipAddresses)
        {
            bool isSLSSupported = false;

            string[] tokens = BracketRegex().Split(ip);

            foreach (string token in tokens)
            {
                var abaMatches = ABARegex().Matches(token);
                foreach (Match abaMatch in abaMatches)
                {
                    string aba = abaMatch.Groups[1].Value;
                    string bab = string.Concat(aba[1], aba[0], aba[1]);

                    var bracketMatches = BracketRegex().Matches(ip);
                    foreach (Match bracketMatch in bracketMatches)
                    {
                        if (bracketMatch.Value.Contains(bab))
                        {
                            isSLSSupported = true;
                            break;
                        }
                    }
                    if (isSLSSupported) break;
                }
                if (isSLSSupported) break;
            }

            if (isSLSSupported) count++;
        }

        return count;
    }

    public string Answer()
    {
        // part 1
        int ipTLSCount = IPsSupportTLSCount();

        // part 2
        int ipSLSCount = IPsSupportSLSCount();

        return $"IP addresses in the list that support TLS = {ipTLSCount}; IP addresses in the list that support SLS = {ipSLSCount}";
    }

    [GeneratedRegex(@"(.)(?!\1)(.)\2\1")]
    private static partial Regex ABBARegex();
    [GeneratedRegex(@"\[.*?\]")]
    private static partial Regex BracketRegex();
    [GeneratedRegex(@"(?=((.)(?!\2).\2))")]
    private static partial Regex ABARegex();
}