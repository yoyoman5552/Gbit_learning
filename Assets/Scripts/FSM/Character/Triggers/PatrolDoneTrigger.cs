using EveryFunc;
using UnityEngine;
public class PatrolDoneTrigger : FSMTrigger {
    public override void Init () {
        triggerID = FSMTriggerID.PatrolDone;
    }
    public override bool HandleTrigger (FSMBase fsm) {
        return Vector3.Distance (fsm.patrolPos, fsm.transform.position) <= 0.05f;
    }

}