using UnityEngine;
using UnityEditor; // This is needed to add custom buttons in the Inspector

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData; //Reference to a ScriptableObject holding obstacle data
    public GameObject obstaclePrefab; 
    public float tileSpacing = 1.1f; 
    public float gridSize = 10f; 

    private GameObject[,] obstacleInstances = new GameObject[10, 10]; //2D array to store all generated obstacles

    //method generates obstacles based on the data in ObstacleData
    public void GenerateObstacles()
    {
        ClearObstacles(); //clear any existing obstacles

        //loop through each tile in the grid
        for (int y = 0; y < 10; y++)  //Y loop for rows
        {
            for(int x = 0; x < 10; x++)  //X loop for columns
            {
                int index = y * 10 + x; //Calculate the index based on the grid size

                //Check if the current tile is marked as blocked in the ObstacleData
                if(obstacleData.obstacles[index])
                {
                    Debug.Log("Placing obstacle at grid (" + x + ", " + y + ")."); //Log where we are placing the obstacle  
                    Vector3 position = new Vector3(x * tileSpacing, 0.7f, y * tileSpacing); //Create a position in the 3D world based on the grid's coordinates                  
                    GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity); // Instantiate the obstacle prefab at that position
                    obstacle.name = "Obstacle (" + x + ", " + y + ")"; // Name the obstacle in the scene for easy reference                   
                    obstacle.layer = LayerMask.NameToLayer("obstacleLayer"); //Set the obstacle to a specific layer (for collision and rendering purposes)                    
                    obstacleInstances[x, y] = obstacle; //Store the obstacle in the array
                }
            }
        }
    }

    //This method clears all previously generated obstacles from the grid
    private void ClearObstacles()
    {
        //Loop through all the obstacles in the array
        for(int x = 0; x < 10; x++)
        {
            for(int y = 0; y < 10; y++)
            {
                //If an obstacle exists at this position, destroy it
                if(obstacleInstances[x, y] != null)
                {
                    Destroy(obstacleInstances[x, y]);
                    obstacleInstances[x, y] = null; //Clear the reference
                }
            }
        }
    }
}

//This custom editor adds a button in the Inspector to generate obstacles
[CustomEditor(typeof(ObstacleManager))]
public class ObstacleManagerEditor : Editor
{
    //This is where we define how the custom inspector looks
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();//Draw the default fields from the ObstacleManager script
        ObstacleManager manager = (ObstacleManager)target;  //Get a reference to the ObstacleManager object

        // Add a button that will call GenerateObstacles when clicked
        if (GUILayout.Button("Generate Obstacles"))
        {
            manager.GenerateObstacles();
        }
    }
}
