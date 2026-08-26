using UnityEngine;

public static class WalkableAreas 
{
    public static void InitializeAllWalls(GridPathfinder<PathNode> grid)
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            { 
                SetWalkable(grid, x, z, false); 
            }
        }
    }

    public static void SetWalkable(GridPathfinder<PathNode> grid, int x, int z, bool walkable)
    {
        if (x < 0 || z < 0 || x >= grid.GetWidth() || z >= grid.GetHeight())
        { return; }

        PathNode node = grid.GetGridObject(x, z);
        if (node != null)
        {
            node.SetIsWalkable(walkable);
        }
    }
}
