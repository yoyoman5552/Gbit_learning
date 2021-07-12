using UnityEngine;
using EveryFunc;
public class IdleDoneTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.IdleDone;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.idleTime <= 0;
    }

}