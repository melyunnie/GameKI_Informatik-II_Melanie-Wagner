using UnityEngine;
using System.Collections.Generic;

public class Pathfinding 
{
    private const int MOVE_STAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private GridPathfinder<PathNode> grid;
    private List<PathNode> openlist;
    private List<PathNode> closedlist;
    public static Pathfinding Instance { get; private set; }
    public Pathfinding(GridPathfinder<PathNode> grid) 
    {
        Instance = this;
        this.grid = grid;
    }

    public List<PathNode> Findpath(int startx, int startz, int endx, int endz)
    {
        PathNode startNode = grid.GetGridObject(startx, startz);
        PathNode endNode = grid.GetGridObject(endx, endz);
        openlist = new List<PathNode>();
        closedlist = new List<PathNode>();     
        openlist.Add(startNode);
       
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                PathNode pathNode = grid.GetGridObject(x, z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
      
        while (openlist.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openlist);
            if (currentNode == endNode) { return CalculatePath(endNode); }
            openlist.Remove(currentNode);
            closedlist.Add(currentNode);
            
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedlist.Contains(neighbourNode))
                { continue;
                }
                if (!neighbourNode.isWalkable) 
                { 
                    closedlist.Add(neighbourNode); 
                    continue; 
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost< neighbourNode.gCost) 
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost=tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                   
                    if (!openlist.Contains(neighbourNode))
                    {
                        openlist.Add(neighbourNode); 
                    }
                }
            }
        }
        return null;
    }
    private List<PathNode> GetNeighbourList(PathNode currentNode) 
    {
        List<PathNode> neighbourList = new List<PathNode>();
        if (currentNode.x-1>=0) 
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.z));
            if (currentNode.z - 1 >= 0) { 
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.z - 1)); 
            }
            if (currentNode.z + 1< grid.GetHeight()) 
            { 
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.z + 1));
            }
        }
        if (currentNode.x + 1 < grid.GetWidth()) 
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.z));
            if (currentNode.z - 1 >= 0)
            { 
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.z - 1)); 
            }
            if (currentNode.z +1 < grid.GetHeight()) 
            { 
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.z + 1)); 
            }
        }
        if (currentNode.z - 1 >=0) 
        { 
            neighbourList.Add(GetNode(currentNode.x, currentNode.z - 1)); 
        }
        if (currentNode.z + 1 < grid.GetHeight()) 
        { 
            neighbourList.Add(GetNode(currentNode.x , currentNode.z + 1)); 
        }
        return neighbourList;
    }

    public PathNode GetNode(int x,int z) 
    {
        return grid.GetGridObject(x, z); 
    }
    private List<PathNode> CalculatePath(PathNode endNode) 
    {
        List<PathNode>path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode!= null) 
        { 
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    } 
    public int CalculateDistanceCost(PathNode a, PathNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST* Mathf.Min(xDistance, zDistance)+MOVE_STAIGHT_COST*remaining;
    }
    private PathNode GetLowestFCostNode(List<PathNode>pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) 
        {
            if (pathNodeList[i].fCost< lowestFCostNode.fCost) 
            { 
                lowestFCostNode=pathNodeList[i];
            }
        } return lowestFCostNode;
    }
    public GridPathfinder<PathNode> GetGrid() { return grid; }
    public void UpdateWalkableNodes()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                PathNode node = grid.GetGridObject(x, z);

                Vector3 worldPos = new Vector3(x * 10f + 5f, 0, z * 10f + 5f);

                bool blocked = Physics.CheckBox(worldPos,new Vector3(4.5f, 1f, 4.5f),Quaternion.identity,LayerMask.GetMask("Object"));
                node.SetIsWalkable(!blocked);
            }
        }
    }

}


        
    

