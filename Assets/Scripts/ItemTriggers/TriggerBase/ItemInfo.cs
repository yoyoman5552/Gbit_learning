using System;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class ItemInfo {
    [HideInInspector]
    public int id;
    public string name;
    public Sprite image;
    //public string detail;
    //使用次数
    public int useNum = 1;
    public ItemInfo () {

    }
    public ItemInfo (string name) {
        this.name = name;

    }
}