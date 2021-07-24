using EveryFunc;
using UnityEngine;
//关闭EButton，关闭执行
public class ChangeRoom_Trigger : ITrigger
{
    [Tooltip("切换的目标房间")]
    public GameObject targetRoom;
    [Tooltip("玩家切换到目标门前")]
    public GameObject targetDoor;
    private Transform targetPlayerPos;
    [Tooltip("切换门的方式")]
    public ChangeRoomType changeRoomType = ChangeRoomType.normal;
    private void Start()
    {
        if (targetDoor.transform.Find("PlayerPos") != null)
            targetPlayerPos = targetDoor.transform.Find("PlayerPos");
        else
        {
            Debug.LogError("门尚未设置玩家出生位置");
        }
    }
    public override void Action()
    {
        if (targetRoom != null)
        {
            Debug.Log("切换房间：" + targetRoom.name);
            GameManager.Instance.ChangeRoom(targetRoom, targetDoor.transform, this.transform);
        }
        else
        {
            Debug.Log("切换房间Trigger为空：" + this.gameObject.name);
        }
    }
}