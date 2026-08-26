using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomMaker : MonoBehaviour 
{
    public static List<RectInt> PlaceRooms(GridPathfinder<PathNode> grid, int roomCount, Vector2Int roomMinSize, Vector2Int roomMaxSize, int roomPlacementAttempts, System.Random random)
    {
        List<RectInt> rooms = new List<RectInt>();
        int gridWidth = grid.GetWidth();
        int gridHeight = grid.GetHeight();

        for (int i = 0; i < roomCount; i++)
        {
            for (int attempt = 0; attempt < roomPlacementAttempts; attempt++)
            {
                int w = random.Next(roomMinSize.x, roomMaxSize.x + 1);
                int h = random.Next(roomMinSize.y, roomMaxSize.y + 1);
                int x = random.Next(1, Math.Max(2, gridWidth - w - 1));
                int z = random.Next(1, Math.Max(2, gridHeight - h - 1));

                RectInt placedRoom = new RectInt(x, z, w, h);

                if (CheckRoomOverlap(placedRoom, rooms))
                    continue;

                rooms.Add(placedRoom);
                MakeWalkableFloor(grid, placedRoom);
                break;
            }
        }

        return rooms;
    }
    private static bool CheckRoomOverlap(RectInt placedRoom, List<RectInt> rooms)
    {
        RectInt padded = new RectInt(placedRoom.x - 1, placedRoom.y - 1, placedRoom.width + 2, placedRoom.height + 2);
        foreach (RectInt room in rooms)
        {
            if (padded.Overlaps(room))
                return true;
        }
        return false;
    }
    private static void MakeWalkableFloor(GridPathfinder<PathNode> grid, RectInt room)
    {
        for (int x = room.xMin; x < room.xMax; x++)
            for (int z = room.yMin; z < room.yMax; z++)
                WalkableAreas.SetWalkable(grid, x, z, true);
    }
    public static void CarveLShaped(GridPathfinder<PathNode> grid, Vector2 from, Vector2 to, int corridorWidth, System.Random random)
    {
        int x1 = Mathf.RoundToInt(from.x);
        int z1 = Mathf.RoundToInt(from.y);
        int x2 = Mathf.RoundToInt(to.x);
        int z2 = Mathf.RoundToInt(to.y);

        if (random.Next(2) == 0)
        {
            CarveHorizontal(grid, x1, x2, z1, corridorWidth);
            CarveVertical(grid, z1, z2, x2, corridorWidth);
        }
        else
        {
            CarveVertical(grid, z1, z2, x1, corridorWidth);
            CarveHorizontal(grid, x1, x2, z2, corridorWidth);
        }
    }
    private static void CarveHorizontal(GridPathfinder<PathNode> grid, int xFrom, int xTo, int z, int corridorWidth)
    {
        int min = Mathf.Min(xFrom, xTo);
        int max = Mathf.Max(xFrom, xTo);
        for (int x = min; x <= max; x++)
        { 
            CarveCell(grid, x, z, corridorWidth); 
        }
    }
    private static void CarveVertical(GridPathfinder<PathNode> grid, int zFrom, int zTo, int x, int corridorWidth)
    {
        int min = Mathf.Min(zFrom, zTo);
        int max = Mathf.Max(zFrom, zTo);
        for (int z = min; z <= max; z++)
        { 
            CarveCell(grid, x, z, corridorWidth); 
        }
    }
    private static void CarveCell(GridPathfinder<PathNode> grid, int x, int z, int corridorWidth)
    {
        int half = corridorWidth / 2;
        for (int i = -half; i <= half; i++)
        {
            for (int j = -half; j <= half; j++)
            {
                WalkableAreas.SetWalkable(grid, x + i, z + j, true);
            }
        }
    }
}
