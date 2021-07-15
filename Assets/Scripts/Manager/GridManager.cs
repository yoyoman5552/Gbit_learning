using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{ 
    [Header("公共变量")]
    [Tooltip("网格大小")]
    public float cellsize = 1;

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
    private Grid<PathNode> grid;
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
        Grid<PathNode> pathGrid = new Grid<PathNode>(width, height, cellsize, LeftDownTF.position, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        InitGrid(pathGrid);
        SetObstacles();
    }
    public void InitGrid(Grid<PathNode> pathGrid)
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
        RaycastHit2D hitInfo = Physics2D.Raycast(grid.GetWorldCenterPosition(x, y), Vector3.forward, grid.GetCellsize());
        if (hitInfo.collider != null && (hitInfo.collider.CompareTag("Wall") || hitInfo.collider.CompareTag("Interactive"))) return true;
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
    public List<PathNode> GetRandomPosOutSelf(Vector3 selfPos)
    {
        Vector3 tarPos;
        List<PathNode> pathList;
        do
        {
            tarPos = GetRandomPos();
            pathList = FindPath(selfPos, tarPos);
        } while (Vector3.Distance(tarPos, selfPos) < 1f || pathList == null);
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