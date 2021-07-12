using EveryFunc;
using UnityEngine;
public class TargetLostTrigger : FSMTrigger {
    public override void Init () {
        triggerID = FSMTriggerID.TargetLost;
    }
    public override bool HandleTrigger (FSMBase fsm) {
        //TODO:targetLost
        //return false;
        if (Vector3.Distance (fsm.transform.position, fsm.targetTF.position) > fsm.minRadius + 2f) {
            fsm.targetTF = null;
            return true;
        } else return false;
    }

}