using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class Grid<TGridObject>
{
    //是否要画线
    private bool isDrawLine = false;
    private int width;
    private int height;
    private float cellsize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3 oriPosition;
    public Grid(int width, int height, float cellsize, Vector3 oriPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;
        this.oriPosition = oriPosition;
        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
                if (isDrawLine)
                {
                    debugTextArray[x, y] = EveryFunction.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellsize, cellsize) * 0.5f, 45, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
        }
        if (isDrawLine)
        {
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    /// <summary>
    /// 获取世界坐标
    /// </summary>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellsize + oriPosition;
    }
    /// <summary>
    /// 获取世界中心坐标
    /// </summary>
    public Vector3 GetWorldCenterPosition(int x, int y)
    {
        return new Vector3(x, y) * cellsize + oriPosition + new Vector3(cellsize, cellsize) * 0.5f;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - oriPosition.x) / cellsize);
        y = Mathf.FloorToInt((worldPosition.y - oriPosition.y) / cellsize);
    }
    public void SetTGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }
    public void SetTGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetTGridObject(x, y, value);
    }
    public TGridObject GetTGridObject(int x, int y)
    {
        /*         Debug.Log ("width:" + width + ",height:" + height);
                Debug.Log ("怪物位置：" + x + "," + y);
         */
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else return default(TGridObject);
    }
    public TGridObject GetTGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetTGridObject(x, y);
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellsize()
    {
        return cellsize;
    }
}
