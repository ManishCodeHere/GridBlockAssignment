using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleData))]
public class GridEditorTool : Editor
{
    private const int gridSize = 10; 

    // This method runs when the inspector is drawn
    public override void OnInspectorGUI()
    {
        
        ObstacleData data = (ObstacleData)target; //Get a reference to the ObstacleData object that we are editing
        // Loop through each row of the grid (Y axis)
        for (int y = 0; y < gridSize; y++)
        {
            
            EditorGUILayout.BeginHorizontal(); //Begin a horizontal group to display buttons on the same line
            // Loop through each column of the grid (X axis)
            for (int x = 0; x < gridSize; x++)
            {
                
                int index = y * gridSize + x; //Calculate the index of the current grid cell

                //Create a toggle button for each grid cell
                //The button will show a checkmark if the corresponding obstacle is true
                //And it will remove the checkmark if the obstacle is false
                data.obstacles[index] = GUILayout.Toggle(data.obstacles[index], "", GUILayout.Width(20), GUILayout.Height(20));
            }

            
            EditorGUILayout.EndHorizontal(); // End the horizontal group for this row
        }

        // Check if any changes have been made
        if(GUI.changed)
        {
            EditorUtility.SetDirty(data); // Mark the ObstacleData as dirty to save the changes
        }
    }
}
