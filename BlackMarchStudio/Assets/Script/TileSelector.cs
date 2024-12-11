using UnityEngine;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    public Text positionText; // Reference to the UI Text element

    void Update()
    {
        //Perform a raycast from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            //Check if the object hit by the raycast has a TileInfo script
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if(tileInfo != null)
            {
                //Debug.Log("Hovered over tile at: " + tileInfo.gridPosition);
                //Update the UI with the grid position of the tile when hovered
                positionText.text = "Grid Position: (" + tileInfo.gridPosition.x + ", " + tileInfo.gridPosition.y + ")";
                //Debug.Log("Hovered over tile at: " + tileInfo.gridPosition);
            }
        }
        else
        {
            //Clear the UI text if no tile is under the mouse
            positionText.text = "Grid Position: (None)";
        }
    }
}
