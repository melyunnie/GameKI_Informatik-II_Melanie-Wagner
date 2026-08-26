
public class PathNode
{
    private GridPathfinder<PathNode> grid;
    public int x;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public PathNode cameFromNode;
    public PathNode(GridPathfinder<PathNode> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        isWalkable = true;
    }
    public override string ToString()
    {
        return x + "," + z;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
