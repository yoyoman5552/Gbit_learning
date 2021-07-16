using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Getjigsaw_trigger : ITrigger
{
    public int partWhich;
    public ConditionTrigger target;
    public override void Action()
    {
        UIManager.Instance.JigsawControlList[partWhich-1] = true;
        UIManager.Instance.addJigsaw();
        
        if(UIManager.Instance.remainJigsaw())
        {
            target.flag = 4;            
        }
    }
}
