using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    //线性流程

    //远程控制

    public bool showChip = false;
    private bool detailHadShow = false;


    //detail ui 变量
    public new string name;
    public Sprite objectImage;
    public string detail_title;
    public string detail_index;

    private bool beginBlack = false;
    private bool beginWhite = false;

    public Image blackBackground;
    private Vector4 initColor_w;
    public GameObject background_true;



    //屏幕渐黑时间
    public float initLoadTime;
    private float loadTime;


    public List<string> showText = new List<string>();
    public Text MyshowText;

    private bool loadWord = false;
    private int currentText = 1;

    public GameObject currentRoom;
    // Start is called before the first frame update
    void Start()
    {
        loadTime = initLoadTime;
        initColor_w = blackBackground.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(showChip)
        {
            if(detailHadShow == false)
            {
                UIManager.Instance.CallDetailUI(name, detail_title, detail_index, objectImage, false);
                Invoke("screenBlack", 0.1f);
                detailHadShow = true;
            }
            //屏幕变黑过程
            if(beginBlack)
            {
                loadTime -= Time.deltaTime;
                if(loadTime>0)
                {
                    initColor_w.w += Time.deltaTime / initLoadTime;
                    blackBackground.color = initColor_w; 
                }
                else
                {
                    beginWhite = true;
                    beginBlack = false;
                    loadTime = initLoadTime;

                    //将所有物品不显示
                    GameManager.Instance.player.SetActive(false);
                    currentRoom.SetActive(false);

                    //显示背景
                    background_true.SetActive(true);
                }
            }
            if(beginWhite)
            {
                loadTime -= Time.deltaTime;
                if (loadTime > 0)
                {
                    initColor_w.w -= Time.deltaTime / initLoadTime;
                    blackBackground.color = initColor_w;
                    
                }
                else
                {
                    //屏幕彻底亮起，开始文字显示
                    loadWord = true;
                }
            }
            if(loadWord)
            {
                MyshowText.text = showText[currentText - 1];
            }







        }
    }
    private void screenBlack()
    {
        beginBlack = true;
    }
}


        
