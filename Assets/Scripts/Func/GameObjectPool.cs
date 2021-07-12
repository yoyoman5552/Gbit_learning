using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 对象池
/// </summary>
public class GameObjectPool
{
    /// <summary>
    /// 单例模式:不用MonoBehaviour
    /// </summary>
    private static GameObjectPool instance;
    public static GameObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObjectPool();
            }
            return instance;
        }
        set
        {
            if (instance != null)
            {
                Debug.LogError("单例模式重复创建：GameObjectPool");
                return;
            }
            instance = value;
        }
    }
    //字典：对象映射
    public Dictionary<string, List<GameObject>> cache = new Dictionary<string, List<GameObject>>();
    //生成物体
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        //Debug.Log ("Instantiate:" + prefabId);
        //从资源管理器获取目标预制体
        GameObject resources = ResourceManager.Instance.Load<GameObject>(prefabId);
        //返回生成对象
        return CreateObject(prefabId, resources, position, rotation);
    }
    public void Destroy(GameObject target)
    {
        target.SetActive(false);
    }

    private GameObject CreateObject(string prefabId, GameObject resources, Vector3 position, Quaternion rotation)
    {
        GameObject target = null;
        target = FindUseableObject(prefabId);
        if (target == null)
        {
            target = AddObject(prefabId, resources, position, rotation);
        }
        useObj(target, position, rotation);
        //TODO:如果有要初始化的数值，需要放在这里
        return target;
    }

    private void useObj(GameObject target, Vector3 position, Quaternion rotation)
    {
        target.transform.position = position;
        target.transform.rotation = rotation;
        target.SetActive(true);
    }

    private GameObject AddObject(string prefabId, GameObject resources, Vector3 position, Quaternion rotation)
    {
        //新生成物体
        GameObject obj = GameObject.Instantiate(resources, position, rotation);
        //将物体保存进字典
        if (!cache.ContainsKey(prefabId)) cache.Add(prefabId, new List<GameObject>());
        cache[prefabId].Add(obj);
        return obj;
    }

    private GameObject FindUseableObject(string prefabId)
    {
        if (cache.ContainsKey(prefabId))
        {
            return cache[prefabId].Find(s => s != null && !s.activeSelf);
        }
        return null;
    }
    //释放空间
    public void ClearKey(string key)
    {
        //释放特定物品的内存
        if (cache.ContainsKey(key))
        {
            foreach (var obj in cache[key])
            {
                if (obj == null) continue;
                GameObject.Destroy(obj);
            }
            cache.Remove(key);
        }
    }
    public void ClearAll()
    {
        //清空全部对象
        List<string> stringList = new List<string>(cache.Keys);
        foreach (string key in stringList)
        {
            //                Debug.Log ("删除资源：" + key);
            ClearKey(key);
        }
    }
}