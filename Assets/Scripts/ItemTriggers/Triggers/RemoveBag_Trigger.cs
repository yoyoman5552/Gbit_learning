using EveryFunc;
using UnityEngine;
public class RemoveBag_Trigger : ITrigger
{
    //FIXME:所有的背包类物品是否都是用SetTargetCondition
    public ItemInfo[] itemArray;
    //    public GameObject[] targets = new GameObject[1];
    //   public ItemInfo itemInfo;
    private void Awake()
    {
    }
    public override void Action()
    {
        foreach (var target in itemArray)
        {
            BagManager.Instance.RemoveItemList(target);
            //BagManager.Instance.RemoveItemList();
        }
    }
}