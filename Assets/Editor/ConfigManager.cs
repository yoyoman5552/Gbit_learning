using System;
using System.IO;
using EveryFunc;
using OfficeOpenXml;
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

    [MenuItem("Tools/Resources/生成Boss攻击状态列表")]
    public static void GenerateAttackCurve()
    {
        //技能数据的excel文件路径
        string path = "Assets/Editor/Boss攻击周期.csv";
    
        //获取excel文件信息
        FileInfo fileInfo = new FileInfo(path);
        StreamReader sd=fileInfo.OpenText();
        string[] strs = sd.ReadToEnd().Split('\n');
        sd.Close();
        StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/AttackList.txt");
        //第一行是标签名，最后一行是空行
        for (int i = 1; i < strs.Length - 1; i++)
        {
            Debug.Log(strs[i]);
            string[] row = strs[i].Split(',');
            BossAttackCurve data = new BossAttackCurve();
            int j;
            for (j = 0; j < ConstantList.batteryCount; j++)
                data.stateList[j] = Convert.ToInt32(row[j]);
            data.durationTime = Convert.ToSingle(row[j]); ;
            string JsonString = JsonUtility.ToJson(data);
            sw.Write(JsonString);
            //每个技能表之间加个@，作为分隔标记
            sw.Write('赣');
            
        }
        sw.Close();
        /*         //打开excel表格 using表示只在{}内打开，{}后会释放资源
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    Debug.Log(worksheet.Cells[1, 1].Value);
                    //index=2是因为第一行是标签
                    StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/AttackList.txt");
                    int index = 2;
                    while (worksheet.Cells[index, 1].Value != null)
                    {
                        Debug.Log("i");

                        BossAttackCurve data = SaveBossAttackCurve(index, worksheet.Cells);
                        string JsonString = JsonUtility.ToJson(data);
                        sw.Write(JsonString);
                        //每个技能表之间加个@，作为分隔标记
                        sw.Write('赣');
                        index++;
                    }
                    sw.Close();
                } //关闭excel表格
         */
    }
    public static BossAttackCurve SaveBossAttackCurve(int index, ExcelRange ceil)
    {
        //读取Excel一行的内容，保存到saveData上，最后返回saveData
        //顺序不能变，要变的话，excel顺序也得变
        BossAttackCurve saveData = new BossAttackCurve();
        int rowIndex = 1;
        //根据Excel表的顺序，一路保存
        for (int i = 0; i < saveData.stateList.Length; i++)
        {
            saveData.stateList[i] = Convert.ToInt32(ceil[index, rowIndex++].Value.ToString());
        }
        saveData.durationTime = Convert.ToInt32(ceil[index, rowIndex++].Value.ToString());
        return saveData;
    }
}