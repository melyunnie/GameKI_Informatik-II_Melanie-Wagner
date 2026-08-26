using System;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout : MonoBehaviour
{
    public GameObject floorPrefab;
    GameObject floorInstance;
    public GameObject wallPrefab;
    public Transform spawnParent;
    float prefabBaseSize = 1f;
    float floorYOffset = 0f;
    float wallYOffset = 0f;
    float cellSize;
    float scale; 
    int wallIndex = 0;
    private List<GameObject> wallPool = new List<GameObject>();

    public void MakeLayout(GridPathfinder<PathNode> grid)
    {
        Transform parent;
        if (spawnParent != null)
        {
            parent = spawnParent;
        }
        else
        {
            parent = transform;
        }

        cellSize = GetCellSize(grid);
        Vector3 centerOffset = new Vector3(cellSize / 2f, 0f, cellSize / 2f);
        scale = cellSize / prefabBaseSize;
        DeactivatePoolObjects();
        SpawnFloor(parent, cellSize, grid); 

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                PathNode node = grid.GetGridObject(x, z);
                if (node == null) continue;
                if (node.isWalkable) continue;
                
                if (wallPrefab == null) continue;

                Vector3 worldPos = grid.GetWorldPosition(x, z) + centerOffset + Vector3.up * wallYOffset;

                GameObject instance = WallPool(wallPrefab, parent);
                instance.transform.position = worldPos;
                instance.transform.rotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one * scale;
            }
        }
    }

    private void SpawnFloor(Transform parent, float cellSize, GridPathfinder<PathNode> grid) 
    {
        int width = grid.GetWidth();
        int height = grid.GetHeight();

        float floorWidth = width * cellSize;
        float floorHeight = height * cellSize;

        Vector3 originPos = grid.GetWorldPosition(0, 0);
        Vector3 floorCenter = originPos + new Vector3(floorWidth / 2f, floorYOffset, floorHeight / 2f);

        if (floorInstance == null)
        {
            floorInstance = Instantiate(floorPrefab, floorCenter, Quaternion.identity, parent);
        }
        else
        {
            floorInstance.transform.SetParent(parent);
            floorInstance.transform.position = floorCenter;
            floorInstance.SetActive(true);
        }

        float unityPlaneBaseSize = 10f;
        floorInstance.transform.localScale = new Vector3(floorWidth / unityPlaneBaseSize,1f,floorHeight / unityPlaneBaseSize);
    }
    
    private float GetCellSize(GridPathfinder<PathNode> grid)
    {
        Vector3 a = grid.GetWorldPosition(0, 0);
        Vector3 b = grid.GetWorldPosition(1, 0);
        return Vector3.Distance(a, b);
    }

    private GameObject WallPool(GameObject prefab, Transform parent)
    {
        GameObject instance;
        if (wallIndex < wallPool.Count)
        {
            instance = wallPool[wallIndex];
            instance.transform.SetParent(parent);
            instance.SetActive(true);
        }
        else
        {
            instance = Instantiate(prefab, parent);
            wallPool.Add(instance);
        }
        wallIndex++;
        return instance;
    }

    public void DeactivatePoolObjects()
    {
        if (floorInstance != null)
            floorInstance.SetActive(false);
    
        foreach (GameObject obj in wallPool)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        wallIndex = 0;
    }
}
