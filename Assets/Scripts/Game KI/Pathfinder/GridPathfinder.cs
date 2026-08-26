using UnityEngine;
public class GridPathfinder<T>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;

    private T[,] gridArray;
    

    private System.Func<GridPathfinder<T>, int, int, T> createGridObject;

    public GridPathfinder(int width, int height, float cellSize, Vector3 originPosition,
        System.Func<GridPathfinder<T>, int, int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.createGridObject = createGridObject;

        gridArray = new T[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        Vector3 local = worldPosition - originPosition;
        x = Mathf.FloorToInt(local.x / cellSize);
        z = Mathf.FloorToInt(local.z / cellSize);
    }

    public void SetValue(int x, int z, T value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetValue(x, z, value);
    }

    public T GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        return default;
    }

    public T GetValue(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return GetValue(x, z);
    }
    public T GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }

        return default;
    }
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

}


