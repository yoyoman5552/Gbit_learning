using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI类，关于UI的方法都放在此类，场景中UI的组件也放在UIManager下
/// </summary>
public class UIManager : MonoBehaviour
{

    public AudioClip colletBetterAudio;

    public static UIManager Instance;
    [Tooltip("背包UI")]
    public GameObject bagUIOBJ;
    //弹窗UI结构变量
    [Tooltip("对话式UI")]
    public Animator talkWindow;
    [Tooltip("物品信息UI")]
    public Animator PopUpWindow;
    [Tooltip("目标UI")]
    public GameObject targetUI;

    //拼图相关变量
    [Tooltip("拼图UI")]
    public Animator jigsawUI;
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
    public Text[] bagCountList;
    private Text detailTitle;
    private Text detailIndex;
    //private Image characterImage;
    private Image BackGround;
    private Text objectName;
    private ItemTrigger targetTrigger;

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
        jigsawUI.SetBool("IsShow", true);
        talkWindow.SetBool("IsShow", false);
        PopUpWindow.SetBool("IsShow", false);
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
        detailIndex = jigsawUI.transform.Find("detail").GetComponent<Text>();
        objectName = jigsawUI.transform.Find("name").GetComponent<Text>();
        objectName.text = name;
        detailIndex.text = detail;
        Time.timeScale = 0;
    }

    public void CallTalkUI(string detail)
    {
        //显示简短UI
        // BackGround = talkWindow.transform.GetChild(0).GetComponent<Image>();
        detailIndex = talkWindow.transform.GetComponentInChildren<Text>();
        //characterImage = talkWindow.transform.GetChild(2).GetComponent<Image>();
        //characterImage.sprite = showCharacter;

        detailIndex.text = detail;
        talkWindow.SetBool("IsShow", true);
        PopUpWindow.SetBool("IsShow", false);
        jigsawUI.SetBool("IsShow", false);
        Time.timeScale = 0;
    }
    public void CallDetailUI(string name, string detail_title, string detail_index, Sprite showObject, bool hadSound)
    {
        //显示物品信息UI
        //BackGround = PopUpWindow.transform.GetChild(0).GetComponent<Image>();
        detailTitle = PopUpWindow.transform.Find("detail_title").GetComponent<Text>();
        detailIndex = PopUpWindow.transform.Find("detail_index").GetComponent<Text>();
        Image characterImage = PopUpWindow.transform.Find("objImage").GetComponent<Image>();
        objectName = PopUpWindow.transform.Find("name").GetComponent<Text>();

        characterImage.sprite = showObject;
        objectName.text = name;
        detailTitle.text = detail_title;
        detailIndex.text = detail_index;
        Debug.Log("call detailUI:" + Input.GetKeyDown(KeyCode.E));
        PopUpWindow.SetBool("IsShow", true);
        talkWindow.SetBool("IsShow", false);
        jigsawUI.SetBool("IsShow", false);
        Time.timeScale = 0;
        if (hadSound)
        {
            GameManager.Instance.playerController.source.PlayOneShot(colletBetterAudio);
            print("inthisFunction");
        }
    }
    private void InitComponent()
    {
        bagUIList = bagUIOBJ.GetComponentsInChildren<Image>();
        bagCountList = new Text[bagUIList.Length];
        for (int i = 0; i < bagUIList.Length; i++)
        {
            bagCountList[i] = bagUIList[i].GetComponentInChildren<Text>();
        }
        /* foreach (var test in bagUIList)
        {
            Debug.Log("UI:" + test.name);
        } */
        UpdateBagUI(new Dictionary<string, ItemInfo>());
    }
    public void UpdateBagUI(Dictionary<string, ItemInfo> itemList)
    {
        int i = 0;
        List<Sprite> nameList = new List<Sprite>();
        foreach (var key in itemList.Keys)
        {
            //            Debug.Log("物品：" + itemList[key].name + ",使用次数：" + itemList[key].useNum);
            if (!nameList.Contains(bagUIList[i].sprite))
            {
                nameList.Add(bagUIList[i].sprite);
                bagUIList[i].sprite = itemList[key].image;
                bagUIList[i].gameObject.SetActive(true);
                if (itemList[key].useNum > 1)
                {
                    bagCountList[i].gameObject.SetActive(true);
                    bagCountList[i].text = itemList[key].useNum.ToString();
                }
                else
                {
                    bagCountList[i].gameObject.SetActive(false);
                    bagCountList[i].text = "";
                }
            }

            i++;
            //如果超出上限，就不再显示
            if (i >= bagUIList.Length) break;
        }
        for (int j = i; j < bagUIList.Length; j++)
        {
            bagUIList[j].gameObject.SetActive(false);
            bagCountList[j].gameObject.SetActive(false);
            bagCountList[j].text = "";
        }
    }
    public void ShowCount(int i)
    {

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
        talkWindow.SetBool("IsShow", false);

        //弹窗式UI
        //PopUpWindow = GameObject.FindWithTag("UI").transform.GetChild(1).gameObject;
        PopUpWindow.SetBool("IsShow", false);

        jigsawUI.SetBool("IsShow", false);
        SetTargetUI("");
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
    public bool CheckContinueTrigger()
    {
        //        Debug.Log("targetTrigger:" + targetTrigger);
        if (targetTrigger != null)
        {
            targetTrigger.ContinueTrigger();
        }
        return targetTrigger != null;
    }
    public void SaveActiveTrigger(ItemTrigger targetTrigger)
    {
        this.targetTrigger = targetTrigger;
    }
    public void RemoveActiveTrigger()
    {
        this.targetTrigger = null;
    }
    public void SetTargetUI(string content)
    {
        targetUI.GetComponentInChildren<Text>().text = content;
        targetUI.SetActive(true);
    }
}