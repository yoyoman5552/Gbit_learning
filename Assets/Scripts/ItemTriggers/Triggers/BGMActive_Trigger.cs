using UnityEngine;
public class BGMActive_Trigger : ITrigger {
    public AudioClip clip;
    [Tooltip ("是否允许播放")]
    public bool isPlay = true;
    public override void Action () {
        if (!isPlay) {
            GameManager.Instance.bgmPlayer.clip = null;
            GameManager.Instance.bgmPlayer.Stop ();
        } else {
            GameManager.Instance.bgmPlayer.clip = clip;
            GameManager.Instance.bgmPlayer.Play ();
        }
        //TODO:播放音乐
        //        target.GetComponent<ItemTrigger>().StartTrigger();
    }
}