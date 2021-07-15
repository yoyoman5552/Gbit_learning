using UnityEngine;
using EveryFunc;
public class BossWeakDoneTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.BossWeakDone;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.hurtedTime < 0;
    }

}