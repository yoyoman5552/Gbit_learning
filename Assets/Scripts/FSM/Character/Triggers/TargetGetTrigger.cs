using EveryFunc;
using UnityEngine;
public class TargetGetTrigger : FSMTrigger {
    public override void Init () {
        triggerID = FSMTriggerID.TargetGet;
    }
    public override bool HandleTrigger (FSMBase fsm) {
        return Vector3.Distance (fsm.transform.position, fsm.targetTF.position) <= fsm.attackRadius;
    }

}