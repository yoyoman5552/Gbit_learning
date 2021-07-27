using EveryFunc;
using UnityEngine;
using System;
using UnityEngine.UI;
//关闭EButton，关闭执行
//TODO:UI目前还不懂需求
public class exit_Trigger : ITrigger
{
    public ExitGame getChip;
    
    public override void Action()
    {
        //TODO:切换房间
        //Debug.Log("UIDetailShow:名称：" + name + ",详细：" + detail);
        //TODO:不同情况下调用不同弹窗
        //UIManager.Instance.CallTalkUI(detail);

        getChip.showChip = true;
    }

}