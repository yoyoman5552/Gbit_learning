using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllJigsaw_trigger : ITrigger
{
    public ConditionTrigger target;
    public string detail_easy1;
    public override void Action()
    {
        //TODO:切换房间
        UIManager.Instance.CallTalkUI(detail_easy1);
        //对话UI结束：detailUI
        //TODO:detailUI
        //detail结束：jigsawUI
        UIManager.Instance.getPopWindowNum(2);
        target.flag = 3;
        //Debug.Log("UIEasyShow:" + detail);
    }
}
