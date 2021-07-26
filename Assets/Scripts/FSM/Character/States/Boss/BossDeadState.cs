using EveryFunc;
using UnityEngine;
using System;
public class BossDeadState : FSMState
{
    public override void Init()
    {
        stateID = FSMStateID.BossDead;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        BossFSM bossFSM = fsm.GetComponent<BossFSM>();
        Debug.Log("Dead");
        fsm.animator.SetBool("IsDead", true);

        //将玩家的机械臂卸下来
        Debug.Log("fsm.targetTF:" + fsm.targetTF);
        fsm.targetTF.GetComponent<PlayerController>().SetArmor(false, 0);

        //把所有的机械臂掉落物都关掉
        foreach (var armor in fsm.transform.GetComponentsInChildren<SetArmorFlag_Trigger>())
        {
            armor.gameObject.SetActive(false);
        }

        //生成最后的物品
        GameObject obj = GameObject.Instantiate(bossFSM.generateItem, bossFSM.generateItem.transform.position, Quaternion.identity);
        obj.SetActive(true);
        obj.transform.SetParent(bossFSM.transform);
        
        //关掉自身的fsm
        fsm.enabled = false;
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
}