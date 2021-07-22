using UnityEngine;
public class AddConditionFlag_Trigger : ITrigger
{
    public ConditionTrigger targetCondition;
    public int addFlag=1;
    public override void Action()
    {
        targetCondition.flag = Mathf.Min(targetCondition.flag + addFlag, targetCondition.triggers.Length);
    }
}