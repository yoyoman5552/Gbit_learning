using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class BatteryFSM : FSMBase
{
    public IAttack[] attackList = new IAttack[3];
    [HideInInspector]
    public int attackIndex;
    public override void ConfigFSM()
    {
        if (statesList != null) return;
        statesList = new List<FSMState>();

        FSMState batteryIdle = new BatteryIdleState();
        FSMState batteryAttack = new BatteryAttackState();

        statesList.Add(batteryIdle);
        statesList.Add(batteryAttack);
    }
    public void ChangeState(int count)
    {
        //if (count == 0) ChangeActiveState(FSMStateID.BatteryIdle);
        //else
        //{
            if (count !=0)
            {
                attackIndex = count - 1;
                ChangeActiveState(FSMStateID.BatteryAttack);
            }
        //}
    }
}
