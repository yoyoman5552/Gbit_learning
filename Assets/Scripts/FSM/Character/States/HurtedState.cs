using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//闲置状态
public class HurtedState : FSMState
{
    //idle时间
    private float hurtedTimer;
    private float originSpeed;
    private bool added;
    //一定要初始化stateID,而且要初始化对
    public override void Init()
    {
        stateID = FSMStateID.Hurted;
    }
    public override void EnterState(FSMBase fsm)
    {
        base.EnterState(fsm);
        originSpeed = fsm.m_speed;
        fsm.m_speed = fsm.HurtedSpeed;
        fsm.moveVelocity = fsm.hurtedVelocity;
        hurtedTimer = fsm.hurtedTime;
        added = false;
        //fsm.material.SetFloat("FlushAmount",1f);
        //播放待机动画
        //            fsm.animator.SetBool()
    }
    public override void ActionState(FSMBase fsm)
    {
        //倒计时
        hurtedTimer -= Time.deltaTime;
        if (hurtedTimer <= 0 && !added)
        {
            added = true;
            this.addMap(FSMTriggerID.Dead, FSMStateID.Dead);
            this.addMap(FSMTriggerID.TargetFound, FSMStateID.Chase);
            this.addMap(FSMTriggerID.TargetLost, FSMStateID.Patrol);
        }
    }

    public override void ExitState(FSMBase fsm)
    {
        fsm.StopPosition();
        //fsm.material.SetFloat("FlushAmount",0f);
        //删除映射
        ClearAll();
    }
}