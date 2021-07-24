
using UnityEngine;
using EveryFunc;
public class AwakeBoss_Trigger : ITrigger
{
    //public bool flag = true;
    private void Awake()
    {
    }

    public override void Action()
    {
        GameManager.Instance.currentRoom.transform.Find("Boss").GetComponent<BossFSM>().ChangeActiveState(FSMStateID.BossAttack);
    }



}