using UnityEngine;
public class SetConditionFlag_Trigger : ITrigger
{
    public ConditionTrigger targetCondition;
    public int changeToFlag = 2;
    public override void Action()
    {

        targetCondition.flag = changeToFlag;
        if (targetCondition.name == "ƴͼװ��")
        {
            if (UIManager.Instance.remainJigsaw())
            {
                targetCondition.flag = 4;
            }
            
        }
    }
}