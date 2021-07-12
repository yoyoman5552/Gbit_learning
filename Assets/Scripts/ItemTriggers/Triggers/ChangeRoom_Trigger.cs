using EveryFunc;
using UnityEngine;
//关闭EButton，关闭执行
public class ChangeRoom_Trigger : ITrigger
{
    [Tooltip("切换的目标房间")]
    public GameObject targetRoom;
    [Tooltip("玩家切换到目标门前")]
    public GameObject targetDoor;
    private Vector3 targetPlayerPos;
    private void Start()
    {
        if (targetDoor.transform.Find("PlayerPos") != null)
            targetPlayerPos = targetDoor.transform.Find("PlayerPos").position;
        else
        {
            Debug.LogError("门尚未设置玩家出生位置");
        }
    }
    public override void Action()
    {
        //TODO:切换房间
        if (targetRoom != null)
        {
            Debug.Log("切换房间：" + targetRoom.name);
            GameManager.Instance.ChangeRoom(targetRoom, targetPlayerPos);
        }
        else
        {
            Debug.Log("切换房间Trigger为空：" + this.gameObject.name);
        }
    }
}