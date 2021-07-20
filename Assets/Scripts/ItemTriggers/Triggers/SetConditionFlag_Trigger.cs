using UnityEngine;
public class SetConditionFlag_Trigger : ITrigger
{
    public ConditionTrigger targetCondition;
    public int changeToFlag = 2;
    
    public override void Action()
    {

        
        if (targetCondition.name == "Æ´Í¼×°ÖÃ")
        {
            
            if (UIManager.Instance.remainJigsaw())
            {
                if (targetCondition.flag != 4)
                {
                    targetCondition.flag = 4;
                    return;
                }
            }
            if (targetCondition.flag > changeToFlag)
            {
                return;
            }
        }
        targetCondition.flag = changeToFlag;
    }
}