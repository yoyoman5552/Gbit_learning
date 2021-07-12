using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//状态机基类
public abstract class PlayerFSMBase : MonoBehaviour {
    [Header ("公开变量")]
    [Tooltip ("移动速度")]
    public float moveSpeed;
    [Tooltip ("血量")]
    public float HP;
    [Tooltip ("跳跃高度")]
    public float jumpHeight;
    [Tooltip ("默认状态编号")]
    public PlayerFSMStateID[] DefaultStateID;

    [Header ("私有变量")]
    //移动方向
    [HideInInspector]
    public Vector3 moveVelocity;

    //状态列表
    protected List<PlayerFSMState> statesList;
    //状态关系图
    protected StateRelationShip[, ] statesRelationMap;
    //当前状态
    protected List<PlayerFSMState> currentStates;
    //默认状态
    protected PlayerFSMState[] defaultState;
    //    public PlayerPlayerFSMStateID currentID;
    [HideInInspector]
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    //子物体获取（贴图为主）
    [HideInInspector]
    public Transform childTF;
    //人物检测圈
    [HideInInspector]
    public PlayerCircleDetect playerDetect;
    [HideInInspector]
    public bool walkAble;
    //是否能够监听键盘输入
    [HideInInspector]
    public bool reactAble;
    //是否在跳跃
    [HideInInspector]
    public bool isJump;
    [HideInInspector]
    public float m_speed;
    //是否受伤
    [HideInInspector]
    public bool isHurted;
    //自身的collider
    [HideInInspector]
    public new BoxCollider2D collider;
    //强制移动目标位置
    [HideInInspector]
    public Vector3 targetPos;
    //跳跃的相对缓冲减速（默认为0即可）
    [HideInInspector]
    public Vector2 velocity;
    //跳跃时间
    [HideInInspector]
    public float smoothTime = 0.2f;
    //跳跃的虚拟z轴速度
    [HideInInspector]
    public float velocity_Y;
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
        playerDetect = this.GetComponentInChildren<PlayerCircleDetect> ();
        collider = this.GetComponent<BoxCollider2D> ();
        reactAble = walkAble = true;
        isJump = isHurted = false;
    }
    public void InitDefaultState () {
        defaultState = new PlayerFSMState[DefaultStateID.Length];
        for (int i = 0; i < DefaultStateID.Length; i++) {
            defaultState[i] = statesList.Find (s => s.stateID == DefaultStateID[i]);
            currentStates.Add (defaultState[i]);
            //        currentStates.EnterState (this);            
        }
    }
    //配置状态机
    //根据人物状态需要设置状态机
    public abstract void ConfigFSM ();
    //--创建状态对象
    //--设置状态(AddMap)

    //每帧处理的逻辑
    public virtual void Update () {
        //每帧判断条件，如果有条件满足了就切换状态
        foreach (var state in currentStates) {
            //判断当前状态条件
            state.DetectTriggers (this);
            //执行当前逻辑
            state.ActionState (this);
        }
        //贴图翻转
        textureClip ();
    }
    public virtual void FixedUpdate () {
        foreach (var state in currentStates) {
            state.FixedActionState (this);
        }
    }

    //切换状态
    public void ChangeActiveState (PlayerFSMStateID stateID) {
        //更新当前状态
        //退出当前状态
        //               Debug.Log ("change state:" + currentState.stateID.ToString () + " to " + stateID.ToString ());
        List<PlayerFSMState> stateCopy = new List<PlayerFSMState> ();
        foreach (var state in currentStates) {
            if (statesRelationMap[(int) state.stateID, (int) stateID] == StateRelationShip.Forbidden) {
                return;
            } else if (statesRelationMap[(int) state.stateID, (int) stateID] == StateRelationShip.Immediately) {
                stateCopy.Add (state);
            }
        }
        foreach (var state in stateCopy) {
            state.ExitState (this);
            currentStates.Remove (state);
        }
        currentStates.Add (statesList.Find (s => s.stateID == stateID));
        currentStates[currentStates.Count - 1].EnterState (this);

        /*         currentStates.ExitState (this);
                //切换状态
                //如果需要切换的状态编号是 Default 就直接返回默认状态,否则返回查找的状态
                currentStates = stateID == PlayerFSMStateID.Default ? defaultState : statesList.Find (s => s.stateID == stateID);
                //进入下一个状态
                currentStates.EnterState (this);
         */
    }
    public bool IsStateChangeable (PlayerFSMStateID stateID) {
        foreach (var state in currentStates) {
            if (statesRelationMap[(int) state.stateID, (int) stateID] == StateRelationShip.Forbidden) return false;
        }
        return true;
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