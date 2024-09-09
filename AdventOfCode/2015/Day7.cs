using System;

namespace AdventOfCode._2015;

public class Day7 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day7-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private class LogicGate
    {
        public string Output { get; init; }
        public ushort? Value { get; set; }
        public bool IsValueSet { get; set; } = false;
        public string? Operation { get; init; } = null;
        public string? Operand1 { get; set; } = null;
        public string? Operand2 { get; set; } = null;

        public LogicGate(string operand1, string output)
        {
            Output = output;
            // if (int.TryParse(operand1, out int value))
            // {
            //     Value = (ushort)value;
            //     IsValueSet = true;
            // }
            // else 
            // {
            //     Operand1 = operand1;
            // }

            Operand1 = operand1;
        }
        public LogicGate(string operation, string operand1, string output)
        {
            Output = output;
            Operation = operation;
            // if (int.TryParse(operand1, out int value))
            // {
            //     switch (operation)
            //     {
            //         case "NOT":
            //             Value = (ushort)~value;
            //             IsValueSet = true;
            //             break;
            //         default:
            //             // operation not supported
            //             break;
            //     }
            // }
            // else 
            // {
            //     Operand1 = operand1;
            // }

            Operand1 = operand1;
        }
        public LogicGate(string operand1, string operation, string operand2, string output)
        {
            Output = output;
            Operation = operation;
            // if (int.TryParse(operand1, out int value1) && int.TryParse(operand2, out int value2))
            // {
            //     Value = operation switch
            //     {
            //         "AND" => (ushort)(value1 & value2),
            //         "OR" => (ushort)(value1 | value2),
            //         "LSHIFT" => (ushort)(value1 << value2),
            //         "RSHIFT" => (ushort)(value1 >>> value2),
            //         _ => throw new Exception("Operation not supported")
            //     };

            //     IsValueSet = true;
            // }
            // else 
            // {
            //     Operand1 = operand1;
            //     Operand2 = operand2;
            // }

            Operand1 = operand1;
            Operand2 = operand2;
        }

        public LogicGate(string output, ushort value)
        {
            Output = output;
            Value = value;
            IsValueSet = true;
        }

    }

    private static Dictionary<string, LogicGate> InitLogicGates()
    {
        // init
        string[] lines = inputText.Split('\n');
        Dictionary<string, LogicGate> logicGates = [];

        foreach (string line in lines)
        {
            string[] tokens = line.Trim('\r').Split(' ');
            
            switch (tokens.Length)
            {
                case 3:
                    logicGates.Add(tokens[2], new LogicGate(tokens[0], tokens[2]));
                    break;
                case 4:
                    logicGates.Add(tokens[3], new LogicGate(tokens[0], tokens[1], tokens[3]));
                    break;
                case 5:
                    logicGates.Add(tokens[4], new LogicGate(tokens[0], tokens[1], tokens[2], tokens[4]));
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        return logicGates;
    }

    private static ushort SolveForIdentifier(Dictionary<string, LogicGate> logicGates, string output)
    {
        LogicGate logicGate = logicGates[output];

        if (logicGate.IsValueSet)
            return (ushort)logicGate.Value!;
        else 
        {
            ushort value1 = 0;
            ushort value2 = 0;
            if (logicGate.Operand1 is not null) 
            {
                if (int.TryParse(logicGate.Operand1, out int value))
                    value1 = (ushort)value;
                else
                    value1 = SolveForIdentifier(logicGates, logicGate.Operand1);
            }
            if (logicGate.Operand2 is not null)
            {
                if (int.TryParse(logicGate.Operand2, out int value))
                    value2 = (ushort)value;
                else
                    value2 = SolveForIdentifier(logicGates, logicGate.Operand2);
            }

            logicGate.Value = logicGate.Operation switch
            {
                "AND" => (ushort)(value1 & value2),
                "OR" => (ushort)(value1 | value2),
                "LSHIFT" => (ushort)(value1 << value2),
                "RSHIFT" => (ushort)(value1 >> value2),
                "NOT" => (ushort)~value1,
                _ => value1
            };

            logicGate.IsValueSet = true;
            logicGate.Operand1 = null;
            logicGate.Operand2 = null;

            return (ushort)logicGate.Value;
        }
    }

    public string Answer()
    {
        string answer = string.Empty;

        // part 1
        Dictionary<string, LogicGate> logicGates1 = InitLogicGates();

        ushort a = SolveForIdentifier(logicGates1, "a");

        answer += $"{a} signal at wire 'a' and ";

        // part 2
        Dictionary<string, LogicGate> logicGates2 = InitLogicGates();
        logicGates2["b"] = new LogicGate("b", a);

        answer += $"{SolveForIdentifier(logicGates2, "a")} signal at wire 'a' after rewiring into b";

        return answer;
    }
}
