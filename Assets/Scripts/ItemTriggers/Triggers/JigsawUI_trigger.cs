using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawUI_trigger : ITrigger
{
    //public GameObject JiagsawControl;
    public new string name;
    public string detail;
    public override void Action()
    {

        //this.gameObject.SetActive(true);
        string title = name.Replace("<size=200><color=white>", "");
        title = title.Replace("</color></size>", "");
        UIManager.Instance.CallJigsawUI(title, detail);
    }
}
