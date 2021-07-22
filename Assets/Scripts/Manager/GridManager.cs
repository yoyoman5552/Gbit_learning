using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class GridManager : MonoBehaviour
{
    [Header("公共变量")]
    [Tooltip("网格大小")]
    public float cellsize = 1;
    [Tooltip("是否debug画线")]
    public bool isDrawLine;

    [Header("私有变量")]
    //网格的左下角
    [HideInInspector]
    public Transform LeftDownTF;
    //网格的右上角
    [HideInInspector]
    public Transform RightUpTF;
    //A*算法
    private PathFinding path;
    //网格
    private MyGrid<PathNode> grid;
    //网格宽高
    private int width, height;

    /// <summary>
    /// 单例模式
    /// </summary>
    public static GridManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Init()
    {
        //初始化 宽、高、网格、A*算法
        width = Mathf.RoundToInt((RightUpTF.position.x - LeftDownTF.position.x) / cellsize);
        height = Mathf.RoundToInt((RightUpTF.position.y - LeftDownTF.position.y) / cellsize);
        MyGrid<PathNode> pathGrid = new MyGrid<PathNode>(width, height, cellsize, LeftDownTF.position, (MyGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        InitGrid(pathGrid);
        SetObstacles();
        if (isDrawLine)
        {
            DrawLine();
        }
    }
    public void InitGrid(MyGrid<PathNode> pathGrid)
    {
        grid = pathGrid;
        path = new PathFinding(grid);
    }
    public void SetObstacles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (IsAWall(x, y))
                {
                    //                     Debug.Log (grid.GetTGridObject (x, y).ToString ());
                    grid.GetTGridObject(x, y).SetIsThroughable(false);
                }
                else grid.GetTGridObject(x, y).SetIsThroughable(true);
            }
        }

    }
    public void DrawLine()
    {
        if (MyGrid<PathNode>.debugTextArray != null)
        {
            Debug.Log("destroy debugTextArray");
            for (int x = 0; x < MyGrid<PathNode>.debugTextArray.GetLength(0); x++)
            {
                for (int y = 0; y < MyGrid<PathNode>.debugTextArray.GetLength(1); y++)
                {
                    Destroy(MyGrid<PathNode>.debugTextArray[x, y]);
                }
            }
        }
        GameObject parent = new GameObject(GameManager.Instance.currentRoom.name + "DebugGrid");
        parent = Instantiate(parent, Vector2.zero, Quaternion.identity);
        MyGrid<PathNode>.debugTextArray = new TextMesh[grid.GetWidth(), grid.GetHeight()];
        int fontsize = 10;
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                if (grid.GetTGridObject(x, y).GetIsThroughable())
                {

                    MyGrid<PathNode>.debugTextArray[x, y] = EveryFunction.CreateWorldText(grid.gridArray[x, y].ToString(), null, grid.GetWorldPosition(x, y) + new Vector3(cellsize, cellsize) * 0.5f, fontsize, Color.green, TextAnchor.MiddleCenter, TextAlignment.Center);
                }
                else
                {
                    MyGrid<PathNode>.debugTextArray[x, y] = EveryFunction.CreateWorldText(grid.gridArray[x, y].ToString(), null, grid.GetWorldPosition(x, y) + new Vector3(cellsize, cellsize) * 0.5f, fontsize, Color.red, TextAnchor.MiddleCenter, TextAlignment.Center);

                }
                MyGrid<PathNode>.debugTextArray[x, y].transform.SetParent(parent.transform);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.gray, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.gray, 100f);
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(width, 0), grid.GetWorldPosition(width, height), Color.gray, 100f);
        Debug.DrawLine(grid.GetWorldPosition(0, height), grid.GetWorldPosition(width, height), Color.gray, 100f);
    }
    public List<PathNode> FindPath(Vector3 oriPos, Vector3 targetPos)
    {
        int startX, startY, endX, endY;
        grid.GetXY(oriPos, out startX, out startY);
        grid.GetXY(targetPos, out endX, out endY);
        //        Debug.Log ("起点：" + new Vector2 (startX, startY) + ",终点：" + new Vector2 (endX, endY));
        return path.FindPath(startX, startY, endX, endY);
    }
    public bool IsAWall(int x, int y)
    {
        var hitColliders = Physics2D.OverlapCircleAll(grid.GetWorldCenterPosition(x, y), grid.GetCellsize() / 2f);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Wall") || collider.CompareTag("Breakable"))
            {
                return true;
            }
        }
        return false;
    }
    public Vector3 GetXY(Vector3 pos)
    {
        int x, y;
        grid.GetXY(pos, out x, out y);
        return new Vector3(x, y);
    }
    public Vector3 GetWorldPos(int x, int y)
    {
        return grid.GetWorldPosition(x, y);
    }
    public Vector3 GetWorldCenterPosition(int x, int y)
    {
        return grid.GetWorldCenterPosition(x, y);
    }
    public List<PathNode> GetRandomPosOutSelf(Vector3 selfPos, float radius = 100f)
    {
        Vector3 tarPos;
        List<PathNode> pathList;
        do
        {
            tarPos = GetRandomPos();
            pathList = FindPath(selfPos, tarPos);
        } while (Vector3.Distance(tarPos, selfPos) < grid.GetCellsize() || Vector3.Distance(tarPos, selfPos) > radius || pathList == null || pathList.Count > radius + 4);
        return pathList;
    }
    public Vector3 GetRandomPos()
    {
        float randomX, randomY;
        float limit = 1f;
        randomX = Random.Range(LeftDownTF.position.x + limit, RightUpTF.position.x - limit);
        randomY = Random.Range(LeftDownTF.position.y + limit, RightUpTF.position.y - limit);
        return new Vector3(randomX, randomY);
    }
}