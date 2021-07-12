using EveryFunc;
using UnityEngine;
public class InteractiveState : PlayerFSMState {
    GameObject target;
    public override void Init () {
        stateID = PlayerFSMStateID.Interactive;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (PlayerFSMBase fsm) { }
    public override void ActionState (PlayerFSMBase fsm) {
        //如果不能交互
        if (!fsm.reactAble) return;
        if (Input.GetKeyDown (KeyCode.E)) {
            target = fsm.playerDetect.GetFirst ();
            //如果有条件类型的trigger
            if (target != null) {
                if (target.GetComponent<ConditionTrigger> () != null) {
                    target.GetComponent<ConditionTrigger> ().StartTrigger ();
                }
                //否则就是直接执行trigger 
                else {
                    target.GetComponent<ActiveTrigger> ().StartTrigger ();
                }
            }
        }
    }
    public override void FixedActionState (PlayerFSMBase fsm) { }
    public override void ExitState (PlayerFSMBase fsm) { }
}