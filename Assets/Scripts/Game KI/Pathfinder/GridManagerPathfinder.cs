using UnityEngine;
[ExecuteAlways]
public class GridManagerPathfinder : MonoBehaviour
{
    [SerializeField]  int width = 20;
    [SerializeField]  int height = 20;
    [SerializeField]  float cellSize = 10f;
    private GridPathfinder<PathNode> grid;
    private Pathfinding pathfinding;
    public static GridManagerPathfinder Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        grid = new GridPathfinder<PathNode>(width,height,cellSize,transform.position,(g, x, z) => new PathNode(g, x, z));
        pathfinding = new Pathfinding(grid);
    }
    public Pathfinding GetPathfinding()
    {
        return pathfinding;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 origin = transform.position;
        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(origin + new Vector3(x * cellSize, 0, 0), origin + new Vector3(x * cellSize, 0, height * cellSize));
        }
        for (int z = 0; z <= height; z++)
        {
            Gizmos.DrawLine(origin + new Vector3(0, 0, z * cellSize),origin + new Vector3(width * cellSize, 0, z * cellSize));
        }
    }
}
