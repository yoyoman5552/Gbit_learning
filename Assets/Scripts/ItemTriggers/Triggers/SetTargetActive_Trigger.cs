
using UnityEngine;

public class SetTargetActive_Trigger : ITrigger
{
    public GameObject target;
    [Tooltip("目标状态")]
    public bool targetFlag = true;

    public override void Action()
    {
        target.SetActive(targetFlag);
        //        target.GetComponent<ItemTrigger>().StartTrigger();
    }
}