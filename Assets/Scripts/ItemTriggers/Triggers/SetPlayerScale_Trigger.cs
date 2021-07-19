using UnityEngine;

public class SetPlayerScale_Trigger : ITrigger
{
    public float scaleRatio = 1;
    public override void Action()
    {
        GameManager.Instance.playerController.SetScale(scaleRatio);
        //        target.GetComponent<ItemTrigger>().StartTrigger();
    }
}