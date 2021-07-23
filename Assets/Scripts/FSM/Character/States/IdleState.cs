using EveryFunc;
using UnityEngine;
using System;
public class IdleState : FSMState
{
    private float idleTimer;
    public override void Init()
    {
        stateID = FSMStateID.Idle;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {

        idleTimer = fsm.idleTime;
        fsm.idleTime += UnityEngine.Random.Range(0, 1f);
    }
    public override void ActionState(FSMBase fsm)
    {
        fsm.idleTime -= Time.deltaTime;
    }
    public override void ExitState(FSMBase fsm)
    {
        fsm.idleTime = idleTimer;
    }
}