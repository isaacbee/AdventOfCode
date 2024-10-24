namespace AdventOfCode._2016;

public class Day22 : ISolution
{
    private static readonly string filePath = Path.Join("lib", "2016", "Day22-input.txt");
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly StorageNode[,] nodes = InitNodes();

    private class StorageNode(int size, int used, int avail, int usePercent)
    {
        public int Size { get; private set; } = size;
        public int Used { get; private set; } = used;
        public int Avail { get; private set; } = avail;
        public int UsePercent { get; private set; } = usePercent;
    }

    private static StorageNode[,] InitNodes()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        string[] tokens = lines[^1].Split([' ', '-', '/'], StringSplitOptions.RemoveEmptyEntries);
        int xMax = int.Parse(tokens[3][1..]);
        int yMax = int.Parse(tokens[4][1..]);

        StorageNode[,] nodes = new StorageNode[yMax + 1, xMax + 1];
        foreach (string line in lines)
        {
            if (line.StartsWith('/'))
            {
                tokens = line.Split([' ', '-', '/'], StringSplitOptions.RemoveEmptyEntries);
                int xNode = int.Parse(tokens[3][1..]);
                int yNode = int.Parse(tokens[4][1..]);
                StorageNode node = new(int.Parse(tokens[5][..^1]), int.Parse(tokens[6][..^1]), int.Parse(tokens[7][..^1]), int.Parse(tokens[8][..^1]));
                nodes[yNode, xNode] = node;
            }
        }
        return nodes;
    }

    /// <summary>
    /// Returns the number of viable pairs of nodes in the storage cluster.
    /// </summary>
    /// <remarks>A viable pair of nodes is any two nodes (A,B) such that 1) Node A is not empty; 2) Nodes A and B are not the same node; and 3) The data on node A would fit on node B.</remarks>
    private static int GetViablePairs()
    {
        int count = 0;
        var usedNodes = nodes.Cast<StorageNode>().OrderBy(x => x.Used).ToArray();
        var availNodes = nodes.Cast<StorageNode>().OrderByDescending(x => x.Avail).ToArray();

        for (int i = 0; i < usedNodes.Length; i++)
        {
            for (int j = 0; j < availNodes.Length; j++)
            {
                StorageNode currentNode = usedNodes[i];
                StorageNode checkNode = availNodes[j];
                if (currentNode.Used > 0) // condition (1)
                {
                    if (currentNode != checkNode) // condition (2)
                    {
                        if (currentNode.Used <= checkNode.Avail) // condition (3)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Draw a representation of the layout of the nodes in the storage cluster.
    /// </summary>
    /// <remarks>The empty node = <c>_</c>; Nodes that are too full to fit in the empty node = <c>#</c>; Accessable node at (0,0) = <c>(.)</c>; Node with the goal data = <c>G</c>; All other nodes = <c>.</c></remarks>
    private static void DrawNodes()
    {
        // First calculate the size of the empty node.
        int emptyAvail = 0;
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i,j].Used == 0)
                {
                    emptyAvail = nodes[i,j].Avail;
                    break;
                }
            }
            if (emptyAvail > 0)
            {
                break;
            }
        }

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i,j].Used == 0)
                {
                    Console.Write(" _ ");
                }
                else if (nodes[i,j].Used > emptyAvail)
                {
                    Console.Write(" # ");
                }
                else if ((j, i) == (0, 0))
                {
                    Console.Write("(.)");
                }
                else if ((j, i) == (nodes.GetLength(1) - 1, 0))
                {
                    Console.Write(" G ");
                }
                else 
                {
                    Console.Write(" . ");
                }
            }
            Console.WriteLine();
        }
    }

    public string Answer()
    {
        // part 1
        int viablePairs = GetViablePairs();
        // This identifies a few properties of the data: 1) Data can only fit in the singular empty node (which gets drawn as '_'); 2) Of the 1050 nodes, there are 29 nodes that are too full to fit in the empty node (which get drawn as '#'). 3) All other 1020 nodes have data which fits in the empty node but not each other (these get drawn as '.')

        // part 2
        // Note that this is much faster to manually solve the "maze" by hand or by calculating the total number of steps than to write a BFS solver. If you wanted to do a programming solve, you would set the start to be the initial initial blank node and end point to be the space immediately to the left of the node with the goal data. You would then run a BFS to get to that end, treating the edges and '#' nodes as walls. You would then do the swap between the new blank node position and the goal data node. Then repeatedly run the BFS to get the new start position of the blank node to the end space immediately left of the new goal data node; until the goal data is in the (0,0) node.
        DrawNodes();
        // S............................G
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // .#############################
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ...._.........................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................
        // ..............................

        // We can directly access data only on node /dev/grid/node-x0-y0, but we can move data from any standard node ('.') to the blank node ('_'). The goal is to gain access to the data which begins in the node with y=0 and the highest x (that is, the node in the top-right corner) (i.e. (29,0)), the node with the goal data is marked with 'G'. 
        // We can move data continually from the next standard node to the blank node, effectively moving the blank node. This blank node needs to move from its original position to the space directly in front (left) of the goal node, but needs to navigate around the nodes that are too full ('#'). This is accomplished via 4 moves left (to (0,25)), 25 nodes up (to (0,0)), and 28 nodes right (to (28,0)). This involved a total of 57 moves to get the blank node in position. 
        // Now we can start moving the goal data to the left. We can do so by first moving the goal data left into the blank node (i.e. from (29,0) to (28,0)), then 'move' the blank node down, left, left, left, and up (i.e. from (29,0) to (27,0)); this gets the blank node back into position again for the next move of the goal data. Because each move of the goal data takes 5 total moves of data (except for the last move taking only 1), this makes moving the goal data to (0,0) take a total of 28 * 5 + 1 = 141 moves.
        // The total moves for both parts is 57 + 141 = 198

        return $"there are {viablePairs} viable pairs of nodes in the storage cluster; and the fewest number of steps required to move your goal data to node-x0-y0 = 198";
    }
}