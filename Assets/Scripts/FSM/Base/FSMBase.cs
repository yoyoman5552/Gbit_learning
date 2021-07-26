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
    public int HP;
    [Tooltip ("默认状态编号")]
    public FSMStateID DefaultStateID;
    [Tooltip ("巡逻半径")]
    public float patrolRadius = 3f;
    //只要一个圆形半径就好了 
    [Tooltip ("发现玩家的圆形最短半径")]
    public float minRadius;
    //TODO:应该要封装技能：技能数据放一起
    [Tooltip ("攻击距离半径")]
    public float attackRadius;

    //冲刺技能CD变量  通过修改attackInterval控制
    private float SprintCD;
    [Tooltip ("冲刺技能持续时间")]
    public float initSprintTimer;

    private float SprintTimer;

    [Tooltip ("冲刺速度")]
    public float SprintSpeed;

    [Tooltip ("冲刺的加载时间，预保存玩家位置")]
    public float initLoadStimer;

    /*     [Tooltip("发现玩家的扇形半径")]
        public float sectorRadius;
        [Tooltip("发现玩家的扇形角度")]
        public float sectorAngle;
     */
    [Tooltip ("待机时长")]
    public float idleTime;
    [Tooltip ("受伤混沌时长")]
    public float hurtedTime;
    [Tooltip ("受伤速度")]
    public float HurtedSpeed = 1f;
    [Tooltip ("攻击间隔")]
    public float attackInterval = 2f;
    [Tooltip ("攻击力")]
    public int damage;
    [Tooltip ("击退速度")]
    public float GetHurtSpeed = 0.2f;
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

    //近战攻击方式:true为冲刺，false为冲刺后的近战
    [HideInInspector]
    public bool meleeAttackStyle;

    [Tooltip ("是否远程攻击")]
    public bool AttackStyle;

    //材质
    [HideInInspector]
    //冲刺技能CD状态
    public bool SprintUsed = false;

    //是否正在冲刺
    public bool Sprinting = false;

    //冲刺方向
    public Vector3 SprintDir;

    [HideInInspector]
    public Material material;

    //状态列表
    protected List<FSMState> statesList;

    //当前状态
    protected FSMState currentState;
    //默认状态
    protected FSMState defaultState;
    //    public FSMStateID currentID;
    [HideInInspector]
    public Rigidbody2D rb;
    protected SpriteRenderer sprite;
    //子物体获取（贴图为主）
    private Transform childTF;
    //追击目标
    [HideInInspector]
    public Transform targetTF;
    [HideInInspector]
    public Vector3 OriginPos;
    [HideInInspector]
    public bool walkAble;
    //  [HideInInspector]
    public float m_speed;
    //是否受伤
    [HideInInspector]
    public bool isHurted;

    public FSMStateID test_stateID;
    [HideInInspector]
    public Vector3 hurtedVelocity;

    [HideInInspector]

    //动画
    public Animator animator;

    public AudioSource enemyAudio;
    public AudioClip attackClip;
    public AudioClip GetHurtClip;

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
        material = sprite.material;
        walkAble = true;
        isHurted = false;
        targetTF = null;
        SprintTimer = initSprintTimer;
        SprintCD = attackInterval;

        animator = GetComponentInChildren<Animator> ();

        /*  //动画机
        
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
        test_stateID = currentState.stateID;
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
        checkSprintCD ();

    }
    private void checkSprintCD () {
        if (SprintUsed) {
            SprintCD -= Time.deltaTime;
            if (SprintCD <= 0) {
                SprintUsed = false;
                SprintCD = attackInterval;
            }
        }
    }
    public virtual void FixedUpdate () {
        if (walkAble) {
            //如果是冲刺状态
            if (Sprinting) {
                //冲刺
                rb.velocity = SprintDir * SprintSpeed * Time.fixedDeltaTime * ConstantList.speedPer;
            } else {
                //移动
                rb.velocity = moveVelocity * m_speed * Time.fixedDeltaTime * ConstantList.speedPer;
            }

        }

        //冲刺时出了攻击范围，小怪会出现异常，所以直接放到fsmbase
        //        SprintAttack();
    }
    // private void SprintAttack()
    // {
    //     if (Sprinting)
    //     {
    //         rb.velocity = SprintDir * SprintSpeed * Time.fixedDeltaTime * ConstantList.speedPer;

    //         /*             SprintTimer -= Time.deltaTime;
    //                     if (SprintTimer > 0)
    //                     {
    //                         walkAble = false;
    //                         animator.SetBool("Sprint", true);
    //                         //rb.velocity = (targetTF.transform.position - transform.position).normalized * 0.5f;
    //                         rb.velocity = SprintDir * SprintSpeed * Time.fixedDeltaTime * ConstantList.speedPer;
    //                     }
    //                     else
    //                     {
    //                         walkAble = true;
    //                         SprintUsed = true;
    //                         Sprinting = false;
    //                         SprintTimer = initSprintTimer;
    //                         animator.SetBool("Sprint", false);
    //                     }
    //                 }
    //          */
    //     }
    // }
    public void DeadDelay () {
        
        this.gameObject.SetActive (false);
        GameManager.Instance.CheckEnemy ();
        Destroy (this.gameObject);
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
        if (isHurted) {
            return;
        }
        if (moveVelocity.x > 0.05f) {
            sprite.flipX = true;
        } else if (moveVelocity.x < -0.05f) {
            sprite.flipX = false;
        }
    }
    public void textureClip (float dir) {
        if (isHurted) {
            return;
        }
        if (dir > 0.05f) {
            sprite.flipX = true;
        } else if (dir < -0.05f) {
            sprite.flipX = false;
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

    public virtual void TakenDamage (int damage, Vector3 dir) {
        if (!isHurted) {
            HP = Mathf.Max (HP - damage, 0);
            isHurted = true;
            material.SetFloat ("_FlashAmount", 1);
            hurtedVelocity = dir;
            if (dir.x > 0)
                sprite.flipX = false;
            else
                sprite.flipX = true;
            rb.velocity = dir * GetHurtSpeed;
            if (enemyAudio != null) {
                enemyAudio.PlayOneShot(GetHurtClip);
            }
            StartCoroutine (hurtedContinus (hurtedTime));

        }
    }
    public IEnumerator hurtedContinus (float timer) {
        yield return new WaitForSeconds (timer);
        isHurted = false;
        material.SetFloat ("_FlashAmount", 0);
    }
}