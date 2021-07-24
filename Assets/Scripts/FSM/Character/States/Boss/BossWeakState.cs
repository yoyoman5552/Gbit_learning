using System;
using EveryFunc;
using UnityEngine;
public class BossWeakState : FSMState {
    private float timer;
    public override void Init () {
        stateID = FSMStateID.BossWeak;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (FSMBase fsm) {
        BossFSM bossFSM = fsm.GetComponent<BossFSM> ();
        bossFSM.m_hurtedCount = 0;
        //判断attackStateNum的数值 +2是因为第一个状态为默认的
        for (int i = 0; i < bossFSM.hurtedMaxHP.Length; i++)
            if (bossFSM.HP <= bossFSM.hurtedMaxHP[i])
                bossFSM.attackStateNum = (i + 1) / (bossFSM.hurtedMaxHP.Length);
        //        bossFSM.attackStateNum = Mathf.Min (bossFSM.attackStateNum + 1, 3);
        bossFSM.animator.SetBool ("IsWeak", true);
        timer = fsm.hurtedTime;
    }
    public override void ActionState (FSMBase fsm) {
        fsm.hurtedTime -= Time.deltaTime;
    }
    public override void ExitState (FSMBase fsm) {
        fsm.hurtedTime = timer;
        fsm.animator.SetBool ("IsWeak", false);
    }
}