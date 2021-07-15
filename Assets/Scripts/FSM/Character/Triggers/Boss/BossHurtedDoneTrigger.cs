using UnityEngine;
using EveryFunc;
public class BossHurtedDoneTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.BossHurtedDone;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        return fsm.GetComponent<BossFSM>().m_hurtedCount >= fsm.GetComponent<BossFSM>().hurtedThreshold;
    }

}