using UnityEngine;
using EveryFunc;
public class DeadTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.Dead;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.HP <= 0;
    }

}