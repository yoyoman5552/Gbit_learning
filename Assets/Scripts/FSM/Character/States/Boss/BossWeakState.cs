using EveryFunc;
using UnityEngine;
using System;
public class BossWeakState : FSMState
{
    private float timer;
    public override void Init()
    {
        stateID = FSMStateID.BossWeak;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        fsm.GetComponent<BossFSM>().m_hurtedCount = 0;
        timer = fsm.hurtedTime;
    }
    public override void ActionState(FSMBase fsm)
    {
        fsm.hurtedTime -= Time.deltaTime;
    }
    public override void ExitState(FSMBase fsm)
    {
        fsm.hurtedTime = timer;
    }
}