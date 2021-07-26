
using UnityEngine;
using EveryFunc;
public class BGMManager : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource source;
    private BGMType currentType;
    public static BGMManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        Init();
    }
    private void Init()
    {
        ChangeBGM(BGMType.Stop);
    }
    public void ChangeBGM(BGMType changeType)
    {
        Debug.Log("bgm change:" + currentType.ToString() + " to " + changeType.ToString());
        //如果是同一个bgm，就不切换了
        if (currentType == changeType) return;
        else
        {
            currentType = changeType;
//            Debug.Log("now type:"+currentType);
        }
        source.clip = clips[(int)currentType];
        source.Play();  
        //if (currentType)
    }
}