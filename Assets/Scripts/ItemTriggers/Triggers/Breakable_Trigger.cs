using EveryFunc;
using UnityEngine;
public class Breakable_Trigger : ITrigger
{
    public BreakLevel level = BreakLevel.easy;
    private ItemTrigger itemTrigger;
    private void Start()
    {
        if (GetComponent<ConditionTrigger>() != null) itemTrigger = GetComponent<ConditionTrigger>();
        else itemTrigger = GetComponent<ItemTrigger>();
    }
    public override void Action()
    {
        Debug.Log("障碍物等级:" + level.ToString());
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject, 2f);
        if (itemTrigger != null)
        {
            itemTrigger.StartTrigger();
        }
        /*  if (level == BreakLevel.easy) {
             this.gameObject.SetActive (false);
             Destroy (this.gameObject, 2f);
         } */
    }
}