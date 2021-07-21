using UnityEngine;
using System.Collections.Generic;
using EveryFunc;
using System;
[Serializable]
public class SaveData
{
    public SaveData()
    {
        itemList = new Dictionary<string, ItemInfo>();
        enemys = new List<EnemyData>();
    }
    //房间号
    public string roomName;
    //人物名
    public Transform lastDoor;
    //物品列表
    public Dictionary<string, ItemInfo> itemList;
    //敌人数据
    public List<EnemyData> enemys;
    //TODO:房间里的物品数据
}
public class EnemyData
{
    public EnemyData()
    {

    }
    public EnemyData(bool attackType, int hp, Vector3 pos)
    {
        this.attackType = attackType;
        this.hp = hp;
        this.pos = pos;
    }
    public Vector3 pos;
    public int hp;
    public bool attackType;
}