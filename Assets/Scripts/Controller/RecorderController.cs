using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderController : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    private float Clip1timer;
    private float Clip2timer;
    private float Clip3timer;
    public bool active = false;
    private bool GetClip1_able = true;
    private bool clip1Play = true;
    private bool clip2Play = false;
    private bool clip3Play = false;

    //public string talkIndex1;
    public string talkIndex2;
    public string talkIndex3;
    public string talkIndex4;
    public string talkIndex5;
    // Start is called before the first frame update
    void Start()
    {
        Clip1timer = clip1.length;
        Clip2timer = clip2.length;
        Clip3timer = clip3.length;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if (GetClip1_able)
            {
                GameManager.Instance.playerController.source.clip = clip1;
                GameManager.Instance.playerController.source.Play();
                GameManager.Instance.playerController.dontWalkAPI(Clip1timer+Clip2timer+Clip3timer+0.5f);
                GameManager.Instance.playerChildController.fightController(Clip1timer+Clip2timer+Clip3timer+0.5f);
                GetClip1_able = false;
            }
            if (clip1Play)
            {
                Clip1timer -= Time.deltaTime;
                if (Clip1timer < 0)
                {
                    UIManager.Instance.CallTalkUI(talkIndex2);
                    Invoke("startPlay2", 0.1f);
                    clip1Play = false;
                    
                }
            }
            if(clip2Play)
            {
                Clip2timer -= Time.deltaTime;
                if(Clip2timer<0)
                {
                    UIManager.Instance.CallTalkUI(talkIndex3);
                    Invoke("startPlay3", 0.1f);
                    clip2Play = false;
                }
            }

            if(clip3Play)
            {
                Clip3timer -= Time.deltaTime;
                if(Clip3timer<0)
                {
                    UIManager.Instance.CallTalkUI(talkIndex4);
                    Invoke("lastTalkUI", 0.1f);
                    clip3Play = false;
                    active = false;
                }
            }
        }
    }
    private void startPlay2()
    {
        GameManager.Instance.playerController.source.clip = clip2;
        GameManager.Instance.playerController.source.Play();
        clip2Play = true;
    }
    private void startPlay3()
    {
        GameManager.Instance.playerController.source.clip = clip3;
        GameManager.Instance.playerController.source.Play();
        clip3Play = true;
    }
    private void lastTalkUI()
    {
        UIManager.Instance.CallTalkUI(talkIndex5);
    }
}

