using UnityEngine;
using EveryFunc;
public class GetHurtedTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.GetHurted;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.isHurted;
    }

}