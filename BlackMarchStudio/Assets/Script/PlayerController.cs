using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Pathfinding pathfinding; 
    public GridManager gridManager; 
    public float moveSpeed = 5f;    
    private bool isMoving = false; 

    void Update()
    {
        //Check for mouse input each frame
        if(!isMoving)
        {
            HandleMouseInput();
        }
    }

    void HandleMouseInput()
    {
        //Check for a left mouse click
        if(Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Create a ray from the camera to the mouse position          
            if (Physics.Raycast(ray, out RaycastHit hit)) //Check if the ray hits something
            {
                Vector3 targetPosition = hit.transform.position; //Get the clicked position            
                Vector2Int targetGridPosition = gridManager.WorldToGridPosition(targetPosition);//Convert the target position to a grid position              
                if (gridManager.IsTileWalkable(targetGridPosition)) //Check if the tile is walkable
                {
                    MovePlayer(targetPosition); 
                }
                else
                {
                    Debug.Log("Cannot move: The selected tile is not walkable.");
                }
            }
        }
    }

    public void MovePlayer(Vector3 target)
    {
        // Find the path to the target using the Pathfinding script
        List<Vector3> path = pathfinding.FindPath(transform.position, target);

        if(path.Count > 0)
        {
            StartCoroutine(MoveAlongPath(path)); //Move the player along the path
        }
        else
        {
            Debug.Log("Cannot move: No valid path to the target.");
        }
    }

    private IEnumerator MoveAlongPath(List<Vector3> path)
    {
        isMoving = true; //Disable input while the player is moving

        //Use a for loop to iterate through the path
        for(int i = 0; i < path.Count; i++)
        {
            Vector3 nextPosition = path[i]; //Get the next position
            nextPosition.y = transform.position.y; //Keep the player at the same height

            //Move towards the next position
            while (Vector3.Distance(transform.position, nextPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
                yield return null; //Wait for the next frame
            }

            //Snap to the exact position to avoid precision issues
            transform.position = nextPosition;
        }

        isMoving = false; //Re-enable input after movement is done
    }
}
