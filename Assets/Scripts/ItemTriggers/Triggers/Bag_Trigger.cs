using EveryFunc;
using UnityEngine;
public class Bag_Trigger : ITrigger
{
    //FIXME:所有的背包类物品是否都是用SetTargetCondition
    [HideInInspector]
    public GameObject[] targets = new GameObject[1];
    public ItemInfo itemInfo;
    private void Awake()
    {
        SetConditionFlag_Trigger[] triggers = GetComponents<SetConditionFlag_Trigger>();
        targets = new GameObject[triggers.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = triggers[i].targetCondition.gameObject;
        }
    }
    public override void Action()
    {
        //将名字堆叠在一起,生成一个独一的名字和哈希值
        string name = this.gameObject.name;
        foreach (var target in targets)
        {
            name += target.name;
        }
        itemInfo.id = BagManager.Instance.GetHashCode(name);
        //将该物品的id存入目标里
        foreach (var target in targets)
        {
            target.GetComponent<ItemTrigger>().itemList.Add(itemInfo.id);
        }
        itemInfo.name = name;
        itemInfo.useNum = targets.Length;
        BagManager.Instance.AddItem(itemInfo);
    }
}