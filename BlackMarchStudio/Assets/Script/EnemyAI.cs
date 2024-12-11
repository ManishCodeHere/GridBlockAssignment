using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; 
    private GridManager gridManager; 

    private Vector3 lastPlayerPosition; 
    private Vector3 targetPosition; 
    public float enemyHeight= 1.0f; 
    private bool isMoving = false; 

    void Start()
    {
        
        gridManager = FindObjectOfType<GridManager>();
        lastPlayerPosition =player.position;//Set initial values
        targetPosition = transform.position; //Enemy starts at its current position
    }

    void Update()
    {
        //Check if the player has moved
        if(player.position != lastPlayerPosition)
        {
            
            lastPlayerPosition = player.position;
            SetTargetPosition(); //Find a new position for the enemy to move to
        }

        //If the enemy has a target position, move toward it
        if(isMoving)
        {
            MoveToTarget();
        }
    }

    //This function finds the target position for the enemy
    void SetTargetPosition()
    {
        
        Vector2Int playerGridPos = gridManager.WorldToGridPosition(player.position);
        List<Vector2Int> adjacentTiles = gridManager.GetNeighbors(playerGridPos);//Get the 4 tiles next to the player

        //Use a for loop to check each tile
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            Vector2Int tile = adjacentTiles[i];        
            if(gridManager.IsTileWalkable(tile))//Check if the tile is walkable
            {
                
                targetPosition = gridManager.GridToWorldPosition(tile);
                targetPosition.y = enemyHeight;
                isMoving = true;
                return;
            }
        }

        // If no walkable tile is found, the enemy won't move
        isMoving = false;
    }

    // This function moves the enemy toward the target position
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2.0f); // Move closer to the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // Check if the enemy has reached the target
        {
            
            isMoving= false; // Stop moving once the target is reached
        }
    }
}
