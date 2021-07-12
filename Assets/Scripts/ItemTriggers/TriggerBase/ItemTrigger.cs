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
    public List<int> itemList;
    /// <summary>
    /// 激活方式
    /// </summary>
    public virtual void StartTrigger()
    {

    }
}