using UnityEngine.Events;
using UnityEngine;
using System.Collections;
public class finalVoiceActive_Trigger : ITrigger
{

    public AudioClip clip;
    public override void Action()
    {
        GameManager.Instance.playerController.source.clip = clip;
        GameManager.Instance.playerController.source.Play();
        GameManager.Instance.playerController.dontWalkAPI(clip.length);
        GameManager.Instance.playerChildController.fightController(clip.length);
        //PlayAudio();
        //不能移动
        
        //TODO:播放音乐
        //        target.GetComponent<ItemTrigger>().StartTrigger();


    }
    /*
    public void PlayAudio(UnityAction callback = null)
    {
        //执行协成获取音频文件的时间
        StartCoroutine(AudioPlayFinished(clip.length, callback));
    }
    private IEnumerator AudioPlayFinished(float time, UnityAction callback)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time);

        GameManager.Instance.playerController.resetWalkAble();

    }
    */
}