using EveryFunc;
using UnityEngine;
using System;
public class BatteryIdleState : FSMState
{
    public override void Init()
    {
        stateID = FSMStateID.BatteryIdle;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
}