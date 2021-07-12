using EveryFunc;
using UnityEngine;
public class OutOfAttackRangeTrigger : FSMTrigger {
    public override void Init () {
        triggerID = FSMTriggerID.OutOfAttackRange;
    }
    public override bool HandleTrigger (FSMBase fsm) {
        //return false;
        return Vector3.Distance (fsm.transform.position, fsm.targetTF.position) > fsm.attackRadius;
    }

}