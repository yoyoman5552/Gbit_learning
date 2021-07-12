using EveryFunc;
using UnityEngine;
public class NormalMoveState : PlayerFSMState {
    Vector3 moveDir;
    public override void Init () {
        stateID = PlayerFSMStateID.NormalMove;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (PlayerFSMBase fsm) {
        moveDir = new Vector3 (0, 0, fsm.transform.position.z);
    }
    public override void ActionState (PlayerFSMBase fsm) {
        //TODO:攻击方法
        moveDir.x = Input.GetAxisRaw ("Horizontal");
        moveDir.y = Input.GetAxisRaw ("Vertical") * ConstantList.moveYPer;
    }
    public override void FixedActionState (PlayerFSMBase fsm) {
        //如果能够移动
        if (fsm.walkAble && fsm.reactAble) {
            fsm.rb.velocity = moveDir.normalized * fsm.m_speed * Time.fixedDeltaTime * ConstantList.speedPer;
        } else {
            fsm.rb.velocity = Vector3.zero;
        }
    }
    public override void ExitState (PlayerFSMBase fsm) {

    }
}