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
    //背包UI列表
    [HideInInspector]
    public Image[] bagUIList;
    private Text detailText;
    private Image characterImage;
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


    public void CallTalkUI(string detail, Sprite showCharacter)
    {
        //显示简短UI
        BackGround = talkWindow.transform.GetChild(0).GetComponent<Image>();
        detailText = talkWindow.transform.GetChild(1).GetComponent<Text>();
        characterImage = talkWindow.transform.GetChild(2).GetComponent<Image>();
        characterImage.sprite = showCharacter;

        detailText.text = detail;
        talkWindow.SetActive(true);
        Time.timeScale = 0;
    }
    public void CallDetailUI(string name, string detail, Sprite showObject)
    {
        //显示物品信息UI
        BackGround = PopUpWindow.transform.GetChild(0).GetComponent<Image>();
        detailText = PopUpWindow.transform.GetChild(1).GetComponent<Text>();
        characterImage = PopUpWindow.transform.GetChild(2).GetComponent<Image>();
        objectName = PopUpWindow.transform.GetChild(3).GetComponent<Text>();

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
        //对话式UI
        talkWindow = GameObject.FindWithTag("UI").transform.GetChild(0).gameObject;
        talkWindow.SetActive(false);

        //弹窗式UI
        PopUpWindow = GameObject.FindWithTag("UI").transform.GetChild(1).gameObject;
        PopUpWindow.SetActive(false);
    }
    public void resetTimeScale()
    {
        Time.timeScale = 1;
    }

}

