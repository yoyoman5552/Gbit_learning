using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using EveryFunc;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using EveryFunc.FSM;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //景深
    private DepthOfField depthOfField;
    //色差
    private ChromaticAberration chromaticAberration;

    //单例模式
    public static GameManager Instance;
    //玩家
    [HideInInspector]
    public GameObject player;
    //玩家属性
    [HideInInspector]
    public PlayerController playerController;
    public PlayerChildController playerChildController;
    [Tooltip("当前房间")]
    [HideInInspector]
    public GameObject currentRoom;
    [Tooltip("黑色UI图片")]
    public GameObject BlackImage;
    [Tooltip("玩家受伤效果")]
    public Sprite[] bloodPicture;
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

    //保存数据
    public SaveData saveData;
    private int bloodIndex;
    private SpriteRenderer bloodRenderer;
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
    private void Update()
    {
        if (bloodRenderer.color.a > 0)
        {
            bloodRenderer.color = bloodRenderer.color - new Color(0, 0, 0, Time.deltaTime / 5);
        }
    }
    private void InitComponent()
    {
        bloodIndex = 0;
        saveData = new SaveData();
        bloodRenderer = bloodEffect.GetComponent<SpriteRenderer>();
        globalLight = this.GetComponentInChildren<Light2D>();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerChildController = player.GetComponentInChildren<PlayerChildController>();
        roomList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Room"));
        Debug.Log("roomList length:" + roomList.Count);
        foreach (var room in roomList)
        {
            Debug.Log("roomList+:" + room.name);
            room.transform.position = Vector3.zero;
            room.SetActive(false);
        }
        //FIXME:选择初始房间
        GameObject firstRoom = roomList.Find(s => s.name == "Room1");
        ChangeRoom(firstRoom, this.transform.Find("StartPos"), null);

        Volume[] volumes = VolumeManager.instance.GetVolumes(LayerMask.NameToLayer("All"));
        foreach (var volume in volumes)
        {
            var profile = volume.sharedProfile;
            foreach (var component in profile.components)
            {
                if (component.GetType() == typeof(ChromaticAberration))
                    chromaticAberration = (ChromaticAberration)component;
                else if (component.GetType() == typeof(DepthOfField))
                    depthOfField = (DepthOfField)component;
            }
        }
    }
    /// <summary>
    /// 切换房间
    /// </summary>
    /// <param name="targetRoom">目标房间</param>
    public void ChangeRoom(GameObject targetRoom, Transform targetDoor, Transform lastDoor, ChangeRoomType changeRoomType = ChangeRoomType.normal)
    {
        SaveData(targetRoom, lastDoor);
        playerController.SetReactable(false);
        if (changeRoomType == ChangeRoomType.normal)
        {
            normalChangeRoom(targetRoom, targetDoor);
        }
    }
    private void normalChangeRoom(GameObject targetRoom, Transform targetDoor)
    {
        BlackImage.SetActive(true);
        if (player.transform.position.x >= 0)
            StartCoroutine(ChangeRoomDelay(1, targetRoom, targetDoor));
        else
            StartCoroutine(ChangeRoomDelay(-1, targetRoom, targetDoor));
    }
    private IEnumerator ChangeRoomDelay(float dir, GameObject targetRoom, Transform targetDoor)
    {
        bool hasChanged = false;
        float UIPosX, moveSpeed = UIMoveSpeed * Time.fixedDeltaTime * ConstantList.speedPer;
        if (dir == 1)
        {
            UIPosX = UIRightPos;
            BlackImage.transform.position = new Vector3(UILeftPos, BlackImage.transform.position.y, BlackImage.transform.position.z);
        }
        else
        {
            UIPosX = UILeftPos;
            BlackImage.transform.position = new Vector3(UIRightPos, BlackImage.transform.position.y, BlackImage.transform.position.z);
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
                SwitchRoom(targetRoom, targetDoor, dir);
            }
        }
        BlackImage.SetActive(false);
        playerController.SetReactable(true);
    }
    private void SwitchRoom(GameObject targetRoom, Transform targetDoor, float dir)
    {
        //切换房间
        if (currentRoom != null)
            currentRoom.SetActive(false);

        targetRoom.SetActive(true);
        currentRoom = targetRoom;
        //人物位置设置
        Transform playerPos = targetDoor.Find("PlayerPos");
        if (playerPos != null)
        {
            player.transform.position = playerPos.position;
            if (playerPos.position.x - targetDoor.position.x > 0.05f)
                playerController.Flip(1);
            else if (playerPos.position.x - targetDoor.position.x < -0.05f)
                playerController.Flip(-1);
        }
        //否则为初始房间
        else
        {
            player.transform.position = targetDoor.position;
        }
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

        //查找场景中是否存在敌人
        FSMBase[] enemy = currentRoom.transform.GetComponentsInChildren<NormalEnemyFSM>();
        if (enemy.Length > 0)
        {
            //如果有敌人，关掉人物的交互
            playerController.SetEAble(false);
        }
        //BulletPool.bulletPoolInstance.DestroyBulletsInPool();
    }
    public void CheckEnemy()
    {
        //确认场景中是否有敌人
        FSMBase[] enemy = currentRoom.transform.GetComponentsInChildren<FSMBase>();
        if (enemy.Length == 0)
        {
            playerController.SetEAble(true);
        }
    }
    public void SaveData(GameObject Room, Transform playerPos)
    {
        Debug.Log("room:" + Room);
        //保存房间和人物位置
        saveData.roomName = Room.name;
        saveData.lastDoor = playerPos;

        //保存物品列表
        saveData.itemList = new Dictionary<int, ItemInfo>(BagManager.Instance.itemList);

        //保存敌人数据
        saveData.enemys.Clear();
        foreach (var enemy in Room.transform.GetComponentsInChildren<FSMBase>(true))
        {
            FSMBase fsm = enemy.GetComponent<FSMBase>();
            EnemyData enemyData = new EnemyData(fsm.AttackStyle, fsm.HP, fsm.transform.localPosition);
            saveData.enemys.Add(enemyData);
        }
    }
    //更新受击特效
    public void UpdateHurtedEffect(float MaxHP, float m_hp)
    {
        if (bloodRenderer.color.a <= 0) bloodIndex = 0;
        bloodRenderer.sprite = bloodPicture[bloodIndex];
        Vector4 x = bloodRenderer.color;
        x.w = 1;
        bloodRenderer.color = x;
        if (bloodIndex + 1 >= bloodPicture.Length)
        {//如果人物死亡
            StartCoroutine(RestartGameDelay());
            Time.timeScale = 0;
            //            PlayerDead();
        }
        bloodIndex = (bloodIndex + 1) % bloodPicture.Length;
    }
    IEnumerator RestartGameDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        PlayerDead();
    }
    public void PlayerDead()
    {
        Debug.Log("角色死亡");
        //TODO:死亡UI显示blahblah
        BagManager.Instance.itemList = saveData.itemList;
        playerController.PlayerInit();
        ChangeRoom(roomList.Find(s => s.name == saveData.lastDoor.parent.name), saveData.lastDoor, null);

        //初始化敌人
    }
}