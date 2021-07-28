using UnityEngine;

public class SaveData_Trigger : ITrigger
{
    public GameObject TargetRoom;
    public Transform TargetDoor;
    public override void Action()
    {
        GameManager.Instance.SaveData(TargetRoom, TargetDoor);
    }
}