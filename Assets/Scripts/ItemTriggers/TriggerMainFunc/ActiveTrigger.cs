using System.Collections.Generic;
using UnityEngine;
using System;
[SerializeField]
public class ActiveTrigger : ItemTrigger
{

    private void Start()
    {
        /* //初始化Trigger列表
        TriggerList = new List<ITrigger>();
        ITrigger[] triggers = this.GetComponents<ITrigger>();
        for (int i = 0; i < triggers.Length; i++)
        {
            TriggerList.Add(triggers[i]);
        } */
    }
    private void Update()
    {
        /*        if(Input.GetKeyDown(KeyCode.J)){
                   StartTrigger();
               } */
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
        for (currentIndex = 0; currentIndex < TriggerList.Count; currentIndex++)
        {
            TriggerList[currentIndex].Action();
            //如果是跟自言自语有关的，就推出，暂停
            if (TriggerList[currentIndex].GetType() == typeof(UIEasyShow_Trigger))
            {
                Debug.Log("easyShow,index:" + currentIndex);
                UIManager.Instance.SaveActiveTrigger(this);
                break;
            }
        }
    }
    public override void ContinueTrigger()
    {
        UIManager.Instance.RemoveActiveTrigger();
        for (currentIndex = currentIndex + 1; currentIndex < TriggerList.Count; currentIndex++)
        {
            TriggerList[currentIndex].Action();
            //如果是跟自言自语有关的，就退出，暂停
            if (TriggerList[currentIndex].GetType() == typeof(UIEasyShow_Trigger))
            {
                Debug.Log("easyShow,index:" + currentIndex);
                UIManager.Instance.SaveActiveTrigger(this);
                break;
            }
        }
    }
}