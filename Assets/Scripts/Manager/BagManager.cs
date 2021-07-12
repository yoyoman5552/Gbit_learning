using UnityEngine;
using System.Collections.Generic;
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
    public Dictionary<int, ItemInfo> itemList;
    public int[] hashArray;
    public static int HASHMAXLENGTH = 10086;
    public BagManager()
    {
        itemList = new Dictionary<int, ItemInfo>();
        hashArray = new int[HASHMAXLENGTH];

    }
    //增加物品
    public void AddItem(ItemInfo item)
    {
        //如果已经有该物品了
        if (itemList.ContainsKey(item.id))
        {
            Debug.LogError("增加了已有的物品:"+item.name);
            return;
        }
        Debug.Log("增加物品:" + item.name + "，id为" + item.id);
        itemList.Add(item.id, item);
        UIManager.Instance.UpdateBagUI(itemList);
    }
    public void RemoveItemList(int targetID)
    {
        Debug.Log("删除物品：id为" + targetID);
        if (itemList.ContainsKey(targetID))
        {
            itemList[targetID].useNum -= 1;
            //该物品被消耗殆尽
            if (itemList[targetID].useNum <= 0)
            {
                //物品列表中移除该物品
                itemList.Remove(targetID);
                UIManager.Instance.UpdateBagUI(itemList);
            }
        }
        //if (!itemList.ContainsKey(targetID)) Debug.LogError("删除了不具有的东西");
    }

    public int GetHashCode(string name)
    {
        //先将数组转换成ASCII码
        byte[] array = System.Text.Encoding.ASCII.GetBytes(name);  //数组array为对应的ASCII数组 
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