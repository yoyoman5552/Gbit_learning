using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ItemTrigger : MonoBehaviour
{
    public List<ITrigger> TriggerList;
    /// <summary>
    /// 能否执行
    /// </summary>
    [HideInInspector]
    public bool isActive = true;
    //物品id列表
    [HideInInspector]
    public List<string> itemList;

    [HideInInspector]
    public int currentIndex;
    /// <summary>
    /// 激活方式
    /// </summary>
    public virtual void StartTrigger()
    {

    }
    public virtual void ContinueTrigger()
    {

    }
    //是否是展示UI
    public bool IsShowUI(ITrigger targetTrigger)
    {
        return targetTrigger.GetType() == typeof(UIEasyShow_Trigger) || targetTrigger.GetType() == typeof(UIDetailShow_Trigger);
    }
}