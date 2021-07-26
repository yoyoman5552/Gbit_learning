using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class PatrolState : FSMState {
    private List<PathNode> pathList;
    private int index;
    // private bool isArrivePoint;
    private Vector2 targetPos;
    public override void Init () {
        stateID = FSMStateID.Patrol;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (FSMBase fsm) {
        //如果是第一次开始巡逻，就记录当前的默认位置
        fsm.m_speed = fsm.walkSpeed;
        //是否完成巡逻
        // fsm.isDonePatrol = false;
        //随机位置的路径获取
        pathList = GridManager.Instance.GetRandomPosOutSelf (fsm.transform.position, fsm.OriginPos, fsm.patrolRadius);
        //==null 说明现在处在一个无法行动的地方
        if (pathList == null) {
            pathList = GridManager.Instance.GetRandomPosOutSelf (fsm.OriginPos, fsm.OriginPos, fsm.patrolRadius);
        }
        //如果无论如何都没有路的话
        if (pathList == null) {
            fsm.ChangeActiveState (FSMStateID.Patrol);
        }
        //        Debug.Log("find pos:" + pathList[pathList.Count - 1]);
        //初始化
        index = 1;
        //        Debug.Log ("gridManager:" + GridManager.Instance);
        targetPos = GridManager.Instance.GetWorldCenterPosition (pathList[index].x, pathList[index].y);
        //巡逻终点
        fsm.patrolPos = GridManager.Instance.GetWorldCenterPosition (pathList[pathList.Count - 1].x, pathList[pathList.Count - 1].y);
        // isArrivePoint = true;
        //        fsm.MovePosition (pathList[index].);
    }
    public override void ActionState (FSMBase fsm) {
        if (index >= pathList.Count) {
            //fsm.isDonePatrol = true;
            //如果到达位置，或者到不搭位置
            fsm.patrolPos = fsm.transform.position;
            return;
        }
        //如果到达下个点了
        if (Vector3.Distance (targetPos, fsm.transform.position) < 0.03f) {
            //            Debug.Log ("arive");
            //isArrivePoint = false;
            index++;
            if (index >= pathList.Count) return;
            targetPos = GridManager.Instance.GetWorldCenterPosition (pathList[index].x, pathList[index].y);
        }
        //加上一个随机值
        Vector2 randomDir = EveryFunction.GetRandomDir () * 0.01f;
        fsm.MovePosition (targetPos + randomDir);
        //if (Vector3.Distance (targetPos, fsm.transform.position) < 0.05f) isArrivePoint = true;
    }
    public override void ExitState (FSMBase fsm) {
        //停止移动
        fsm.StopPosition ();
        //fsm.isDonePatrol = false;
        //      fsm.walkAble=false;
    }
}