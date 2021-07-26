using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//死亡状态
public class DeadState : FSMState
{
    //一定要初始化stateID,而且要初始化对
    public override void Init()
    {
        stateID = FSMStateID.Dead;
    }
    public override void EnterState(FSMBase fsm)
    {
        //fsm.enemyAudio.PlayOneShot(fsm.deadClip);
        base.EnterState(fsm);
        //TODO:新增死亡动画
        fsm.animator.SetBool("die", true);
        Debug.Log("死亡");
        //fsm.gameObject.SetActive(false);
        fsm.GetComponent<Collider2D>().enabled = false;
        fsm.Invoke("DeadDelay", 0.5f);
        GameManager.Instance.CheckEnemy();
        //死亡之后状态机禁用
        fsm.enabled = false;
    }
}