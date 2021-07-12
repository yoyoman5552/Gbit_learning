using EveryFunc;
using UnityEngine;
//关闭EButton，关闭执行
public class CloseTrigger_Trigger : ITrigger {
    public override void Action () {
        this.transform.Find ("EButton").GetComponent<SpriteRenderer> ().enabled = false;
        foreach (var trigger in this.GetComponents<ItemTrigger> ()) {
            trigger.isActive = false;
        }
    }
}