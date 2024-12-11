using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button generateObstaclesButton; // Reference to the button
    public ObstacleManager obstacleManager; // Reference to the ObstacleManager script

    void Start()
    {
        // Ensure the Button and ObstacleManager are assigned in the Inspector
        if (generateObstaclesButton != null && obstacleManager != null)
        {
            // Add a listener to the button click event
            generateObstaclesButton.onClick.AddListener(GenerateObstacles);
        }
    }

    // Method to call the GenerateObstacles function from the ObstacleManager script
    void GenerateObstacles()
    {
        obstacleManager.GenerateObstacles(); // Trigger the generation of obstacles
    }
}
