using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using EveryFunc.FSM;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    //景深
    private DepthOfField depthOfField;
    //色差
    private ChromaticAberration chromaticAberration;
    private float chromaticRatio;

    //单例模式
    public static GameManager Instance;
    /* {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<GameManager>();
            return Instance;
        }
        set
        {
            if (Instance != null)
                Debug.Log("多重实例：+GameManager");
            Instance = value;
        }
    } */
    //玩家
    [HideInInspector]
    public GameObject player;
    //玩家属性
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public PlayerChildController playerChildController;
    [Tooltip ("当前房间")]
    [HideInInspector]
    public GameObject currentRoom;
    [Tooltip ("黑色UI图片")]
    public GameObject BlackImage;
    [Tooltip ("玩家受伤效果")]
    public Sprite[] bloodPicture;
    public GameObject bloodEffect;
    [Tooltip ("UI移动速度")]
    public float UIMoveSpeed;
    [Tooltip ("UI的位置界限右")]
    public float UIRightPos;
    [Tooltip ("UI的位置界限左")]
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

    //摄像头
    //[HideInInspector]
    //public GameObject mainCamera;
    //播放背景音乐机器
    [HideInInspector]
    public AudioSource bgmPlayer;
    private void Awake () {
        if (Instance != null) {
            Debug.LogError ("GameManager多重实例");
            Destroy (this.gameObject);
            return;
        }
        Instance = this;
        InitComponent ();
    }
    private void Start () {
        //FIXME:选择初始房间
        GameObject firstRoom = roomList.Find (s => s.name == "Room1");
        ChangeRoom (firstRoom, this.transform.Find ("StartPos"), null, ChangeRoomType.Enter);
    }
    private void OnDestroy () {
        chromaticAberration.intensity.value = 0;
    }
    private void Update () {
        if (bloodRenderer.color.a > 0) {
            bloodRenderer.color = bloodRenderer.color - new Color (0, 0, 0, Time.deltaTime / 5);
        }
        if (chromaticAberration.intensity.value > 0)
            chromaticAberration.intensity.value = chromaticAberration.intensity.value - Time.deltaTime / 5;
    }
    public Volume targetVolume;
    private void InitComponent () {
        Cursor.visible = false;
        chromaticRatio = bloodIndex = 0;
        saveData = new SaveData ();
        bloodRenderer = bloodEffect.GetComponent<SpriteRenderer> ();
        // globalLight = this.GetComponentInChildren<Light2D>();
        player = GameObject.FindWithTag ("Player");
        playerController = player.GetComponent<PlayerController> ();
        playerChildController = player.GetComponentInChildren<PlayerChildController> ();
        roomList = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Room"));
        bgmPlayer = Camera.main.transform.Find ("BGMPlayer").GetComponent<AudioSource> ();
        //Debug.Log("roomList length:" + roomList.Count);
        foreach (var room in roomList) {
            Debug.Log ("roomList+:" + room.name);
            room.transform.position = Vector3.zero;
            room.SetActive (false);
        }

        // Debug.Log("volume:" + targetVolume.name);
        var profile = targetVolume.sharedProfile;
        foreach (var component in profile.components) {
            //   Debug.Log("component:" + component.GetType());
            if (component.GetType () == typeof (ChromaticAberration)) {
                chromaticAberration = (ChromaticAberration) component;
            } else if (component.GetType () == typeof (DepthOfField))
                depthOfField = (DepthOfField) component;
        }
    }
    /// <summary>
    /// 切换房间
    /// </summary>
    /// <param name="targetRoom">目标房间</param>
    public void ChangeRoom (GameObject targetRoom, Transform targetDoor, Transform lastDoor, ChangeRoomType changeRoomType = ChangeRoomType.Normal) {

        //保存数据
        SaveData (targetRoom, lastDoor);

        //不允许玩家移动，显示切换ui
        playerController.SetReactable (false);

        //判断切换方式
        if (changeRoomType == ChangeRoomType.Normal) {
            NormalChangeRoom (targetRoom, targetDoor);
        } else if (changeRoomType == ChangeRoomType.SBTimeTrip) {
            SpecialChangeRoom (targetRoom, targetDoor);
        } else if (changeRoomType == ChangeRoomType.Enter) {
            BlackImage.GetComponent<ChangeRoomController> ().ChangeRoom ("开头", targetRoom, targetDoor);
        }
    }
    private void NormalChangeRoom (GameObject targetRoom, Transform targetDoor) {
        //播放切换房间动画
        if (player.transform.position.x >= 0)
            BlackImage.GetComponent<ChangeRoomController> ().ChangeRoom ("右切", targetRoom, targetDoor);
        else
            BlackImage.GetComponent<ChangeRoomController> ().ChangeRoom ("左切", targetRoom, targetDoor);
    }
    private void SpecialChangeRoom (GameObject targetRoom, Transform targetDoor) {
        BlackImage.GetComponent<ChangeRoomController> ().ChangeRoom ("中切", targetRoom, targetDoor);
    }

    //开始房间
    public void StartRoom () {
        playerController.SetReactable (true);
    }

    //房间的切换
    public void SwitchRoom (GameObject targetRoom, Transform targetDoor) {
        //切换房间
        if (currentRoom != null)
            currentRoom.SetActive (false);

        currentRoom = targetRoom;
        //显示房间
        targetRoom.SetActive (true);
        //人物位置设置
        bool isAcitve = targetDoor.gameObject.activeInHierarchy;
        if (!isAcitve) targetDoor.gameObject.SetActive (true);
        Transform playerPos = targetDoor.Find ("PlayerPos");
        if (!isAcitve) targetDoor.gameObject.SetActive (false);
        Debug.Log ("targetDoor:" + targetDoor);
        if (playerPos != null) {
            Debug.Log ("切换房间：" + playerPos.position);
            player.transform.position = playerPos.position;
            if (playerPos.position.x - targetDoor.position.x > 0.05f)
                playerController.Flip (1);
            else if (playerPos.position.x - targetDoor.position.x < -0.05f)
                playerController.Flip (-1);
        }
        //否则为初始房间
        else {
            player.transform.position = targetDoor.position;
        }
        //房间初始化
        RoomInit ();

    }
    private void RoomInit () {
        //Debug.Log("房间初始化");
        Transform roomDetail = currentRoom.transform.Find ("RoomDetail");
        if (roomDetail == null) Debug.LogError (currentRoom.name + " 房间没有RoomDetail");
        GridManager.Instance.LeftDownTF = roomDetail.Find ("GridLimit").Find ("LeftDownPos");
        GridManager.Instance.RightUpTF = roomDetail.Find ("GridLimit").Find ("RightUpPos");
        GridManager.Instance.Init ();

        //查找场景中是否存在敌人
        FSMBase[] enemy = currentRoom.transform.GetComponentsInChildren<NormalEnemyFSM> ();
        if (enemy.Length > 0) {
            //如果有敌人，关掉人物的交互
            //playerController.SetEAble(false);

            //切换成战斗音乐
            BGMManager.Instance.ChangeBGM (BGMType.Attack);
        }
        //否则没有敌人
        else {
            BGMManager.Instance.ChangeBGM (BGMType.Search);
        }
        //BulletPool.bulletPoolInstance.DestroyBulletsInPool();
        //对象池清空
        GameObjectPool.Instance.ClearKey ("PurpleBullet");
        GameObjectPool.Instance.ClearKey ("RedBullet");
        GameObjectPool.Instance.ClearKey ("BossRedBullet");
        GameObjectPool.Instance.ClearKey ("BossPurpleBullet");
    }
    public void CheckEnemy () {
        //确认场景中是否有敌人
        FSMBase[] enemy = currentRoom.transform.GetComponentsInChildren<FSMBase> ();
        if (enemy.Length == 0) {
            //playerController.SetEAble(true);
            BGMManager.Instance.ChangeBGM (BGMType.Search);
        }
    }
    public void SaveData (GameObject Room, Transform playerPos) {
        //保存房间和人物位置
        saveData.roomName = Room.name;
        saveData.lastDoor = playerPos;

        //保存物品列表
        saveData.itemList = new Dictionary<string, ItemInfo> (BagManager.Instance.itemList);

        //保存敌人数据
        saveData.enemys.Clear ();
        foreach (var enemy in Room.transform.GetComponentsInChildren<FSMBase> (true)) {
            FSMBase fsm = enemy.GetComponent<FSMBase> ();
            EnemyData enemyData = new EnemyData (fsm.AttackStyle, fsm.HP, fsm.transform.localPosition);
            saveData.enemys.Add (enemyData);
        }
    }
    //更新受击特效
    public void UpdateHurtedEffect (int damage) {
        if (bloodRenderer.color.a <= 0) bloodIndex = 0;
        bloodRenderer.sprite = bloodPicture[bloodIndex];
        Vector4 x = bloodRenderer.color;
        x.w = 1;
        bloodRenderer.color = x;
        if (bloodIndex + damage >= bloodPicture.Length) { //如果人物死亡

            //停止播放音乐
            BGMManager.Instance.ChangeBGM (BGMType.Stop);

            StartCoroutine (RestartGameDelay ());
            Time.timeScale = 0;
            //            PlayerDead();
        }
        bloodIndex = (bloodIndex + damage) % bloodPicture.Length;

        //设置色差效果
        chromaticAberration.intensity.value = (float) bloodIndex / bloodPicture.Length;
    }
    IEnumerator RestartGameDelay () {
        yield return new WaitForSecondsRealtime (1f);
        Time.timeScale = 1;
        PlayerDead ();
    }
    public void PlayerDead () {
        Debug.Log ("角色死亡");

        BagManager.Instance.itemList = saveData.itemList;
        playerController.PlayerInit ();
        Transform parent = saveData.lastDoor.parent;
        while (parent.parent != null && !parent.CompareTag ("Room")) {
            parent = parent.parent;
        }
        Debug.Log ("parent:" + parent.name + "roomlist contain:" + roomList.Find (s => s.name == parent.name));
        ChangeRoom (roomList.Find (s => s.name == parent.name), saveData.lastDoor, null);

        //初始化敌人

        if (currentRoom.name == "Factory") {
            currentRoom.transform.Find ("交互物品").Find ("起点").GetComponent<PassiveTrigger> ().isActive = true;
            BossFSM bossFSM = currentRoom.transform.Find ("Boss").GetComponent<BossFSM> ();
            bossFSM.BossInit ();
        }
    }
}