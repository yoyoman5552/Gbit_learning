using EveryFunc;
using UnityEngine;
public class JumpState : PlayerFSMState {
    //重力加速度
    private float velocity_Y;
        private Transform playerChildTF;
    public override void Init () {
        stateID = PlayerFSMStateID.Jump;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState (PlayerFSMBase fsm) {
        
        velocity_Y = fsm.velocity_Y;
        playerChildTF = fsm.childTF;
        ReadyToJump(fsm);
    }
    public override void ActionState (PlayerFSMBase fsm) {
        //如果不能交互
        if (!fsm.reactAble) return;
        //检测跳跃状态
        CheckJump (fsm);
    }
    public override void FixedActionState (PlayerFSMBase fsm) {
        Jump ();
    }
    public override void ExitState (PlayerFSMBase fsm) { }
    // 角色跳跃
    private void ReadyToJump (PlayerFSMBase fsm) {
        velocity_Y = Mathf.Sqrt (fsm.jumpHeight * -2f * ConstantList.g);
    }
    private void Jump () {
        //重力模拟
        velocity_Y += ConstantList.g * Time.fixedDeltaTime;
        playerChildTF.Translate (new Vector3 (0, velocity_Y) * Time.fixedDeltaTime);
    }
    private void CheckJump (PlayerFSMBase fsmBase) {
        //判断子物体是在下落状态(velocity_Y<0)而且子物体离父物体距离小于等于0.05
        if (playerChildTF.position.y <= fsmBase.transform.position.y + 0.05f && velocity_Y < 0) {
            //满足了就说明跳跃完成
            velocity_Y = 0;
            playerChildTF.position = fsmBase.transform.position;
            fsmBase.isJump = false;
            fsmBase.walkAble = true;
            fsmBase.collider.isTrigger = false;
        }
    }
}