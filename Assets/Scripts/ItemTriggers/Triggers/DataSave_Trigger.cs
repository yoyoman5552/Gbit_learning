using System;
using EveryFunc;
using UnityEngine;
//使人跳跃
public class DataSave_Trigger : ITrigger {
    public Transform SavePos;
    public override void Action () {
        //TODO:保存
        Debug.Log("保存成功");
        
    }
}