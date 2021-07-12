using UnityEngine;
using EveryFunc;
public class TargetFoundTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.TargetFound;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.targetTF!=null;
    }

}