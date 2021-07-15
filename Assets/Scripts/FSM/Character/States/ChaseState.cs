using System;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class ChaseState : FSMState {
    //FIXME:人物撞墙，追击状态有点问题
    private List<PathNode> pathList;
    private Vector3 nextPos;
    private float defaultAttackArea;
    private bool firstGetAttackArea = true;
    public override void Init () {
        stateID = FSMStateID.Chase;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (FSMBase fsm) {
        if(firstGetAttackArea)
        {
            defaultAttackArea = fsm.attackRadius;
            firstGetAttackArea = false;
        }
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

        //近战攻击的改进：冲刺路径上有障碍物时，缩小攻击检测距离使敌人重新寻找新路径，当路径上没有障碍物时，恢复初始攻击距离
        //射线检测
        
        Vector3 rayDirection = GameManager.Instance.player.transform.position - fsm.transform.position;
        Vector3 detectRayPosition = fsm.transform.position + 0.5f*rayDirection.normalized;
        RaycastHit2D hit = Physics2D.Raycast(detectRayPosition, rayDirection, 10.0f, LayerMask.GetMask("Default"));
        if (hit.collider != null && hit.collider.name == "PlayerCircleDetect")
        {
            fsm.attackRadius = defaultAttackArea;
        }
        else
        {
            fsm.attackRadius = 0.5f;
        }


    }
    public override void ExitState (FSMBase fsm) {
        //fsm.isDoneChase = false;
        fsm.StopPosition ();
        fsm.attackRadius = defaultAttackArea;
    }
}