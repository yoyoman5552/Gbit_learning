using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class ConditionTrigger : ItemTrigger
{
    public ItemTrigger[] triggers; // = new ItemTrigger[2];
    [Tooltip("范围：1~")]
    public int flag = 1;
    public override void StartTrigger()
    {
        //如果trigger并没有激活
        if (!isActive) return;
        //消耗物品（如果有
        foreach (var item in itemList)
        {
            BagManager.Instance.RemoveItemList(item);
        }
        triggers[flag - 1].StartTrigger();
    }
}