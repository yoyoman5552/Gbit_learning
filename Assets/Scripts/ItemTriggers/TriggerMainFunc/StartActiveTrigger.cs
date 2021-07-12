using System.Collections.Generic;
using UnityEngine;
//在开始时就会调用的Trigger
public class StartActiveTrigger : ItemTrigger {

    private void Start () {
        StartTrigger ();
    }
    private void Update () {
        /*        if(Input.GetKeyDown(KeyCode.J)){
                   StartTrigger();
               } */
    }
    public override void StartTrigger () {
        //如果trigger并没有激活
        if (!isActive) return;
        foreach (var trigger in TriggerList) {
            trigger.Action ();
        }

    }
}