using System;
using EveryFunc;
using UnityEngine;
using System.Collections;
public class BossWeakState : FSMState
{
    private float timer;
    BossFSM bossFSM;
    private float wudiTimer;
    public override void Init()
    {
        stateID = FSMStateID.BossWeak;
        //        throw new System.NotImplementedException();
    }
    public override void EnterState(FSMBase fsm)
    {
        bossFSM = fsm.GetComponent<BossFSM>();

        if (!fsm.isHurted)
        {
            fsm.isHurted = true;
            fsm.StartCoroutine(wudiDelay(fsm));
        }
        bossFSM.GenerateWeapon();
        //        bossFSM.attackStateNum = Mathf.Min (bossFSM.attackStateNum + 1, 3);
        bossFSM.animator.SetBool("IsWeak", true);

        timer = fsm.idleTime;
    }
    public override void ActionState(FSMBase fsm)
    {
        fsm.idleTime -= Time.deltaTime;
    }
    public override void ExitState(FSMBase fsm)
    {
        fsm.idleTime = timer;

        //清零伤害储量
        bossFSM.m_hurtedCount = 0;

        //判断attackStateNum的数值 +2是因为第一个状态为默认的
        for (int i = 0; i < bossFSM.hurtedMaxHP.Length; i++)
            if (bossFSM.HP <= bossFSM.hurtedMaxHP[i])
                bossFSM.attackStateNum = ((float)i + 1) / (bossFSM.hurtedMaxHP.Length);

        fsm.animator.SetBool("IsWeak", false);
    }
    private IEnumerator wudiDelay(FSMBase fsm)
    {
        yield return new WaitForSeconds(wudiTimer);
        fsm.isHurted = false;
    }
}