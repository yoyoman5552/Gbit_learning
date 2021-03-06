using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ExitGame : MonoBehaviour
{
    //线性流程

    //远程控制

    public bool showChip = false;
    private bool detailHadShow = false;

    public Sprite changeSprite;
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


    public List<Text> showText = new List<Text>();
    public Text MyshowText;

    private bool loadWord = false;
    private int currentText = 0;

    public GameObject currentRoom;

    private float subtitleLoadTime;
    private float subtitleDisTime;
    public float initSubtitleLoadTime;


    private float wordShowTime;
    public float initWordShowTime;
    private Vector4 TextColor;

    private bool reLoad = false;
    private bool enterInit = false;
    // Start is called before the first frame update1
    void Start()
    {
        wordShowTime = initWordShowTime;
        loadTime = initLoadTime;
        initColor_w = blackBackground.color;
        subtitleLoadTime = initSubtitleLoadTime;
        subtitleDisTime = initSubtitleLoadTime;
        TextColor = MyshowText.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (showChip)
        {
            if (detailHadShow == false)
            {
                UIManager.Instance.CallDetailUI(name, detail_title, detail_index, objectImage, false);
                Invoke("screenBlack", 0.1f);
                detailHadShow = true;
            }
            //屏幕变黑过程
            if (beginBlack)
            {
                loadTime -= Time.deltaTime;
                if (loadTime > 0)
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
            if (beginWhite)
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
                    loadTime = initLoadTime;
                    beginWhite = false;
                }
            }
            if (loadWord)
            {
                MyshowText.text = showText[currentText].text;
                //MyshowText = showText[currentText-1];
                wordShowTime -= Time.deltaTime;
                //字体逐渐显示
                if (wordShowTime > 0)
                {
                    subtitleDisTime = initSubtitleLoadTime;
                    subtitleLoadTime -= Time.deltaTime;
                    if (subtitleLoadTime > 0)
                    {
                        TextColor.w += Time.deltaTime / initSubtitleLoadTime;
                        if (TextColor.w > 1) TextColor.w = 1;
                        MyshowText.color = TextColor;
                    }
                }
                else
                {
                    subtitleLoadTime = initSubtitleLoadTime;
                    subtitleDisTime -= Time.deltaTime;
                    if (subtitleDisTime > 0)
                    {
                        //字体变淡
                        TextColor.w -= Time.deltaTime / initSubtitleLoadTime;
                        if (TextColor.w < 0) TextColor.w = 0;
                        MyshowText.color = TextColor;
                    }
                    else
                    {
                        wordShowTime = initWordShowTime;
                        currentText++;
                        if (currentText > showText.Count - 1)
                        {
                            
                            loadWord = false;
                            //屏幕渐黑/=
                            reLoad = true;
                        }
                    }
                }
            }
            if(reLoad)
            {
                loadTime -= Time.deltaTime;
                if (loadTime > 0)
                {
                    initColor_w.w += Time.deltaTime / initLoadTime;
                    blackBackground.color = initColor_w;
                }
                else
                {
                    background_true.GetComponent<SpriteRenderer>().sprite = changeSprite;
                    reLoad = false;
                    enterInit = true;
                    loadTime = initLoadTime;
                }
            }
            if(enterInit)
            {
                loadTime -= Time.deltaTime;
                if(loadTime>0)
                {
                    initColor_w.w -= Time.deltaTime/initLoadTime;
                    blackBackground.color = initColor_w;
                }
                else
                {
                    enterInit = false;
                    SceneManager.LoadScene("newLoad");
                }
            }

        }
    }
    private void screenBlack()
    {
        beginBlack = true;
    }
}


        
