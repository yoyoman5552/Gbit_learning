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
        //�����ƶ�
        
        //TODO:��������
        //        target.GetComponent<ItemTrigger>().StartTrigger();


    }
    /*
    public void PlayAudio(UnityAction callback = null)
    {
        //ִ��Э�ɻ�ȡ��Ƶ�ļ���ʱ��
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