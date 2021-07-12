using UnityEngine.UI;
using System;
using UnityEngine;
[Serializable]
public class ItemInfo
{
    [HideInInspector]
    public int id;
    public string name;
    public Sprite image;
    public string detail;
    [HideInInspector]
    public int useNum;
    public ItemInfo(){

    }
    public ItemInfo(string name){
        this.name=name;

    }
}