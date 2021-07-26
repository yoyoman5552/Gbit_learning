using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    //标题
    public GameObject title;
    //鼠标选择
    public bool mouseClick = false;
    //画布：判断鼠标位置
    public GameObject canvas;
    //标题界面
    public GameObject titleScene;
    //选关界面
    public GameObject chooseScene;
    //载入界面
    public GameObject loadScene;

    //标题界面按钮
    private Button buttonEnter;
    private Button buttonExit;
    //按钮文本_场景一
    private Text buttonEnter_text;
    private Text buttonExit_text;

    public Sprite backgroundSprite_toChange;
    public Image background;
    //选关界面按钮
    private Button firstLevel;
    private Button secondLevel;
    private Button thirdLevel;
    //按钮文本_场景二
    private Text firstLevel_text;
    private Text secondLevel_text;
    private Text thirdLevel_text;



    //文本颜色
    private Color initColor;
    public Color changeColor;


    //键盘选择（标题界面）
    private int Scene1Select;
    //键盘选择（选关界面）
    private int Scene2Select;

    //当前界面
    private int currentScene;

    //是否按下键盘按键
    private bool keyDown = false;

    //转场黑幕
    public GameObject blackCover;
    private Image coverBlack;

    //正在进入/退出界面
    private bool exitScene1 = false;
    private bool enterScene2 = false;
    private bool exitScene2 = false;
    private bool enterScene3 = false;


    public float initLoadSceneEnter;
    private float loadSceneEnter;
    public float initLoadSceneEnterLoad;
    private float loadSceneEnterLoad;
    //关灯过程时间
    public float initLoadSceneTime;
    private float loadSceneTime;

    //控制变量：未完全载入界面时禁止回车功能
    private bool canChangeScene = true;

    //黑幕透明通道控制变量
    private Vector4 coverBlackSet;


    //载入场景相关变量

    //载入界面加载完成
    private bool scene3Loaded;

    //载入界面音频
    public GameObject loadAudio;
    private AudioSource loadAudioSource;

    //字幕
    private Text subtitle;

    public List<string> subtitle_show = new List<string>();

    //每行字幕显示时间
    private float subtitleShowTime;
    public float initSubtitleShowTime;
    //每行字幕由淡变深/由深变淡的时间
    private float subtitleLoadTime;
    private float subtitleDisTime;
    public float initSubtitleLoadTime;
    //当前时第几行字幕
    private int subtitleNum = 0;
    //控制text透明度
    private Vector4 textColor;
    //开始进入游戏场景
    private bool wordFinish = false;


    //按钮音频
    public AudioSource buttonAudio;
    public AudioClip enterGameAudio;
    public AudioClip enterGameAudio2;
    // Start is called before the first frame update
    void Start()
    {
        ComponentInit();
        loadSceneEnterLoad = initLoadSceneEnterLoad;
        loadSceneEnter = initLoadSceneEnter;
        textColor = subtitle.color;
        subtitleShowTime = initSubtitleShowTime;
        subtitleLoadTime = initSubtitleLoadTime;
        subtitleDisTime = initSubtitleLoadTime;
        initColor = buttonEnter_text.color;
        currentScene = 1;
        Scene1Select = 1;
        Scene2Select = 1;
        coverBlackSet = coverBlack.color;
        loadSceneTime = initLoadSceneTime;
        blackCover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkScene();
        GetOverUI(canvas);
        selectItem();
        inputEnter();
        SceneChange();
        loadLastScene();
        //载入界面运行
        

    }

    private void loadLastScene()
    {
        if (scene3Loaded)
        {
            //StartCoroutine(showText());
            loadScene.SetActive(true);
            subtitle.text = subtitle_show[subtitleNum];

            subtitleShowTime -= Time.deltaTime;
            if (subtitleShowTime > 0)
            {
                subtitleDisTime = initSubtitleLoadTime;
                subtitleLoadTime -= Time.deltaTime;
                if (subtitleLoadTime > 0)
                {
                    textColor.w += Time.deltaTime / initSubtitleLoadTime;
                    if (textColor.w > 1) textColor.w = 1;
                    subtitle.color = textColor;
                }

            }
            else
            {
                //字体变浅
                subtitleLoadTime = initSubtitleLoadTime;
                subtitleDisTime -= Time.deltaTime;
                if (subtitleDisTime > 0)
                {
                    textColor.w -= Time.deltaTime / initSubtitleLoadTime;
                    if (textColor.x < 0) textColor.w = 0;
                    subtitle.color = textColor;
                }
                else
                {

                    subtitleShowTime = initSubtitleShowTime;
                    subtitleNum++;
                    if (subtitleNum > subtitle_show.Count - 1)
                    {
                        scene3Loaded = false;
                        wordFinish = true;
                    }
                }
            }
        }
    }

    private void SceneChange()
    {
        if (exitScene1)
        {

            canChangeScene = false;
            loadSceneEnter -= Time.deltaTime;
            if (loadSceneEnter > 0)
            {

                coverBlackSet.w += (Time.deltaTime * 255 / initLoadSceneEnter) / 255;
                coverBlack.color = coverBlackSet;
            }

            else
            {
                exitScene1 = false;
                enterScene2 = true;
                currentScene = 2;
                loadSceneEnter = initLoadSceneEnter;
                title.SetActive(false);
            }
        }
        else if (enterScene2)
        {
            loadSceneEnter -= Time.deltaTime;
            if (loadSceneEnter > 0)
            {
                coverBlackSet.w -= (Time.deltaTime * 255 / initLoadSceneEnter) / 255;
                coverBlack.color = coverBlackSet;
            }
            else
            {
                canChangeScene = true;
                enterScene2 = false;
                loadSceneEnter = initLoadSceneEnter;
                blackCover.SetActive(false);
            }
        }
        else if (exitScene2)
        {
            canChangeScene = false;
            loadSceneEnterLoad -= Time.deltaTime;
            if (loadSceneEnterLoad > 0)
            {

                coverBlackSet.w += (Time.deltaTime * 255 / initLoadSceneEnterLoad) / 255;
                coverBlack.color = coverBlackSet;
            }

            else
            {
                background.sprite = backgroundSprite_toChange;
                exitScene2 = false;
                enterScene3 = true;
               
                loadAudio.SetActive(true);
                currentScene = 3;
                loadSceneEnterLoad = initLoadSceneEnterLoad;
            }

        }
        else if (enterScene3)
        {
            loadSceneEnterLoad -= Time.deltaTime;
            if (loadSceneEnterLoad > 0)
            {
                loadAudioSource.volume += Time.deltaTime / initLoadSceneEnterLoad;
                if (loadAudioSource.volume > 1) loadAudioSource.volume = 1;
                coverBlackSet.w -= (Time.deltaTime * 255 / initLoadSceneEnterLoad) / 255;
                coverBlack.color = coverBlackSet;
            }
            else
            {
                canChangeScene = true;
                enterScene3 = false;
                scene3Loaded = true;
                loadSceneEnterLoad = initLoadSceneEnterLoad;
                blackCover.SetActive(false);
            }
        }
        if(wordFinish)
        {
            blackCover.SetActive(true);
            loadSceneTime -= Time.deltaTime;
            if(loadSceneTime>0)
            {
                loadAudioSource.volume -= Time.deltaTime / initLoadSceneTime;
                coverBlackSet.w += (Time.deltaTime * 255 / initLoadSceneTime) / 255;
                coverBlack.color = coverBlackSet;
            }
            else
            {
                loadSceneTime = initLoadSceneTime;
                wordFinish = false;
                SceneManager.LoadScene("Main Scene");
            }
        }
    }
    private void inputEnter()
    {
        if ((Input.GetKeyDown(KeyCode.Return)||mouseClick) && canChangeScene)
        {
            if (currentScene == 1)
            {
                buttonAudio.PlayOneShot(enterGameAudio);
                if (Scene1Select == 1)
                {
                    exitScene1 = true;
                   
                }
                else if (Scene1Select == 2)
                    Application.Quit();
            }
            else if (currentScene == 2)
            {
                buttonAudio.PlayOneShot(enterGameAudio2);
                //当前只有第一关卡可选
                exitScene2 = true;

            }
            blackCover.SetActive(true);
            mouseClick = false;
        }
    }

    private void ComponentInit()
    {
        coverBlack = blackCover.GetComponent<Image>();

        //场景一按钮初始化
        buttonEnter = titleScene.transform.GetChild(0).GetComponent<Button>();
        buttonExit = titleScene.transform.GetChild(1).GetComponent<Button>();
        buttonEnter_text = buttonEnter.GetComponentInChildren<Text>();
        buttonExit_text = buttonExit.GetComponentInChildren<Text>();

        //场景二按钮初始化
        firstLevel = chooseScene.transform.GetChild(0).GetComponent<Button>();
        secondLevel = chooseScene.transform.GetChild(1).GetComponent<Button>();
        thirdLevel = chooseScene.transform.GetChild(2).GetComponent<Button>();
        firstLevel_text = firstLevel.GetComponentInChildren<Text>();
        secondLevel_text = secondLevel.GetComponentInChildren<Text>();
        thirdLevel_text = thirdLevel.GetComponentInChildren<Text>();

        //场景三
        loadAudioSource = loadAudio.GetComponent<AudioSource>();
        loadAudioSource.volume = 0;
        loadAudio.SetActive(false);
        subtitle = loadScene.GetComponentInChildren<Text>();
    }


    private void checkScene()
    {
        switch(currentScene)
        {
            case 1:
                titleScene.SetActive(true);
                chooseScene.SetActive(false);
                break;
            case 2:
                titleScene.SetActive(false);
                chooseScene.SetActive(true);
                break;
            case 3:
                titleScene.SetActive(false);
                chooseScene.SetActive(false);
                break;

        }
    }

    private void selectItem()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !keyDown)
        {
            keyDown = true;

            if (currentScene == 1)
            {
                Scene1Select -= 1;
                if (Scene1Select <1)
                    Scene1Select = 2;
            }
            else if(currentScene == 2)
            {
                Scene2Select -= 1;
                if (Scene2Select <1)
                    Scene2Select = 1;
            }

        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            keyDown = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !keyDown)
        {
            keyDown = true;
            if (currentScene == 1)
            {
                Scene1Select += 1;
                if (Scene1Select > 2)
                    Scene1Select = 1;
            }
            else if (currentScene == 2)
            {
                Scene2Select += 1;
                if (Scene2Select >1)
                    Scene2Select = 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            keyDown = false;
        }

        if (currentScene == 1)
        {
            if (Scene1Select == 1)
            {
                buttonEnter_text.color = changeColor;
                buttonExit_text.color = initColor;
            }
            else if (Scene1Select == 2)
            {
                buttonExit_text.color = changeColor;
                buttonEnter_text.color = initColor;
            }

        }
        else if(currentScene == 2)
        {
            if (Scene2Select == 1)
            {
                firstLevel_text.color = changeColor;
                //secondLevel_text.color = initColor;
                //thirdLevel_text.color = initColor;
            }
            else if (Scene2Select == 2)
            {
                
                firstLevel_text.color = initColor;
                secondLevel_text.color = changeColor;
                thirdLevel_text.color = initColor;
                
            }
            else if(Scene2Select == 3)
            {
                firstLevel_text.color = initColor;
                secondLevel_text.color = initColor;
                thirdLevel_text.color = changeColor;
            }

        }
        
    }


    private void GetOverUI(GameObject canvas)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            foreach(RaycastResult myresult in results)
            {
                if (currentScene == 1)
                {
                    if (myresult.gameObject.name == buttonEnter.name)
                    {
                        Scene1Select = 1;
                    }
                    else if (myresult.gameObject.name == buttonExit.name)
                    {
                        Scene1Select = 2;
                    }
                }
                else if(currentScene ==2)
                {
                    if (myresult.gameObject.name == firstLevel.name)
                    {
                        Scene2Select = 1;
                    }
                    else if (myresult.gameObject.name == secondLevel.name)
                    {
                        //Scene2Select = 2;
                    }
                    else if((myresult.gameObject.name == thirdLevel.name))
                    {
                        //Scene2Select = 3;
                    }
                }                
            }
        }
    }
}
