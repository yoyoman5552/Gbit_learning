using UnityEngine;
public class SetArmorFlag_Trigger : ITrigger
{
    public bool ArmorFlag;
    public override void Action()
    {
        GameManager.Instance.playerController.SetArmor(ArmorFlag);
    }
}