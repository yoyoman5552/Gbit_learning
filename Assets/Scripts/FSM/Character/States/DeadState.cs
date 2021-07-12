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
        base.EnterState(fsm);
        //播放待机动画
        //        fsm.animator.SetBool("IsDead", true);
        //            fsm.transform.Find ("CharacterUICanvas").gameObject.SetActive (false);
        //            fsm.animator.SetBool()
        //            GameObject.Destroy (fsm.gameObject, 1f);
        //怪物死亡的时候，给房间传参，现存怪物表里删掉该物体
        //RoomController.Instance.RemoveMonster(fsm.gameObject);

        //   Debug.Log ("come in DeadState：" + RoomController.Instance.MonsterList.Count);
        //GameController.Instance.test_EnterDead++;
        //fsm.Invoke("DeadDelay", 2f);
        //死亡之后状态机禁用
        //fsm.enabled = false;
    }
}