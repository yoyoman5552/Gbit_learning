using EveryFunc;
using UnityEngine;
using System;
using UnityEngine.UI;
//关闭EButton，关闭执行
//TODO:UI目前还不懂需求
public class UIDetailShow_Trigger : ITrigger
{

    public new string name;
    public Sprite objectImage;
    public string detail_title;
    public string detail_index;
    public override void Action()
    {
        //TODO:切换房间
        //Debug.Log("UIDetailShow:名称：" + name + ",详细：" + detail);
        //TODO:不同情况下调用不同弹窗
        //UIManager.Instance.CallTalkUI(detail);
        string title = detail_title.Replace("<size=200><color=white>", "");
        title = title.Replace("</color></size>", "");
        UIManager.Instance.CallDetailUI(name, title, detail_index, objectImage, false);

    }
}