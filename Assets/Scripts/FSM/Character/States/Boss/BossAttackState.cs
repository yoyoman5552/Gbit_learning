using System;
using System.IO;
using EveryFunc;
using UnityEngine;
public class BossAttackState : FSMState
{
    private BossAttackCurve[] attackList;
    private int curveIndex;
    private float waitTime;
    private GameObject[] batteryArray;
    public override void Init()
    {
        stateID = FSMStateID.BossAttack;

        InitAttackList();
    }
    public override void EnterState(FSMBase fsm)
    {
        //切换成最终战的bgm
        BGMManager.Instance.ChangeBGM(BGMType.BossBattle);

        BossFSM bossFSM = fsm.GetComponent<BossFSM>();
        waitTime = 0;
        batteryArray = bossFSM.batteryArray;
        fsm.animator.SetFloat("AttackStateNum", bossFSM.attackStateNum);
        fsm.animator.SetBool("IsWeak", false);
    }
    public override void ActionState(FSMBase fsm)
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        // Debug.Log ("当前状态：" + attackList[curveIndex].stateList[0] + "," +
        //     attackList[curveIndex].stateList[1] + "," +
        //     attackList[curveIndex].stateList[2] + "," +
        //     attackList[curveIndex].stateList[3] + "," + "持续时间：" + attackList[curveIndex].durationTime);
        //切换炮台状态
        waitTime = attackList[curveIndex].durationTime;
        for (int i = 0; i < ConstantList.batteryCount; i++)
        {
            batteryArray[i].GetComponent<BatteryFSM>().ChangeState(attackList[curveIndex].stateList[i]);
        }
        //进入下一个状态
        curveIndex = (curveIndex + 1) % attackList.Length;

    }
    public override void ExitState(FSMBase fsm)
    {
        //被进入虚弱状态，将没有打完的波数进1
        curveIndex = (curveIndex + 1) % attackList.Length;

        //强制切换状态
        for (int i = 0; i < ConstantList.batteryCount; i++)
        {
            batteryArray[i].GetComponent<BatteryFSM>().ChangeActiveState(FSMStateID.BatteryIdle);
        }
    }
    private void InitAttackList()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/AttackList.txt");
        string[] strs = reader.ReadToEnd().Split('赣');
        reader.Close();
        attackList = new BossAttackCurve[strs.Length - 1];
        for (int i = 0; i < strs.Length - 1; i++)
        {
            attackList[i] = JsonUtility.FromJson<BossAttackCurve>(strs[i]);
        }
        
        curveIndex=0;
    }
}