using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour 
{
   
    [SerializeField] int roomCount = 10;
    [SerializeField] Vector2Int roomMinSize = new Vector2Int(4, 4);
    [SerializeField] Vector2Int roomMaxSize = new Vector2Int(10, 8);
    [Range(0, 50)][SerializeField] int roomPlacementAttempts ;
    [SerializeField] int corridorWidth = 1;
    [Range(0f, 1f)][SerializeField] float extraLoopConnectionChance ;

    [SerializeField] bool useSeed = false;
    [SerializeField] int seed;
    [SerializeField] List<Transform> Waypoints= new List<Transform>();

    public MapLayout layout;

    private List<RectInt> rooms = new List<RectInt>();
    private void Start()
    {
        GenerateMap();
    }
    private void Update()
    {
        
    }
    public void GenerateMap()
    {
        GridPathfinder<PathNode> grid = GridManagerPathfinder.Instance.GetPathfinding().GetGrid();
        System.Random random;
        if (useSeed)
        {
            random = new System.Random(seed);
        }
        else
        {
            random = new System.Random();
        }

        WalkableAreas.InitializeAllWalls(grid);
        rooms = RoomMaker.PlaceRooms(grid, roomCount, roomMinSize, roomMaxSize, roomPlacementAttempts, random);
        RoomConector.ConnectRooms(grid, rooms, corridorWidth, extraLoopConnectionChance, random);
        List<Vector2Int> WayPointCoords = ConvertToGridCoords(grid, Waypoints);
        RoomConector.ConnectWayPoints(grid, rooms, WayPointCoords, corridorWidth, random);

        if (layout != null)
            layout.MakeLayout(grid);
    }

    private List<Vector2Int> ConvertToGridCoords(GridPathfinder<PathNode> grid, List<Transform> objects)
    {
        List<Vector2Int> coords = new List<Vector2Int>();
        foreach (Transform obj in objects)
        {
            if (obj == null) continue;

            grid.GetXZ(obj.position, out int x, out int z);
            coords.Add(new Vector2Int(x, z));
        }
        return coords;
    }
    public void Reset()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
