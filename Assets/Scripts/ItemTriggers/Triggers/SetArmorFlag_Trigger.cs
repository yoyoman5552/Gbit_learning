using UnityEngine;
public class SetArmorFlag_Trigger : ITrigger
{
    public bool ArmorFlag;
    [Tooltip("使用次数,如果ArmorFlag为false就不用管")]
    public int useNum = 999;

    public override void Action()
    {
        if (ArmorFlag == false) useNum = -1;
        GameManager.Instance.playerController.SetArmor(ArmorFlag,useNum);
    }
}