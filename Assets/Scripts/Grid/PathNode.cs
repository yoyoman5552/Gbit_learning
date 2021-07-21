using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x;
    public int y;
    public int fcost;
    public int gcost;
    public int hcost;
    public PathNode cameFromNode;
    private bool isThroughable;
    // private Grid<PathNode> grid;
    public PathNode(MyGrid<PathNode> grid, int x, int y, bool isThroughable = true)
    {
        //        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isThroughable = isThroughable;
    }
    public void CaculateFcost()
    {
        fcost = gcost + hcost;
    }
    public override string ToString()
    {
        return x + "," + y;
    }
    public void SetIsThroughable(bool dflag)
    {
        isThroughable = dflag;
    }
    public bool GetIsThroughable()
    {
        return isThroughable;
    }
    /*     public Vector3 GetWorldPosition(){
            return grid.GetWorldPosition(x,y);
        }
        public Vector3 GetWorldCenterPosition(){
            return grid.GetWorldCenterPosition(x,y);
        }
     */
}