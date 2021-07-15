using EveryFunc;
using UnityEngine;
using System;
public class BatteryIdleState : FSMState
{
    BatteryFSM batteryFSM;
    public override void Init()
    {
        stateID = FSMStateID.BatteryIdle;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        batteryFSM = fsm.GetComponent<BatteryFSM>();
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
}