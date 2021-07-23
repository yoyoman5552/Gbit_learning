using UnityEngine;

public class ChangeRoomController : MonoBehaviour
{
    GameObject targetRoom;
    Transform targetDoor;
    Animator animator;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    public void ChangeRoom(string animName, GameObject targetRoom, Transform targetDoor)
    {
        this.targetRoom = targetRoom;
        this.targetDoor = targetDoor;
        //播放换场动画
        animator.Play(animName);
    }
    public void SwitchRoom()
    {
        GameManager.Instance.SwitchRoom(targetRoom, targetDoor);
    }
    public void StartRoom()
    {
        GameManager.Instance.StartRoom();
    }
}