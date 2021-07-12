using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
//单例模式
public class ResourceManager
{
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        set
        {
            if (instance != null)
                Debug.LogError("ResourceManager重复实例");
            instance = value;
        }
        get
        {
            if (instance == null)
                instance = new ResourceManager();
            return instance;
        }
    }
    public ResourceManager()
    {
        //加载ConfigMap.txt文件
        string fileContent = GetConfigFile();
        //解析文件
        BuildMap(fileContent);
    }

    private static Dictionary<string, string> configMap;
    public string GetConfigFile()
    {
        StreamReader streamReader = new StreamReader(Application.dataPath + "/StreamingAssets/ConfigMap.txt");
        string JsonString = streamReader.ReadToEnd();
        streamReader.Close();
        return JsonString;
    }
    private void BuildMap(string fileContent)
    {
        //解析文件 将文件名和文件路径分开保存，形成字典
        configMap = new Dictionary<string, string>();
        foreach (string str in fileContent.Split('赣'))
        {
            if (str != "")
            {
                string fileName = str.Split('=')[0];
                string filePath = str.Split('=')[1];
                // filePath = filePath.Remove (filePath.Length); //删除末尾的空值
                //                    Debug.Log ("fileName:" + fileName + ",filePath:" + filePath);
                //Debug.Log("Config:"+filePath+","+Resources.Load<GameObject> (filePath));
                //                    if (fileName == "Ball") Debug.Log ("文件中有Ball");
                configMap.Add(fileName, filePath);
            }
        }
    }
    public GameObject OBJLoad(string prefabId)
    {
        throw new NotImplementedException();
    }
    public T Load<T>(string prefabName) where T : UnityEngine.Object
    {
        //加载资源 从prefab名转化成路径名
        //prefabName -> prefabPath
        string prefabPath = configMap[prefabName];
        return Resources.Load<T>(prefabPath);
    }
}