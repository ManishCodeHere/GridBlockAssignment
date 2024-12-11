using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Grid/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public bool[] obstacles = new bool[100]; // 10x10 grid (10 * 10 = 100)
}
