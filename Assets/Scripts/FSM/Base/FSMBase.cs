using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//状态机基类
public abstract class FSMBase : MonoBehaviour {
    [Header ("公开变量")]
    [Tooltip ("移动速度")]
    public float walkSpeed;
    [Tooltip ("追击速度")]
    public float chaseSpeed;
    [Tooltip ("血量")]
    public float HP;
    [Tooltip ("默认状态编号")]
    public FSMStateID DefaultStateID;
    //只要一个圆形半径就好了 
    [Tooltip ("发现玩家的圆形最短半径")]
    public float minRadius;
    //TODO:应该要封装技能：技能数据放一起
    [Tooltip ("攻击距离半径")]
    public float attackRadius;
    /*     [Tooltip("发现玩家的扇形半径")]
        public float sectorRadius;
        [Tooltip("发现玩家的扇形角度")]
        public float sectorAngle;
     */
    [Tooltip ("待机时长")]
    public float idleTime;
    [Tooltip ("受伤混沌时长")]
    public float hurtedTime;

    [Header ("私有变量")]
    //巡逻目的地
    [HideInInspector]
    // public bool isDonePatrol;
    public Vector3 patrolPos;
    //是否完成追击
    [HideInInspector]
    public bool isDoneChase;
    //移动方向
    [HideInInspector]
    public Vector3 moveVelocity;
    [HideInInspector]
    public IAttack attackWay;

    //TODO:是否需要给敌人设置一个巡逻范围：只会在巡逻范围内随机选择点来巡逻
    /*     [Tooltip("巡逻范围,以左下点和右上点为主")]
        public Transform[] patrolTFs; */

    //状态列表
    protected List<FSMState> statesList;

    //当前状态
    protected FSMState currentState {
        get {
            return test_state;
        }
        set {
            if (test_state != null) {

            }
            //                    Debug.Log ("state改变：" + test_state.stateID.ToString () + "->" + value.stateID.ToString ());
            test_state = value;
        }
    }
    private FSMState test_state;
    //默认状态
    protected FSMState defaultState;
    //    public FSMStateID currentID;
    [HideInInspector]
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    //子物体获取（贴图为主）
    private Transform childTF;
    //追击目标
    [HideInInspector]
    public Transform targetTF;
    [HideInInspector]
    public bool walkAble;
    [HideInInspector]
    public float m_speed;
    //是否受伤
    [HideInInspector]
    public bool isHurted;
    private void Awake () {
        Init ();
    }
    //初始化怪物数据
    private void Init () {
        //初始化Component的东西
        InitComponent ();
        //配置状态机
        ConfigFSM ();
        //查找默认状态：默认状态初始化
        InitDefaultState ();
    }

    /*     private void Reset()
        {
            statesList.Clear();
        }
     */
    public virtual void InitComponent () {
        rb = GetComponent<Rigidbody2D> ();
        childTF = this.transform.Find ("CharacterChild");
        sprite = childTF.GetComponent<SpriteRenderer> ();
        walkAble = true;
        isHurted = false;
        targetTF = null;
        /*  //动画机
        animator = GetComponentInChildren<Animator> ();
        //角色数值
        characterStatus = GetComponent<CharacterStatus> ();
        //初始化位置
        startPosition = transform.position;
        //初始化技能管理器
        skillSystem = GetComponent<CharacterSkillSystem> (); */
    }
    public void InitDefaultState () {
        defaultState = statesList.Find (s => s.stateID == DefaultStateID);
        currentState = defaultState;
        currentState.EnterState (this);
    }
    //配置状态机
    //根据人物状态需要设置状态机
    public abstract void ConfigFSM ();
    //--创建状态对象
    //--设置状态(AddMap)

    //每帧处理的逻辑
    public virtual void Update () {
        //检测是否被攻击了，被攻击就放大搜索圈
        //HurtedSearch ();
        //TODO:侦测周围是否有敌人
        DetectTarget ();
        //每帧判断条件，如果有条件满足了就切换状态
        //判断当前状态条件
        currentState.DetectTriggers (this);
        //执行当前逻辑
        currentState.ActionState (this);
        //贴图翻转
        textureClip ();
    }
    public virtual void FixedUpdate () {
        if (walkAble) {
            //移动
            rb.velocity = moveVelocity * m_speed * Time.fixedDeltaTime * ConstantList.speedPer;

            /* if (Vector3.Distance (transform.position, movePos) > 0.05f) {
                Vector3 dir = (movePos - this.transform.position).normalized;
                rb.velocity = dir * m_speed;
            } */
        }
    }

    //切换状态
    public void ChangeActiveState (FSMStateID stateID) {
        //更新当前状态
        //退出当前状态
        //               Debug.Log ("change state:" + currentState.stateID.ToString () + " to " + stateID.ToString ());
        currentState.ExitState (this);
        //切换状态
        //如果需要切换的状态编号是 Default 就直接返回默认状态,否则返回查找的状态
        currentState = stateID == FSMStateID.Default ? defaultState : statesList.Find (s => s.stateID == stateID);
        //进入下一个状态
        currentState.EnterState (this);
    }
    /// <summary>
    /// 检测目标
    /// </summary>
    private void DetectTarget () {
        var targetArray = Physics2D.OverlapCircleAll (this.transform.position, minRadius);
        foreach (var target in targetArray) {
            if (target.CompareTag ("Player")) {
                targetTF = target.transform;
                break;
            }
        }
    }
    /// <summary>
    /// 贴图翻转
    /// </summary>
    private void textureClip () {
        if (rb.velocity.x > 0.05f) {
            sprite.flipX = false;
        } else if (rb.velocity.x < -0.05f) {
            sprite.flipX = true;
        }
    }
    /// <summary>
    /// 移动位置
    /// </summary>
    /// <param name="dirPos"></param>
    public void MovePosition (Vector3 dirPos) {
        moveVelocity = (dirPos - this.transform.position).normalized;
        //Debug.Log("moveVelocity:"+moveVelocity);
    }
    public void StopPosition () {
        moveVelocity = Vector3.zero;
    }
}