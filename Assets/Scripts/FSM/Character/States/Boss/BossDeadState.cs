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
        //boss死亡，切换成结束的bgm
        BGMManager.Instance.ChangeBGM(BGMType.End);

        //死亡动画播出
        BossFSM bossFSM = fsm.GetComponent<BossFSM>();
        bossFSM.battleLight.SetActive(false);
        bossFSM.softLight.SetActive(true);
        Debug.Log("Dead");
        fsm.animator.SetBool("IsDead", true);

        //更改目标,生成物体
        LastGoal(bossFSM);

        //销毁场上所有的机械臂
        DestroyGenerateWeapon(bossFSM);


        //关掉自身的fsm
        fsm.enabled = false;
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
    }
    public void LastGoal(BossFSM bossFSM)
    {
        //更改目标
        if (bossFSM.GetComponent<SetTargetUI_Trigger>() != null)
            bossFSM.GetComponent<SetTargetUI_Trigger>().Action();

        //生成最后的物品
        GameObject obj = GameObject.Instantiate(bossFSM.generateItem, bossFSM.generateItem.transform.position, Quaternion.identity);
        obj.SetActive(true);
        obj.transform.SetParent(bossFSM.transform);

    }
    public void DestroyGenerateWeapon(FSMBase fsm)
    {
        //将玩家的机械臂卸下来
        Debug.Log("fsm.targetTF:" + fsm.targetTF);
        fsm.targetTF.GetComponent<PlayerController>().SetArmor(false, 0);

        //把所有的机械臂掉落物都关掉
        foreach (var armor in fsm.transform.GetComponentsInChildren<SetArmorFlag_Trigger>())
        {
            armor.gameObject.SetActive(false);
        }
    }
}