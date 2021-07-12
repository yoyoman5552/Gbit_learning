using UnityEngine;
public class AddConditionFlag_Trigger : ITrigger
{
    public ConditionTrigger targetCondition;
    public override void Action()
    {
        targetCondition.flag = Mathf.Min(targetCondition.flag + 1, targetCondition.triggers.Length);
    }
}