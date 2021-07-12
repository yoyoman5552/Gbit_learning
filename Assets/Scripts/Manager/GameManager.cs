using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using EveryFunc;
public class GameManager : MonoBehaviour
{
    //单例模式
    public static GameManager Instance;
    //玩家
    [HideInInspector]
    public GameObject player;
    //玩家属性
    [HideInInspector]
    public PlayerController playerController;
    [Tooltip("当前房间")]
    [HideInInspector]
    public GameObject currentRoom;
    [Tooltip("黑色UI图片")]
    public GameObject BlackImage;
    [Tooltip("玩家受伤效果")]
    public GameObject bloodEffect;
    [Tooltip("UI移动速度")]
    public float UIMoveSpeed;
    [Tooltip("UI的位置界限右")]
    public float UIRightPos;
    [Tooltip("UI的位置界限左")]
    public float UILeftPos;
    //房间列表
    [HideInInspector]
    public List<GameObject> roomList;

    //Global Light
    public Light2D globalLight;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager多重实例");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        InitComponent();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void InitComponent()
    {
        globalLight = this.GetComponentInChildren<Light2D>();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        roomList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Room"));
        Debug.Log("roomList length:" + roomList.Count);
        foreach (var room in roomList)
        {
            Debug.Log("roomList+:" + room.name);
            room.transform.position = Vector3.zero;
            room.SetActive(false);
        }
        //FIXME:选择初始房间
        ChangeRoom(roomList.Find(s=>s.name=="Room1"), Vector3.zero);
    }
    /// <summary>
    /// 切换房间
    /// </summary>
    /// <param name="targetRoom">目标房间</param>
    public void ChangeRoom(GameObject targetRoom, Vector3 playerPos)
    {
        BlackImage.SetActive(true);
        playerController.SetReactable(false);
        if (player.transform.position.x >= 0)
            StartCoroutine(ChangeRoomDelay(1, targetRoom, playerPos));
        else
            StartCoroutine(ChangeRoomDelay(-1, targetRoom, playerPos));
    }
    private IEnumerator ChangeRoomDelay(float dir, GameObject targetRoom, Vector3 playerPos)
    {
        bool hasChanged = false;
        float UIPosX, moveSpeed = UIMoveSpeed * Time.fixedDeltaTime * ConstantList.speedPer;
        if (dir == 1)
        {
            UIPosX = UIRightPos;
            BlackImage.transform.position = new Vector3(UILeftPos, 0, 0);
        }
        else
        {
            UIPosX = UILeftPos;
            BlackImage.transform.position = new Vector3(UIRightPos, 0, 0);
        }
        BlackImage.GetComponent<SpriteRenderer>().flipX = dir == 1 ? false : true;
        while (Mathf.Abs(BlackImage.transform.position.x - UIPosX) > 2f)
        {
            BlackImage.transform.position += new Vector3(moveSpeed * dir, 0, 0);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            //切换房间  
            if (Mathf.Abs(BlackImage.transform.position.x) < 2f && !hasChanged)
            {
                Debug.Log("换房间: to " + targetRoom.name);
                hasChanged = true;
                SwitchRoom(targetRoom, playerPos);
            }
        }
        BlackImage.SetActive(false);
        playerController.SetReactable(true);
    }
    private void SwitchRoom(GameObject targetRoom, Vector3 playerPos)
    {
        //切换房间
        if (currentRoom != null)
            currentRoom.SetActive(false);
        targetRoom.SetActive(true);
        currentRoom = targetRoom;
        //人物位置设置
        player.transform.position = playerPos;
        //房间初始化
        RoomInit();
    }
    private void RoomInit()
    {
        Debug.Log("房间初始化");
        Transform roomDetail = currentRoom.transform.Find("RoomDetail");
        if (roomDetail == null) Debug.LogError(currentRoom.name + " 房间没有RoomDetail");
        GridManager.Instance.LeftDownTF = roomDetail.Find("GridLimit").Find("LeftDownPos");
        GridManager.Instance.RightUpTF = roomDetail.Find("GridLimit").Find("RightUpPos");
        GridManager.Instance.Init();
    }
}