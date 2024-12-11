using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; 
    public int gridSizeX = 10; 
    public int gridSizeY = 10; 
    public float tileSpacing = 1.1f; 
    public LayerMask obstacleLayer; 
    public GameObject[,] grid; 

    void Start()
    {
        grid = new GameObject[gridSizeX, gridSizeY]; // Initialize the grid array
        GenerateGrid(); // Generate the grid
    }

    //function generates the grid with tiles at the correct positions
    void GenerateGrid()
    {
        for(int x = 0; x < gridSizeX; x++) 
        {
            for(int y = 0; y < gridSizeY; y++) 
            { 
                Vector3 tilePosition = GridToWorldPosition(new Vector2Int(x, y));  // Calculate the world position for each tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity); // Create a new tile at the calculated position
                TileInfo tileInfo = tile.GetComponent<TileInfo>(); // Assign a grid position to the tile (useful for other systems like pathfinding)
                if (tileInfo != null)
                {
                    tileInfo.gridPosition = new Vector2Int(x, y);
                }
                tile.name = "Tile (" + x + ", " + y + ")";  // Set the name of the tile in the hierarchy for clarity
                grid[x, y] = tile; // Store the tile in the grid array
            }
        }
    }

    //function converts a world position to grid coordinates
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        //Convert the world position to grid coordinates based on the tile spacing
        int x = Mathf.RoundToInt(worldPosition.x / tileSpacing);
        int y = Mathf.RoundToInt(worldPosition.z / tileSpacing);
        return new Vector2Int(x, y); 
    }

    //function converts grid coordinates to a world position
    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * tileSpacing, 0, gridPos.y * tileSpacing); // Convert the grid position to world position
    }

    //function returns the neighboring grid positions for a given tile
    public List<Vector2Int> GetNeighbors(Vector2Int gridPos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        //Directions for the neighbors (up, right, down, left)
        Vector2Int[] directions ={
            new Vector2Int(0, 1),  // Up
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0)  // Left
        };

        //Loop through all directions to find the neighbors
        for(int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborPos = gridPos + directions[i];
            if(IsInBounds(neighborPos) && IsTileWalkable(neighborPos)) //Check if the neighbor is within bounds and is walkable
            {
                neighbors.Add(neighborPos); // Add to the list of valid neighbors
            }
        }

        return neighbors; 
    }

    //function checks if a tile at the given grid position is walkable
    public bool IsTileWalkable(Vector2Int gridPos)//not working
    {
        
        Vector3 worldPosition = GridToWorldPosition(gridPos);
        if(Physics.CheckSphere(worldPosition, 0.4f, obstacleLayer)) //Check if there is an obstacle at the tile's position using Physics
        {
            return false; //The tile is blocked by an obstacle
        }

        return true; //The tile is free to walk
    }

    //function checks if a grid position is within the bounds of the grid
    private bool IsInBounds(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < gridSizeX &&
               gridPos.y >= 0 && gridPos.y < gridSizeY;
    }
}
