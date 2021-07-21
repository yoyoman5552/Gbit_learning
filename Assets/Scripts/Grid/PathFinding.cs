using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A*算法
/// </summary>
public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private MyGrid<PathNode> grid;
    private List<PathNode> oppenList;
    private List<PathNode> closeList;
    private int maxX, maxY;
    /*     public PathFinding (int width, int height, Vector3 oriPosition) {
            grid = new Grid<PathNode> (width, height, 1f,
                oriPosition, (Grid<PathNode> g, int x, int y) => new PathNode (g, x, y));
        } */
    public PathFinding(MyGrid<PathNode> grid)
    {
        this.grid = grid;
        InitData();
    }

    private void InitData()
    {
        maxX = grid.GetWidth();
        maxY = grid.GetHeight();
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //如果不在grid内，也返回找不到路径
        if (endX < 0 || endY < 0 || endX >= maxX || endY >= maxY) return null;

        PathNode startNode = GetNode(startX, startY);
        PathNode endNode = GetNode(endX, endY);
        oppenList = new List<PathNode> { startNode };
        closeList = new List<PathNode>();
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                //每个点初始化
                PathNode pathNode = grid.GetTGridObject(x, y);
                pathNode.gcost = int.MaxValue;
                pathNode.CaculateFcost();
                pathNode.cameFromNode = null;
            }
        }
        //初始点初始化
        startNode.gcost = 0;
        startNode.hcost = CaculateDistanceCost(startNode, endNode);
        startNode.CaculateFcost();

        while (oppenList.Count > 0)
        {
            PathNode currentNode = GetLowerFcostNode(oppenList);
            if (currentNode == endNode)
            {
                //到达终点
                return CaculatePath(currentNode);
            }
            if (!currentNode.GetIsThroughable())
            {
                //如果当前点是墙
                oppenList.Remove(currentNode);
                closeList.Add(currentNode);
                continue;
            }
            oppenList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                //如果neighbourNode已经算过了
                if (closeList.Contains(neighbourNode)) continue;
                //判断新Gcost是否比旧Gcost小
                int tentativeGcost = currentNode.gcost + CaculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGcost < neighbourNode.gcost)
                {
                    neighbourNode.gcost = tentativeGcost;
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.hcost = CaculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CaculateFcost();
                    if (!oppenList.Contains(neighbourNode))
                    {
                        oppenList.Add(neighbourNode);
                    }
                }
            }
        }
        //找不到路径了
        return null;
    }
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        int curX = currentNode.x;
        int curY = currentNode.y;
        //判断点原则：判断是否是墙壁，而对角点需判断当前点到对角点的两侧是不是墙壁（否则人物移动的时候会卡住）
        if (curX - 1 >= 0)
        {
            //left side
            if (CheckThroughable(curX - 1, curY))
            {
                //left
                neighbourList.Add(GetNode(curX - 1, curY));
                //left down
                if (curY - 1 >= 0)
                {
                    if (CheckThroughable(curX - 1, curY - 1) && CheckThroughable(curX, curY - 1))
                    {
                        neighbourList.Add(GetNode(curX - 1, curY - 1));
                    }
                }
                //left up
                if (curY + 1 < maxY)
                {
                    if (CheckThroughable(curX - 1, curY + 1) && CheckThroughable(curX, curY + 1))
                    {
                        neighbourList.Add(GetNode(curX - 1, curY + 1));
                    }
                }
            }
        }
        if (curX + 1 < maxX)
        {
            //right side
            if (CheckThroughable(curX + 1, curY))
            {
                //right
                neighbourList.Add(GetNode(curX + 1, curY));
                //right down
                if (curY - 1 >= 0)
                {
                    if (CheckThroughable(curX + 1, curY - 1) && CheckThroughable(curX, curY - 1))
                    {
                        neighbourList.Add(GetNode(curX + 1, curY - 1));
                    }
                }
                //right up
                if (curY + 1 < maxY)
                {
                    if (CheckThroughable(curX + 1, curY + 1) && CheckThroughable(curX, curY + 1))
                    {
                        neighbourList.Add(GetNode(curX + 1, curY + 1));
                    }
                }
            }
        }
        //down
        if (curY - 1 >= 0 && CheckThroughable(curX, curY - 1)) neighbourList.Add(GetNode(curX, curY - 1));
        //up
        if (curY + 1 < grid.GetHeight() && CheckThroughable(curX, curY + 1)) neighbourList.Add(GetNode(curX, curY + 1));
        return neighbourList;
    }
    private bool CheckThroughable(int x, int y)
    {
        //检测这个点是否能够通过
        return GetNode(x, y).GetIsThroughable();
    }
    private List<PathNode> CaculatePath(PathNode endNode)
    {
        List<PathNode> nodePath = new List<PathNode>();
        nodePath.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            currentNode = currentNode.cameFromNode;
            nodePath.Add(currentNode);
        }
        nodePath.Reverse();
        return nodePath;
    }
    private PathNode GetLowerFcostNode(List<PathNode> pathNodeList)
    {
        PathNode lowerFcostNode = pathNodeList[0];
        foreach (PathNode dnode in pathNodeList)
        {
            if (dnode.fcost < lowerFcostNode.fcost)
                lowerFcostNode = dnode;
        }
        return lowerFcostNode;
    }
    private int CaculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    private PathNode GetNode(int x, int y)
    {
        return grid.GetTGridObject(x, y);
    }
    public MyGrid<PathNode> GetGrid()
    {
        return grid;
    }
    public void SetGrid(MyGrid<PathNode> grid)
    {
        this.grid = grid;
    }
}