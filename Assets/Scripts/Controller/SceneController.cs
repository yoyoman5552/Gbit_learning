using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    //����
    public GameObject title;
    //���ѡ��
    public bool mouseClick = false;
    //�������ж����λ��
    public GameObject canvas;
    //�������
    public GameObject titleScene;
    //ѡ�ؽ���
    public GameObject chooseScene;
    //�������
    public GameObject loadScene;

    //������水ť
    private Button buttonEnter;
    private Button buttonExit;
    //��ť�ı�_����һ
    private Text buttonEnter_text;
    private Text buttonExit_text;

    public Sprite backgroundSprite_toChange;
    public Image background;
    //ѡ�ؽ��水ť
    private Button firstLevel;
    private Button secondLevel;
    private Button thirdLevel;
    //��ť�ı�_������
    private Text firstLevel_text;
    private Text secondLevel_text;
    private Text thirdLevel_text;



    //�ı���ɫ
    private Color initColor;
    public Color changeColor;


    //����ѡ�񣨱�����棩
    private int Scene1Select;
    //����ѡ��ѡ�ؽ��棩
    private int Scene2Select;

    //��ǰ����
    private int currentScene;

    //�Ƿ��¼��̰���
    private bool keyDown = false;

    //ת����Ļ
    public GameObject blackCover;
    private Image coverBlack;

    //���ڽ���/�˳�����
    private bool exitScene1 = false;
    private bool enterScene2 = false;
    private bool exitScene2 = false;
    private bool enterScene3 = false;


    public float initLoadSceneEnter;
    private float loadSceneEnter;
    public float initLoadSceneEnterLoad;
    private float loadSceneEnterLoad;
    //�صƹ���ʱ��
    public float initLoadSceneTime;
    private float loadSceneTime;

    //���Ʊ�����δ��ȫ�������ʱ��ֹ�س�����
    private bool canChangeScene = true;

    //��Ļ͸��ͨ�����Ʊ���
    private Vector4 coverBlackSet;


    //���볡����ر���

    //�������������
    private bool scene3Loaded;

    //���������Ƶ
    public GameObject loadAudio;
    private AudioSource loadAudioSource;

    //��Ļ
    private Text subtitle;

    public List<string> subtitle_show = new List<string>();

    //ÿ����Ļ��ʾʱ��
    private float subtitleShowTime;
    public float initSubtitleShowTime;
    //ÿ����Ļ�ɵ�����/����䵭��ʱ��
    private float subtitleLoadTime;
    private float subtitleDisTime;
    public float initSubtitleLoadTime;
    //��ǰʱ�ڼ�����Ļ
    private int subtitleNum = 0;
    //����text͸����
    private Vector4 textColor;
    //��ʼ������Ϸ����
    private bool wordFinish = false;


    //��ť��Ƶ
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
        //�����������
        

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
                //�����ǳ
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
                //��ǰֻ�е�һ�ؿ���ѡ
                exitScene2 = true;

            }
            blackCover.SetActive(true);
            mouseClick = false;
        }
    }

    private void ComponentInit()
    {
        coverBlack = blackCover.GetComponent<Image>();

        //����һ��ť��ʼ��
        buttonEnter = titleScene.transform.GetChild(0).GetComponent<Button>();
        buttonExit = titleScene.transform.GetChild(1).GetComponent<Button>();
        buttonEnter_text = buttonEnter.GetComponentInChildren<Text>();
        buttonExit_text = buttonExit.GetComponentInChildren<Text>();

        //��������ť��ʼ��
        firstLevel = chooseScene.transform.GetChild(0).GetComponent<Button>();
        secondLevel = chooseScene.transform.GetChild(1).GetComponent<Button>();
        thirdLevel = chooseScene.transform.GetChild(2).GetComponent<Button>();
        firstLevel_text = firstLevel.GetComponentInChildren<Text>();
        secondLevel_text = secondLevel.GetComponentInChildren<Text>();
        thirdLevel_text = thirdLevel.GetComponentInChildren<Text>();

        //������
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
        if (Input.GetKeyDown(KeyCode.W) && !keyDown)
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
        if (Input.GetKeyUp(KeyCode.W))
        {
            keyDown = false;
        }
        if (Input.GetKeyDown(KeyCode.S) && !keyDown)
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
        if (Input.GetKeyUp(KeyCode.S))
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
