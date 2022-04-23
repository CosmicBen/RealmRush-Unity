using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordintes = Vector2Int.zero;
    public Vector2Int StartCoordinates { get { return startCoordintes; } }

    [SerializeField] Vector2Int destinationCoordinates = Vector2Int.zero;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }
    
    private Node startNode = null;
    private Node destinationNode = null;
    private Node currentSearchNode = null;

    private Dictionary<Vector2Int, Node> nodesReached = new Dictionary<Vector2Int, Node>();
    private Queue<Node> frontier = new Queue<Node>();

    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager gridManager = null;
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            grid = gridManager.Grid;

            startNode = grid[startCoordintes];
            destinationNode = grid[destinationCoordinates];
        }        
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startNode.coordinates);
    }

    public List<Node> GetNewPath(Vector2Int startingCoordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(startingCoordinates);
        return BuildPath();
    }

    private void ExploreNeighbors()
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;
            Node neighbor = gridManager.GetNode(neighborCoordinates);

            if (neighbor != null && !nodesReached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                nodesReached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    private void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        gridManager.ResetNodes();
        frontier.Clear();
        nodesReached.Clear();

        bool isRunning = true;
        
        frontier.Enqueue(grid[coordinates]);
        nodesReached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;

            ExploreNeighbors();

            if (currentSearchNode.coordinates == destinationNode.coordinates)
            {
                isRunning = false;
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            currentNode.isPath = true;
            path.Add(currentNode);
        }

        path.Reverse();

        return path;
    }

    public bool WillBlocKPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
