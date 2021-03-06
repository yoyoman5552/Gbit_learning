using System;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class ChaseState : FSMState
{
    //FIXME:人物撞墙，追击状态有点问题
    private List<PathNode> pathList;
    private Vector3 nextPos;
    private float defaultAttackArea;
    private bool firstGetAttackArea = true;

    //随机偏移位置
    float disX;
    float disY;
    public override void Init()
    {
        stateID = FSMStateID.Chase;
        //        throw new System.NotImplementedException();

    }


    //玩家与敌人间距离
    private float Distance;
    public override void EnterState(FSMBase fsm)
    {
        //设置随机数防止敌人追击时合在一起
        disX = UnityEngine.Random.Range(-0.5f, 0.5f);
        disY = UnityEngine.Random.Range(-0.5f, 0.5f);




        if (firstGetAttackArea)
        {
            defaultAttackArea = fsm.attackRadius;
            firstGetAttackArea = false;
        }
        //        Debug.Log("chase state in");
        //fsm.isDoneChase = false;
        fsm.m_speed = fsm.chaseSpeed;
        pathList = GridManager.Instance.FindPath(fsm.transform.position, fsm.targetTF.position);
        if (pathList == null) return;
        if (pathList.Count > 1)
        {
            nextPos = GridManager.Instance.GetWorldCenterPosition(pathList[1].x, pathList[1].y);
            fsm.MovePosition(nextPos);
        }
        //        Debug.Log("pathList count:" + pathList.Count + ",nextPos:" + nextPos);
        /*  else {
                    fsm.isDoneChase = true;
                }
         */
    }
    public override void ActionState(FSMBase fsm)
    {


        //偏移坐标
        Vector3 targetPosition = fsm.targetTF.position;
        targetPosition.x += disX;
        targetPosition.y += disY;

        //如果到达位置
        pathList = GridManager.Instance.FindPath(fsm.transform.position, targetPosition);
        //没有方法抵达:说明自己卡住了，或者玩家卡住了
        //从该敌人最开始的地方判断能不能到达玩家
        if (pathList == null)
            pathList = GridManager.Instance.FindPath(fsm.OriginPos, targetPosition);
        if (pathList == null)
        {
            fsm.StopPosition();
            return;
        }
        if (pathList.Count <= 1)
        {
            //fsm.isDoneChase = true;
            return;
        }
        nextPos = GridManager.Instance.GetWorldCenterPosition(pathList[1].x, pathList[1].y);
        fsm.MovePosition(nextPos);


        //近战攻击的改进：冲刺路径上有障碍物时，缩小攻击检测距离使敌人重新寻找新路径，当路径上没有障碍物时，恢复初始攻击距离
        //射线检测
        //近战攻击状态为冲刺状态时检测
        if (rayDetect(fsm))
        {
            fsm.attackRadius = defaultAttackArea;
        }
        else
        {
            fsm.attackRadius = 0;
        }




    }

    public override void ExitState(FSMBase fsm)
    {
        //fsm.isDoneChase = false;
        fsm.StopPosition();
        fsm.attackRadius = defaultAttackArea;


    }

    private float detectDistance(FSMBase fsm)
    {
        return (Vector3.Distance(fsm.targetTF.position, fsm.transform.position));
    }

    private bool rayDetect(FSMBase fsm)
    {
        Vector3 rayDirection = fsm.targetTF.position - fsm.transform.position;
        Vector3 detectRayPosition = fsm.transform.position + rayDirection.normalized;
        RaycastHit2D hit = Physics2D.Raycast(detectRayPosition, rayDirection, detectDistance(fsm), LayerMask.GetMask("Default"));
        if (hit.collider != null && hit.collider.name == "PlayerCircleDetect")
        {
            return true;
        }
        else
        {
            /*             if (hit.collider != null)
                            Debug.Log("hit:" + hit.collider.name);
             */
            return false;
        }
    }
}