using EveryFunc;
using UnityEngine;
using System;
public class BossIdleState : FSMState
{
    public override void Init()
    {
        stateID = FSMStateID.BossIdle;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        BossFSM bossFSM = fsm.GetComponent<BossFSM>();
        bossFSM.softLight.SetActive(false);
        bossFSM.battleLight.SetActive(false);
        //借由外力将其关掉
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
}