using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager; 

    //function finds a path from the start position to the target position
    public List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
       
        Vector2Int startGridPos = gridManager.WorldToGridPosition(start); //Convert world positions to grid positions
        Vector2Int targetGridPos = gridManager.WorldToGridPosition(target);
        List<Node> openList = new List<Node>();  //Lists to keep track of nodes we need to check (open list) and nodes we already checked (closed list)
        List<Node> closedList = new List<Node>();
        Node startNode = new Node(startGridPos, null, 0, CalculateDistance(startGridPos, targetGridPos)); //Add the starting position to the open list
        openList.Add(startNode);

        //Keep checking nodes until we find the path or run out of options
        while(openList.Count > 0)
        {
           
            Node currentNode = GetLowestCostNode(openList);  //Find the node with the lowest cost (F cost)

            // If we've reached the target, build the path
            if (currentNode.gridPosition == targetGridPos)
            {
                return BuildPath(currentNode);
            }         
            openList.Remove(currentNode); //Move the current node to the closed list
            closedList.Add(currentNode);           
            List<Vector2Int> neighbors = gridManager.GetNeighbors(currentNode.gridPosition); //Check all the neighbors of the current node
            for (int i = 0; i < neighbors.Count; i++)
            {
                Vector2Int neighborPos = neighbors[i];              
                if(!gridManager.IsTileWalkable(neighborPos) || IsInList(closedList, neighborPos)) //Skip this neighbor if it's blocked or already checked
                {
                    continue;
                } 
                int newGCost = currentNode.gCost + 1; //Calculate the cost to move to this neighbor
                Node neighborNode = new Node(neighborPos, currentNode, newGCost, CalculateDistance(neighborPos, targetGridPos));
                Node existingNode = GetNodeFromList(openList, neighborPos);//Check if the neighbor is in the open list
                if (existingNode == null)
                {           
                    openList.Add(neighborNode);//Add it to the open list if it's not there
                }
                else if(newGCost < existingNode.gCost)
                {
                    
                    existingNode.gCost = newGCost; //Update the node if the new path is shorter
                    existingNode.parent = currentNode;
                }
            }
        }
        
       return new List<Vector3>(); //Return an empty path if no valid path is found
    }

    // Calculate the distance (heuristic) between two grid positions
    private int CalculateDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); //Manhattan distance
    } 
    private List<Vector3> BuildPath(Node node) //Build the path by following the parent nodes
    {
        List<Vector3> path = new List<Vector3>();
        while(node != null)
        {           
            path.Add(gridManager.GridToWorldPosition(node.gridPosition)); //Convert grid position back to world position and add to the path
            node = node.parent;
        }
        path.Reverse(); //Reverse the path so it starts at the beginning
        return path;
    }

    // Find the node with the lowest F cost in the list
    private Node GetLowestCostNode(List<Node> nodes)
    {
        Node lowestCostNode = nodes[0];
        for(int i = 1; i < nodes.Count; i++)
        {
            if(nodes[i].FCost < lowestCostNode.FCost)
            {
                lowestCostNode = nodes[i];
            }
        }
        return lowestCostNode;
    }

    //Check if a node with the given position is in the list
    private bool IsInList(List<Node> nodeList, Vector2Int position)
    {
        for(int i = 0; i < nodeList.Count; i++)
        {
            if(nodeList[i].gridPosition == position)
            {
                return true;
            }
        }
        return false;
    }

    //Get a node with the given position from the list
    private Node GetNodeFromList(List<Node> nodeList, Vector2Int position)
    {
        for(int i = 0; i < nodeList.Count; i++)
        {
            if(nodeList[i].gridPosition == position)
            {
                return nodeList[i];
            }
        }
        return null;
    }

    //Node class to store information about each tile
    private class Node
    {
        public Vector2Int gridPosition; //The position on the grid
        public Node parent; //The previous node in the path
        public int gCost; //Cost from the start to this node
        public int hCost; //Estimated cost to the target
        public int FCost => gCost + hCost; //Total cost

        public Node(Vector2Int gridPosition, Node parent, int gCost, int hCost)
        {
            this.gridPosition = gridPosition;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
    }
}
