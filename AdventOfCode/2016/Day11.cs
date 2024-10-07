namespace AdventOfCode._2016;

public class Day11 : ISolution
{
    /// <summary>
    /// Stores the state of the objects in the Facility. Also provides several helper methods to check the state and get the next state after each move.
    /// </summary>
    private class Facility
    {
        private int Floor { get; set; }
        private (int generator, int microchip)[] State { get; set; }
        public int Steps { get; private set; }

        public Facility((int, int)[] state, int floor) : this(state, floor, 0) { }

        public Facility((int, int)[] state, int floor, int steps)
        {
            Floor = floor;
            State = state;
            NormalizeState();
            Steps = steps;
        }

        /// <summary>
        /// Checks if the state contains a generator and an unshielded microchip on the same floor.
        /// </summary>
        /// <param name="maxFloor">The max floor to check.</param>
        /// <returns><c>true</c> if the <see cref="State"/> is "safe"; otherwise, <c>false</c></returns>
        public bool IsValidState(int maxFloor)
        {
            bool[] isGeneratorOnFloor = new bool[maxFloor];
            bool[] isUnpairedChipOnFloor = new bool[maxFloor];

            for (int i = 0; i < State.Length; i++)
            {
                (int g, int m) = State[i];

                isGeneratorOnFloor[g] |= true;
                isUnpairedChipOnFloor[m] |= g != m;
            }
            for (int j = 0; j < maxFloor; j++)
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

        /// <summary>
        /// Gets the list of all sets of components that can be moved from the current floor.
        /// </summary>
        /// <remarks>
        /// Note that the <see cref="State"/> is stored as a tuple array in the form <c>(int generator, int microchip)[]</c> but the moveList contains indices as if the components were in an flattened int array in the form <c>[ generator1, microchip1, ..., generatorN, microchipN ]</c>
        /// </remarks>
        /// <returns>A list containing all sets of components that can be moved from the current floor.</returns>
        public List<int[]> GetAllMovableComponents()
        {
            List<int[]> moveList = [];
            List<bool> canMoveList = new(State.Length * 2);

            foreach (var (generator, microchip) in State)
            {
                canMoveList.Add(generator == Floor);
                canMoveList.Add(microchip == Floor);
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

        /// <summary>
        /// Creates the next state for a Facility given a set of <paramref name="components"/> and the <paramref name="nextFloor"/> to move to. 
        /// </summary>
        /// <remarks>
        /// Note that this does not check if the next state is valid. Also note that input <paramref name="components"/> is "flattened" and expects the an item from <seealso cref="GetAllMovableComponents"/> and input <paramref name="nextFloor"/> expects a valid floor from <seealso cref="GetNextFloors(int)"/>. 
        /// </remarks>
        /// <param name="components">A set of components to move.</param>
        /// <param name="nextFloor">The floor to move the the components to.</param>
        /// <returns>The next <see cref="State"/>.</returns>
        public (int generator, int microchip)[] GetNextState(int[] components, int nextFloor)
        {
            (int generator, int microchip)[] nextState = [.. State];

            foreach (int component in components)
            {
                if (component % 2 == 0)
                {
                    nextState[ component / 2 ].generator = nextFloor;
                }
                else 
                {
                    nextState[ component / 2 ].microchip = nextFloor;
                }
            }
            return nextState;
        }

        public bool AreAllComponentsOnFloor(int floor)
        {
            foreach (var (generator, microchip) in State)
            {
                int index = floor - 1;
                if (generator != index || microchip != index)
                {
                    return false;
                }
            }
            return true;
        }

        public int[] GetNextFloors(int maxFloor)
        {
            if (Floor == 0)
            {
                return [ 1 ];
            }
            else if (Floor == maxFloor - 1)
            {
                return [ maxFloor - 2 ];
            }
            else
            {
                return [ Floor + 1, Floor - 1 ];
            }
        }

        public override int GetHashCode()
        {
            string test = string.Join(",", State);
            return HashCode.Combine(Floor, string.Join(",", State));
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
                return Floor == other.Floor;
            }
            return false;
        }
    }
    
    /// <summary>
    /// Program entry to determine the fewest number of steps required to bring all of the objects to the top floor. This is done via a BFS of all posible states, while pruning all invalid states and states that have been seen before.
    /// </summary>
    /// <param name="start">Initial <see cref="Facility"/> state.</param>
    /// <param name="maxFloor">Floor to move all objects to.</param>
    /// <returns>The fewest number of steps required to bring all of the objects to the top floor.</returns>
    private static int MinStepsToFloor(Facility start, int maxFloor)
    {
        Queue<Facility> q = [];
        q.Enqueue(start);

        HashSet<Facility> memoization = [ start ];

        while (q.Count > 0)
        {
            Facility current = q.Dequeue();
            int currentSteps = current.Steps;
            int[] nextFloors = current.GetNextFloors(maxFloor);
            List<int[]> moveList = current.GetAllMovableComponents();

            foreach (int floor in nextFloors)
            {
                foreach (int[] move in moveList)
                {
                    (int generator, int microchip)[] nextState = current.GetNextState(move, floor);
                    Facility next = new(nextState, floor, currentSteps + 1);
                    if (next.AreAllComponentsOnFloor(maxFloor))
                    {
                        return next.Steps;
                    }
                    next.NormalizeState();

                    if (memoization.Contains(next) is false && next.IsValidState(maxFloor))
                    {
                        memoization.Add(next);
                        q.Enqueue(next);
                    }
                }
            }
        }
        throw new Exception("No method found to safely transport all the components to the top floor");
    }

    public string Answer()
    {
        int floorCount = 4;
        int startElevator = 0;

        // part 1
        (int generator, int microchip)[] startState1 = 
        [
            (0,0), (1,2), (1,2), (1,2), (1,2)
        ];
        Facility start1 = new(startState1, startElevator);
        int steps1 = MinStepsToFloor(start1, floorCount);

        // part 2
        (int generator, int microchip)[] startState2 = 
        [
            (0,0), (0,0), (0,0), (1,2), (1,2), (1,2), (1,2)
        ];
        Facility start2 = new(startState2, startElevator);
        int steps2 = MinStepsToFloor(start2, floorCount);

        return $"the minimum number of steps required to bring all of the objects to the fourth floor = {steps1}; the minimum number of steps required to bring all of the objects, including the four new ones, to the fourth floor = {steps2}";
    }
}