using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllJigsaw_trigger : ITrigger
{
    public ConditionTrigger target;
    public string detail_easy1;
    public override void Action()
    {
        //TODO:�л�����
        UIManager.Instance.CallTalkUI(detail_easy1);
        //�Ի�UI������detailUI
        //TODO:detailUI
        //detail������jigsawUI
        UIManager.Instance.getPopWindowNum(2);
        target.flag = 3;
        //Debug.Log("UIEasyShow:" + detail);
    }
}
