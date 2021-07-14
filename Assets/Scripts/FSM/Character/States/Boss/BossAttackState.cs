using EveryFunc;
using UnityEngine;
using System;
using System.IO;
public class BossAttackState : FSMState
{
    private BossAttackCurve[] attackList;
    private int curveIndex;
    public override void Init()
    {
        stateID = FSMStateID.BossAttack;

        InitAttackList();
    }
    public override void EnterState(FSMBase fsm)
    {
        curveIndex = 0;
    }
    public override void ActionState(FSMBase fsm)
    {
    }
    public override void ExitState(FSMBase fsm)
    {
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
    }
}