using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class BatteryFSM : FSMBase
{
    public BatteryAttackType[] attackList=new BatteryAttackType[2];
    public override void ConfigFSM()
    {
        FSMState batteryIdle = new BatteryIdleState();
        FSMState batteryAttackOne = new BatteryAttackOneState();
        FSMState batteryAttackTwo = new BatteryAttackTwoState();
        FSMState batteryAttackThree = new BatteryAttackThreeState();


        statesList.Add(batteryIdle);
        statesList.Add(batteryAttackOne);
        statesList.Add(batteryAttackTwo);
        statesList.Add(batteryAttackThree);
    }
}
