using System.Collections.Generic;
using UnityEngine;
public class PassiveTrigger : ItemTrigger
{
    [HideInInspector]
    public int conditionFlag;
    [HideInInspector]
    public ConditionTrigger condition;
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
        /*         if(Input.GetKeyDown(KeyCode.J)){
                    StartTrigger();
                } */
    }
    public override void StartTrigger()
    {
        foreach (var trigger in TriggerList)
        {
            trigger.Action();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        //如果上头有人而且还没轮到自己
        if (condition != null)
            if (condition.flag != conditionFlag) return;
        if (other.CompareTag("PlayerDetect") && other.GetComponentInParent<PlayerController>().GeteAble())
            StartTrigger();
    }
}