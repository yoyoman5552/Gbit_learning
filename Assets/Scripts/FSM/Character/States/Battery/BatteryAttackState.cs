using EveryFunc;
using UnityEngine;
using System;
public class BatteryAttackState : FSMState
{
    private IAttack attack;
    private BatteryFSM batteryFSM;
    public override void Init()
    {
        stateID = FSMStateID.BatteryAttack;
    }
    public override void EnterState(FSMBase fsm)
    {
        batteryFSM = fsm.GetComponent<BatteryFSM>();
        attack = batteryFSM.attackList[batteryFSM.attackIndex];
        
        attack.Init();
    }
    public override void ActionState(FSMBase fsm)
    {
        attack.Action(fsm.targetTF.gameObject);
        if (attack.isAction) fsm.ChangeActiveState(FSMStateID.BatteryIdle);
    }
    public override void ExitState(FSMBase fsm)
    {
        attack.StopAllCoroutines();
    }
}