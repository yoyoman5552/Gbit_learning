using EveryFunc;
using UnityEngine;
using System;
public class BossDeadState : FSMState
{
    public override void Init()
    {
        stateID = FSMStateID.BossDead;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        Debug.Log("Dead");
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
}