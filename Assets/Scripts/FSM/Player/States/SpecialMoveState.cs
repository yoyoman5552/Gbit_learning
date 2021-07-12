using EveryFunc;
using UnityEngine;
public class SpecialMoveState : PlayerFSMState {
    Vector3 targetPos;
    private Vector2 velocity;
    private float smoothTime;

    public override void Init () {
        stateID = PlayerFSMStateID.SpecialMove;
    }
    public override void EnterState (PlayerFSMBase fsm) {
        targetPos = fsm.targetPos;
        velocity = fsm.velocity;
        smoothTime = fsm.smoothTime;
    }
    public override void ActionState (PlayerFSMBase fsm) { }
    public override void FixedActionState (PlayerFSMBase fsm) {
        fsm.transform.position = new Vector3 (Mathf.SmoothDamp (fsm.transform.position.x, targetPos.x, ref velocity.x, smoothTime), Mathf.SmoothDamp (fsm.transform.position.y, targetPos.y, ref velocity.y, smoothTime), fsm.transform.position.z);
    }
    public override void ExitState (PlayerFSMBase fsm) {

    }
}