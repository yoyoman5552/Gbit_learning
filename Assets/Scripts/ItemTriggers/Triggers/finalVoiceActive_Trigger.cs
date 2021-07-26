using UnityEngine.Events;
using UnityEngine;
using System.Collections;
public class finalVoiceActive_Trigger : ITrigger
{

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    private AudioClip currentClip;
    public string talkUI1_index;
    public string talkUI2_index;
    public string talkUI3_index;
    public override void Action()
    {
        currentClip = clip1;
        GameManager.Instance.playerController.source.clip = currentClip;
        GameManager.Instance.playerController.source.Play();

        //��ֹ�ƶ�
        GameManager.Instance.playerController.dontWalkAPI(currentClip.length);

        //��ֹ����
        GameManager.Instance.playerChildController.fightController(currentClip.length);


        //PlayAudio();
        //�����ƶ�
        
        //TODO:��������
        //        target.GetComponent<ItemTrigger>().StartTrigger();


    }
    private void callTalkUI1()
    {
        UIManager.Instance.CallTalkUI(talkUI1_index);
    }
    private void callTalkUI2()
    {
        UIManager.Instance.CallTalkUI(talkUI2_index);
    }
    private void callTalkUI3()
    {
        UIManager.Instance.CallTalkUI(talkUI3_index);
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