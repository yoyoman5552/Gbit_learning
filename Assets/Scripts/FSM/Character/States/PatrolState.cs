using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class PatrolState : FSMState
{
    private List<PathNode> pathList;
    private int index;
    // private bool isArrivePoint;
    private Vector2 targetPos;
    public override void Init()
    {
        stateID = FSMStateID.Patrol;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        fsm.m_speed = fsm.walkSpeed;
        //是否完成巡逻
        // fsm.isDonePatrol = false;
        //随机位置的路径获取
        pathList = GridManager.Instance.GetRandomPosOutSelf(fsm.transform.position);
        //初始化
        index = 0;
        targetPos = GridManager.Instance.GetWorldCenterPosition(pathList[index].x, pathList[index].y);
        //巡逻终点
        fsm.patrolPos = GridManager.Instance.GetWorldCenterPosition(pathList[pathList.Count - 1].x, pathList[pathList.Count - 1].y);
        // isArrivePoint = true;
        //        fsm.MovePosition (pathList[index].);
    }
    public override void ActionState(FSMBase fsm)
    {
        if (index >= pathList.Count)
        {
            //fsm.isDonePatrol = true;
            return;
        }
        //如果到达下个点了
        if (Vector3.Distance(targetPos, fsm.transform.position) < 0.03f)
        {
            //            Debug.Log ("arive");
            //isArrivePoint = false;
            index++;
            if (index >= pathList.Count) return;
            targetPos = GridManager.Instance.GetWorldCenterPosition(pathList[index].x, pathList[index].y);
        }
        fsm.MovePosition(targetPos);
        //if (Vector3.Distance (targetPos, fsm.transform.position) < 0.05f) isArrivePoint = true;
    }
    public override void ExitState(FSMBase fsm)
    {
        //停止移动
        fsm.StopPosition();
        //fsm.isDonePatrol = false;
        //      fsm.walkAble=false;
    }
}