using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// UI类，关于UI的方法都放在此类，场景中UI的组件也放在UIManager下
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Tooltip("背包UI")]
    public GameObject bagUIOBJ;
    //弹窗UI结构变量
    [Tooltip("对话式UI")]
    public GameObject talkWindow;
    [Tooltip("物品信息UI")]
    public GameObject PopUpWindow;
    [Tooltip("目标UI")]
    public GameObject targetUI;

    //拼图相关变量
    [Tooltip("拼图UI")]
    public GameObject jigsawUI;
    [Tooltip("拼图碎片图片")]
    private List<GameObject> jigsawImges = new List<GameObject>();
    [Tooltip("拼图碎片是否收集")]
    public List<bool> JigsawControlList = new List<bool>();
    [Tooltip("拼图碎片数量")]
    public int JigsawAmount;
    [Tooltip("拼图碎片收集进度")]
    private int CurrentJigsawAmount = 0;
    [Tooltip("拼图碎片父节点")]
    public Transform totalJigsaw;
    [Tooltip("连弹3窗口")]
    private int popWindow = 0;
    //背包UI列表
    [HideInInspector]
    public Image[] bagUIList;
    private Text detailText;
    //private Image characterImage;
    private Image BackGround;
    private Text objectName;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("UIManager多重实例");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        InitComponent();
        //this.gameObject.SetActive(false);
        initUIObject();
    }

    //拼图信息UI
    public void CallJigsawUI(string name, string detail)
    {
        jigsawUI.SetActive(true);
        for (int i = 0; i < JigsawAmount; i++)
        {
            if (JigsawControlList[i])
            {
                jigsawImges[i].SetActive(true);
            }
            else
            {
                jigsawImges[i].SetActive(false);
            }
        }
        detailText = jigsawUI.transform.GetChild(2).GetComponent<Text>();
        objectName = jigsawUI.transform.GetChild(3).GetComponent<Text>();
        objectName.text = name;
        detailText.text = detail;
        Time.timeScale = 0;
    }

    public void CallTalkUI(string detail)
    {
        //显示简短UI
        // BackGround = talkWindow.transform.GetChild(0).GetComponent<Image>();
        detailText = talkWindow.transform.GetComponentInChildren<Text>();
        //characterImage = talkWindow.transform.GetChild(2).GetComponent<Image>();
        //characterImage.sprite = showCharacter;

        detailText.text = detail;
        talkWindow.SetActive(true);
        Time.timeScale = 0;
    }
    public void CallDetailUI(string name, string detail, Sprite showObject)
    {
        //显示物品信息UI
        //BackGround = PopUpWindow.transform.GetChild(0).GetComponent<Image>();
        detailText = PopUpWindow.transform.GetChild(2).GetComponent<Text>();
        Image characterImage = PopUpWindow.transform.GetChild(3).GetComponent<Image>();
        objectName = PopUpWindow.transform.GetChild(4).GetComponent<Text>();

        characterImage.sprite = showObject;
        objectName.text = name;
        detailText.text = detail;
        PopUpWindow.SetActive(true);
        Time.timeScale = 0;
    }
    private void InitComponent()
    {
        bagUIList = bagUIOBJ.GetComponentsInChildren<Image>();
        /* foreach (var test in bagUIList)
        {
            Debug.Log("UI:" + test.name);
        } */
        UpdateBagUI(new Dictionary<int, ItemInfo>());
    }
    public void UpdateBagUI(Dictionary<int, ItemInfo> itemList)
    {
        int i = 0;
        foreach (var key in itemList.Keys)
        {
            bagUIList[i].sprite = itemList[key].image;
            bagUIList[i].gameObject.SetActive(true);
            i++;
            //如果超出上限，就不再显示
            if (i >= bagUIList.Length) break;
        }
        for (int j = i; j < bagUIList.Length; j++)
        {
            bagUIList[j].gameObject.SetActive(false);
        }
    }


    private void initUIObject()
    {
        for (int i = 0; i < JigsawAmount; i++)
        {
            JigsawControlList.Add(false);
            jigsawImges.Add(totalJigsaw.GetChild(i).gameObject);
        }
        //对话式UI
        //talkWindow = GameObject.FindWithTag("UI").transform.GetChild(0).gameObject;
        talkWindow.SetActive(false);

        //弹窗式UI
        //PopUpWindow = GameObject.FindWithTag("UI").transform.GetChild(1).gameObject;
        PopUpWindow.SetActive(false);

        jigsawUI.SetActive(false);
    }
    public void resetTimeScale()
    {
        Time.timeScale = 1;
    }
    public void addJigsaw()
    {
        CurrentJigsawAmount += 1;
    }
    public bool remainJigsaw()
    {
        if (JigsawAmount == CurrentJigsawAmount)
            return true;
        else return false;
    }
    public int getAllWindow()
    {
        if (popWindow == 0)
        {
            return 0;
        }
        else
        {
            popWindow = popWindow - 1;
            return popWindow;

        }
    }
    public void getPopWindowNum(int num)
    {
        popWindow = num;
    }

}

