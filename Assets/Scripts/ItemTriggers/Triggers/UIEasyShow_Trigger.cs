using EveryFunc;
using UnityEngine;
using System;
using UnityEngine.UI;
//关闭EButton，关闭执行
//TODO:UI目前还不懂需求
public class UIEasyShow_Trigger : ITrigger
{
    public string detail;
    public override void Action()
    {
        //TODO:切换房间
        UIManager.Instance.CallTalkUI(detail);
        //Debug.Log("UIEasyShow:" + detail);
    }
}