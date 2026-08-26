using System.Collections.Generic;
using UnityEngine;

public static class RoomConector
{
    public static void ConnectRooms(GridPathfinder<PathNode> grid, List<RectInt> rooms, int corridorWidth, float extraLoopConnectionChance, System.Random random)
    {
        if (rooms.Count < 2) { return; }

        List<(int a, int b)> edges = BuildMinimumSpanningTree(rooms, random);
        AddExtraLoopEdges(rooms, edges, extraLoopConnectionChance, random);

        foreach ((int a, int b) in edges)
        {
            RoomMaker.CarveLShaped(grid, RoomCenter(rooms[a]), RoomCenter(rooms[b]), corridorWidth, random);
        }
    }

    private static List<(int a, int b)> BuildMinimumSpanningTree(List<RectInt> rooms, System.Random random)
    {
        HashSet<int> connected = new HashSet<int> { 0 };
        List<(int a, int b)> edges = new List<(int a, int b)>();

        while (connected.Count < rooms.Count)
        {
            int bestA = -1, bestB = -1;
            float bestDist = float.MaxValue;

            foreach (int a in connected)
            {
                for (int b = 0; b < rooms.Count; b++)
                {
                    if (connected.Contains(b)) continue;

                    float dist = Vector2.Distance(RoomCenter(rooms[a]), RoomCenter(rooms[b]));
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            edges.Add((bestA, bestB));
            connected.Add(bestB);
        }

        return edges;
    }

    private static void AddExtraLoopEdges(List<RectInt> rooms, List<(int a, int b)> edges, float chance, System.Random random)
    {
        for (int a = 0; a < rooms.Count; a++)
            for (int b = a + 1; b < rooms.Count; b++)
            {
                if (random.NextDouble() < chance)
                    edges.Add((a, b));
            }
    }

    public static Vector2 RoomCenter(RectInt room)
    {
        return new Vector2(room.x + room.width / 2f, room.y + room.height / 2f);
    }

    public static void ConnectWayPoints(GridPathfinder<PathNode> grid, List<RectInt> rooms, List<Vector2Int> waypoints, int corridorWidth, System.Random random)
    {
        if (rooms.Count == 0) 
        {
            return; 
        }

        foreach (Vector2Int point in waypoints)
        {
            WalkableAreas.SetWalkable(grid, point.x, point.y, true);
            RectInt nearestRoom = FindNearestRoom(point, rooms);
            Vector2 roomCenter = RoomConector.RoomCenter(nearestRoom);

            RoomMaker.CarveLShaped(grid, point, roomCenter, corridorWidth, random);
        }
    }

    private static RectInt FindNearestRoom(Vector2Int point, List<RectInt> rooms)
    {
        RectInt nearest = rooms[0];
        float bestDist = float.MaxValue;

        foreach (RectInt room in rooms)
        {
            float dist = Vector2.Distance(point, RoomConector.RoomCenter(room));
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest = room;
            }
        }

        return nearest;
    }
}
