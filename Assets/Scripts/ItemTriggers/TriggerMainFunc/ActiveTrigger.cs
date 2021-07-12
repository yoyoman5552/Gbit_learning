using System.Collections.Generic;
using UnityEngine;
using System;
[SerializeField]
public class ActiveTrigger : ItemTrigger {

    private void Start () {
        /* //初始化Trigger列表
        TriggerList = new List<ITrigger>();
        ITrigger[] triggers = this.GetComponents<ITrigger>();
        for (int i = 0; i < triggers.Length; i++)
        {
            TriggerList.Add(triggers[i]);
        } */
    }
    private void Update () {
        /*        if(Input.GetKeyDown(KeyCode.J)){
                   StartTrigger();
               } */
    }
    public override void StartTrigger () {
        //如果trigger并没有激活
        if (!isActive) return;
        //消耗物品（如果有
        foreach (var item in itemList)
        {
            BagManager.Instance.RemoveItemList(item);
        }
        foreach (var trigger in TriggerList) {
            trigger.Action ();
        }

    }
}