using EveryFunc;
using UnityEngine;
using System;
using UnityEngine.UI;
//�ر�EButton���ر�ִ��
//TODO:UIĿǰ����������
public class exit_Trigger : ITrigger
{
    public ExitGame getChip;
    
    public override void Action()
    {
        //TODO:�л�����
        //Debug.Log("UIDetailShow:���ƣ�" + name + ",��ϸ��" + detail);
        //TODO:��ͬ����µ��ò�ͬ����
        //UIManager.Instance.CallTalkUI(detail);

        getChip.showChip = true;
    }

}