using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("Tile Size - Should match UnityEditor snap settings.")]
    [SerializeField] Vector2Int tileSize = new Vector2Int(10, 10);
    [SerializeField] Vector2Int gridSize = Vector2Int.zero;
    [SerializeField] Vector2Int gridStart = Vector2Int.zero;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public Vector2Int TileSize { get { return tileSize; } }
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; ++x)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                Vector2Int coordinates = new Vector2Int(gridStart.x + x, gridStart.y + y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
            grid[coordinates].isPath = false;
            grid[coordinates].isExplored = false;
        }
    }

    public void ResetNodes()
    {
        foreach (Node node in grid.Values)
        {
            node.connectedTo = null;
            node.isExplored = false;
            node.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / tileSize.x);
        coordinates.y = Mathf.RoundToInt(position.z / tileSize.y);
        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = Vector3.zero;
        position.x = coordinates.x * tileSize.x;
        position.z = coordinates.y * tileSize.y;
        return position;
    }
}
