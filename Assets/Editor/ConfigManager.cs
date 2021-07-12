using System;
using System.IO;
using EveryFunc;
//using OfficeOpenXml;
using UnityEditor;
using UnityEngine;
//生成配置文件类

public class ConfigMap : Editor
{
    [MenuItem("Tools/Resources/生成预制件映射文件")]
    public static void GenerateConfig()
    {
        //生成资源配置文件
        //映射关系 名称=路径
        //1.查找Resources下的所有预制件完整路径
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            //2. 生成对应关系
            // 名称=路径
            //Path.GetFileNameWithoutExtension:将扩展名删了，只有名字
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            //获取Resources/后的，.prefab前的路径内容
            string path = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
            //名称=路径
            resFiles[i] = fileName + "=" + path;
        }
        //3.写入文件
        //StreamingAssets:特殊文件夹，生成项目时不会压缩该文件
        //        File.WriteAllLines ("Assets/StreamingAssets/ConfigMap.txt", resFiles);

        StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/ConfigMap.txt");
        foreach (var str in resFiles)
        {
            sw.Write(str + '赣');
        }
        sw.Close();
    }
}