namespace AdventOfCode._2016;

public class Day11 : ISolution
{
    // private static readonly string filePath = Path.Join("lib", "2016", "Day11-input.txt");
    // private static readonly string inputText = File.ReadAllText(filePath);
    private const int floorCount = 4;
    private static readonly int startElevator = 0;

    private class Facility
    {
        public int Elevator { get; set; }
        public (int generator, int microchip)[] State { get; set; }
        public int Steps { get; set; }

        public Facility((int, int)[] state, int elevator) : this(state, elevator, 0) { }

        public Facility((int, int)[] state, int elevator, int steps)
        {
            Elevator = elevator;
            State = state;
            NormalizeState();
            Steps = steps;
        }

        public bool IsValidState()
        {
            bool[] isGeneratorOnFloor = new bool[floorCount];
            bool[] isUnpairedChipOnFloor = new bool [floorCount];

            for (int i = 0; i < State.Length; i++)
            {
                (int g, int m) = State[i];

                isGeneratorOnFloor[g] = true;
                isUnpairedChipOnFloor[m] |= g != m;
            }
            for (int j = 0; j < floorCount; j++)
            {
                if (isGeneratorOnFloor[j] && isUnpairedChipOnFloor[j])
                {
                    return false;
                }
            }
            return true;
        }

        public void NormalizeState()
        {
            Array.Sort(State);
        }

        public List<int[]> GetAllMovableComponents()
        {
            List<int[]> moveList = [];
            List<bool> canMoveList = new(State.Length * 2);

            foreach (var (generator, microchip) in State)
            {
                canMoveList.Add(generator == Elevator);
                canMoveList.Add(microchip == Elevator);
            }

            for (int i = 0; i < canMoveList.Count; i++)
            {
                if (canMoveList[i] is true)
                {
                    int[] newMove = [ i ];
                    moveList.Add(newMove);

                    for (int j = i + 1; j < canMoveList.Count; j++)
                    {
                        if (canMoveList[j] is true)
                        {
                            newMove = [ i, j ];
                            moveList.Add(newMove);
                        }
                    }
                }
            }

            return moveList;
        }

        public (int generator, int microchip)[] GetNextState(int[] move, int nextFloor)
        {
            (int generator, int microchip)[] nextState = [.. State];

            foreach (int component in move)
            {
                
                if (component % 2 == 0)
                {
                    // if generator
                    nextState[ component / 2 ].generator = nextFloor;
                }
                else 
                {
                    // if microchip
                    nextState[ component / 2 ].microchip = nextFloor;
                }
            }

            return nextState;
        }

        public bool AreAllComponentsOnFloor(int floor)
        {
            foreach (var (generator, microchip) in State)
            {
                if (generator != floor || microchip != floor)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            string test = string.Join(",", State);
            return HashCode.Combine(Elevator, string.Join(",", State));
        }

        public override bool Equals(object? obj)
        {
            if (obj is Facility other)
            {
                if (State.Length != other.State.Length)
                {
                    return false;
                }
                for (int i = 0; i < State.Length; i++)
                {
                    if (State[i] != other.State[i])
                    {
                        return false;
                    }
                }
                return Elevator == other.Elevator;
            }
            return false;
        }
    }

    private static int MinStepsTopFloor(Facility start)
    {
        Queue<Facility> q = [];
        q.Enqueue(start);

        HashSet<Facility> memoization = [ start ];

        while (q.Count > 0)
        {
            Facility current = q.Dequeue();
            int currentElevator = current.Elevator;
            (int generator, int microchip)[] currentState = current.State;
            int currentSteps = current.Steps;

            List<int> nextFloor = currentElevator switch
            {
                0 => [ 1 ],
                floorCount - 1 => [ floorCount - 2 ],
                _ => [ currentElevator - 1, currentElevator + 1 ]
            };

            List<int[]> moveList = current.GetAllMovableComponents();

            foreach (int floor in nextFloor)
            {
                foreach (int[] move in moveList)
                {
                    (int generator, int microchip)[] nextState = current.GetNextState(move, floor);
                    Facility next = new(nextState, floor, currentSteps + 1);
                    if (next.AreAllComponentsOnFloor(floorCount - 1))
                    {
                        return next.Steps;
                    }
                    next.NormalizeState();

                    if (memoization.Contains(next) is false && next.IsValidState())
                    {
                        memoization.Add(next);
                        q.Enqueue(next);
                    }
                }
            }
        }

        return -1;
    }

    public string Answer()
    {
        // part 1
        (int generator, int microchip)[] startState1 = 
        [
            (0,0), (1,2), (1,2), (1,2), (1,2)
        ];
        Facility start1 = new(startState1, startElevator);
        int steps1 = MinStepsTopFloor(start1);

        // part 2
        (int generator, int microchip)[] startState2 = 
        [
            (0,0), (0,0), (0,0), (1,2), (1,2), (1,2), (1,2)
        ];
        Facility start2 = new(startState2, startElevator);
        int steps2 = MinStepsTopFloor(start2);

        return $"the minimum number of steps required to bring all of the objects to the fourth floor = {steps1}; the minimum number of steps required to bring all of the objects, including the four new ones, to the fourth floor = {steps2}";
    }
}