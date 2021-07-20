using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class ConditionTrigger : ItemTrigger
{
    public ItemTrigger[] triggers; // = new ItemTrigger[2];
    [Tooltip("范围：1~")]
    public int flag = 1;
    private void Awake()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i].GetType() == typeof(PassiveTrigger))
            {
                ((PassiveTrigger)triggers[i]).conditionFlag = i+1;
                ((PassiveTrigger)triggers[i]).condition = this;
            }
        }
    }
    public override void StartTrigger()
    {
        //如果trigger并没有激活
        if (!isActive) return;
        //消耗物品（如果有
        foreach (var item in itemList)
        {
            BagManager.Instance.RemoveItemList(item);
        }
        //如果不是被动触发
        if (triggers[flag - 1].GetType() != typeof(PassiveTrigger))
            triggers[flag - 1].StartTrigger();
    }
    public override void ContinueTrigger()
    {
        triggers[flag - 1].ContinueTrigger();
    }
}