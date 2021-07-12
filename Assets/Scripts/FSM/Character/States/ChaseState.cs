using System;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class ChaseState : FSMState {
    //FIXME:人物撞墙，追击状态有点问题
    private List<PathNode> pathList;
    private Vector3 nextPos;
    public override void Init () {
        stateID = FSMStateID.Chase;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (FSMBase fsm) {
        Debug.Log ("chase state in");
        //fsm.isDoneChase = false;
        fsm.m_speed = fsm.chaseSpeed;
        pathList = GridManager.Instance.FindPath (fsm.transform.position, fsm.targetTF.position);
        if (pathList == null) return;
        if (pathList.Count > 1) {
            nextPos = GridManager.Instance.GetWorldCenterPosition (pathList[1].x, pathList[1].y);
            fsm.MovePosition (nextPos);
        }
        Debug.Log ("pathList count:" + pathList.Count + ",nextPos:" + nextPos);
        /*  else {
                    fsm.isDoneChase = true;
                }
         */
    }
    public override void ActionState (FSMBase fsm) {
        //如果到达位置
        pathList = GridManager.Instance.FindPath (fsm.transform.position, fsm.targetTF.position);
        //没有方法抵达
        if (pathList == null) {
            fsm.StopPosition ();
            return;
        }
        if (pathList.Count <= 1) {
            //fsm.isDoneChase = true;
            return;
        }
        nextPos = GridManager.Instance.GetWorldCenterPosition (pathList[1].x, pathList[1].y);
        fsm.MovePosition (nextPos);
    }
    public override void ExitState (FSMBase fsm) {
        //fsm.isDoneChase = false;
        fsm.StopPosition ();
    }
}