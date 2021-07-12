
using UnityEngine;
public class VoiceActive_Trigger : ITrigger
{
    public AudioClip clip;
    public override void Action()
    {
        GameManager.Instance.playerController.source.clip = clip;
        GameManager.Instance.playerController.source.Play();
        //TODO:播放音乐
        //        target.GetComponent<ItemTrigger>().StartTrigger();
    }
}