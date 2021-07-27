using System.Collections.Generic;
using UnityEngine;
public class BagManager
{
    //单例模式
    private static BagManager instance;
    public static BagManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BagManager();
            }
            return instance;
        }
        set
        {
            if (instance != null)
            {
                Debug.LogError("BagManager重复实例");
                return;
            }
            instance = value;
        }
    }
    //物品id字典：物品id->物品
    public Dictionary<string, ItemInfo> itemList;
    public int[] hashArray;
    public static int HASHMAXLENGTH = 10086;
    public BagManager()
    {
        itemList = new Dictionary<string, ItemInfo>();
        hashArray = new int[HASHMAXLENGTH];

    }
    //增加物品
    public void AddItemList(ItemInfo item)
    {
        //如果已经有该物品了
        if (itemList.ContainsKey(item.name))
        {
            //            Debug.LogError("增加了已有的物品:" + item.name);
            //将该物品的使用次数增加
            itemList[item.name].useNum += item.useNum;
        }
        else
        {
            //增加该物体
            ItemInfo addItem = new ItemInfo(item);
            itemList.Add(addItem.name, addItem);
        }
        Debug.Log("增加物品:" + item.name);
        UIManager.Instance.UpdateBagUI(itemList);
    }
    public void ShowCount(ItemInfo item)
    {

    }
    /*     public int FindItemID(string itemName){
        }
     */
    public void RemoveItemList(ItemInfo target)
    {
        Debug.Log("删除物品：" + target.name);
        if (itemList.ContainsKey(target.name))
        {
            itemList[target.name].useNum -= target.useNum;
            Debug.Log("使用次数减少：" + itemList[target.name].useNum);
            //该物品被消耗殆尽
            if (itemList[target.name].useNum <= 0)
            {
                //物品列表中移除该物品
                itemList.Remove(target.name);

            }
            UIManager.Instance.UpdateBagUI(itemList);
        }
        //if (!itemList.ContainsKey(targetID)) Debug.LogError("删除了不具有的东西");
    }

    public int GetHashCode(string name)
    {
        //先将数组转换成ASCII码
        byte[] array = System.Text.Encoding.ASCII.GetBytes(name); //数组array为对应的ASCII数组 
        int num = 0;
        foreach (var str in array)
            num += (int)str;

        num = num % HASHMAXLENGTH;
        //不等于0说明这个位置有存储其他的了
        if (hashArray[num] != 0)
        {
            num = GetOtherHash(num);
        }
        return num;
    }
    public int GetOtherHash(int num, int i = 1)
    {
        if (num + i * i < hashArray.Length)
            if (hashArray[num + i * i] == 0)
                return num;
        if (num - i * i >= 0)
            if (hashArray[num - i * i] == 0)
                return num;
        return GetOtherHash(num, i + 1);
    }
}