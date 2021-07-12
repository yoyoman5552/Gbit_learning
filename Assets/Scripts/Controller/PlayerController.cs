using System;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("共有变量")]
    [Tooltip("最大血量")]
    public int MaxHP = 5;
    [Tooltip("默认移动速度")]
    public float moveSpeed;
    [Tooltip("按下E的时候的速度")]
    public float PressESpeed;
    [Tooltip("跳跃高度")]
    public float jumpHeight;
    [Tooltip("血量分段")]
    public float hpLevels = 3;
    //当前移动速度
    public float m_speed;
    //当前血量
    public int m_hp;
    //跳跃时间
    public float smoothTime = 0.5f;

    [Header("私有变量")]

    //跳跃速度
    private float jumpSpeed;
    //移动方向
    private Vector2 moveDir;
    private Transform playerChildTF;
    private Transform playerFakeChild;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isJump;
    //是否能够移动  //控制跳跃不能移动
    private bool walkAble;
    //控制人物任何状况不能移动
    //    private bool canNotMove;
    //是否能够监听键盘输入
    private bool reactAble;
    //跳跃的虚拟z轴速度
    private float velocity_Y;
    //人物检测圈
    private PlayerCircleDetect playerDetect;
    //强制移动目标位置
    private Vector3 targetPos;
    //跳跃的相对缓冲减速（默认为0即可）
    private Vector2 velocity;
    //自身的collider
    private new BoxCollider2D collider;
    //音乐播放器
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public GameObject PressETarget;
    //动画器
    private Animator playerAnimator;
    //受击时间
    private float hurtedTimer;
    //人物材质
    private Material material;
    //画面血渍
    //private SpriteRenderer GameManager.Instance.bloodEffect;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerChildTF = this.transform.Find("PlayerChild");
        playerFakeChild = this.transform.Find("PlayerChild");
        //playerFakeChild=this.transform.Find("PlayerFakeChild");
        sprite = playerChildTF.GetComponent<SpriteRenderer>();
        playerDetect = this.GetComponentInChildren<PlayerCircleDetect>();
        collider = this.GetComponent<BoxCollider2D>();
        source = this.GetComponent<AudioSource>();
        //新加：获取人物动画
        playerAnimator = this.GetComponentInChildren<Animator>();
        //受伤屏幕血液
        //GameManager.Instance.bloodEffect = this.transform.Find("getHurt").GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //MouseManager.Instance.OnMouseClicked += MoveToTarget;
        m_speed = moveSpeed;
        m_hp = MaxHP;
        isJump = false;
        //        canNotMove = false;
        reactAble = walkAble = true;
        hurtedTimer = 0;
        targetPos = Vector3.back;
        material = sprite.material;
    }
    private void Update()
    {
        if (Time.timeScale == 0) return;
        //获取键盘输入
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical") * ConstantList.moveYPer;
        //如果跳跃 且当前不是跳跃状态
        /* if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            ReadyToJump();
        } */
        if (!reactAble) return;
        //交互键判断
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject target = playerDetect.GetFirst();
            //检测范围内有目标，而且是激活的，而且当前没有正在交互的目标

            if (target != null&&target.GetComponent<ItemTrigger>().isActive&&PressETarget==null)
            {
                CheckETarget(target);
            }
        }
        //检测跳跃状态
        //CheckJump();
        CheckMoveToTarget();
        //受伤检测
        HurtedCheck();
        //测试
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakenDamage(1);
        }
    }
    private void HurtedCheck()
    {
        if (hurtedTimer > 0)
        {
            hurtedTimer -= Time.deltaTime;
            material.SetFloat("_FlashAmount", 1);
        }
        else
        {
            material.SetFloat("_FlashAmount", 0);
        }
    }
    public void SetReactable(bool flag)
    {
        reactAble = flag;
    }
    //    private void MoveToTarget()
    private void CheckJump()
    {
        //判断子物体是在下落状态(velocity_Y<0)而且子物体离父物体距离小于等于0.05
        if (playerFakeChild.position.y <= this.transform.position.y + 0.05f && velocity_Y < 0)
        {
            //满足了就说明跳跃完成
            velocity_Y = 0;
            playerFakeChild.position = this.transform.position;
            isJump = false;
            targetPos = Vector3.back;
            walkAble = true;
            collider.isTrigger = false;
        }
    }
    private void CheckETarget(GameObject target)
    {
        switch (target.tag)
        {
            case "Interactive":
                Debug.Log("target:" + target.name + ",tag:" + target.tag);
                PressETarget = target;
                playerAnimator.SetBool("IsPressE", true);
                break;
            case "QuickInteractive":
                if (target.GetComponent<ConditionTrigger>() != null)
                {
                    target.GetComponent<ConditionTrigger>().StartTrigger();
                }
                //否则就是直接执行trigger 
                else
                {
                    target.GetComponent<ActiveTrigger>().StartTrigger();
                }
                PressETarget = null;
                break;
        }
        if (target.GetComponent<ItemTrigger>().isActive && PressETarget == null)
        {
            Debug.Log("press E");
            PressETarget = target;
            playerAnimator.SetBool("IsPressE", true);
        }

    }
    private void CheckMoveToTarget()
    {
        if (isJump && Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            //满足了就说明移动完成
            velocity_Y = 0;
            //playerFakeChild.position = this.transform.position;
            isJump = false;
            //targetPos = Vector3.back;
            walkAble = true;
            collider.isTrigger = false;
            PressETarget=null;
        }
    }
    private void FixedUpdate()
    {
        //移动
        Move();
        //跳跃
        //Jump();
        //角色图片翻转
        PlayerClip();
    }
    public void TakenDamage(int damage)
    {
        SpriteRenderer renderer = GameManager.Instance.bloodEffect.GetComponent<SpriteRenderer>();
        Vector4 setColor = renderer.color;
        m_hp = Mathf.Max(0, m_hp - damage);
        hurtedTimer = ConstantList.HurtedTime;
        //        setColor.w = (((float)MaxHP - m_hp) / MaxHP) * 255;
        //FIXME:目前是一个血量一个状态
        setColor.w = 1 - (float)m_hp / MaxHP;
        /*  for (int i = 1; i <= hpLevels; i++)
         {
             if (m_hp <= MaxHP / hpLevels * i)
             {
                 Debug.Log("level:" + MaxHP / hpLevels * i + "alpha:" + (hpLevels - i + 1) / hpLevels);
                 setColor.w = (hpLevels - i + 1) / hpLevels;
                 break;
             }
         } */
        renderer.color = setColor;
        /*         if (m_hp <= MaxHP / hpLevels)
                {
                    setColor.w = 255;
                    GameManager.Instance.bloodEffect.color = setColor;
                }
                else if (m_hp <= MaxHP / (float)2)
                {
                    setColor.w = 2 * 255 / 3;
                    GameManager.Instance.bloodEffect.color = setColor;
                }
                else
                {
                    setColor.w = 255 / 3;
                    GameManager.Instance.bloodEffect.color = setColor;
                }
         */
    }
    /// 角色移动
    private void Move()
    {
        //        if (walkAble && reactAble&&!canNotMove)
        if (walkAble && reactAble)
        {
            //speedPer是一个缩进值：让m_speed不用那么大
            if (playerAnimator.GetBool("IsPressE"))
                rb.velocity = moveDir.normalized * PressESpeed * Time.fixedDeltaTime * ConstantList.speedPer;
            else
                rb.velocity = moveDir.normalized * m_speed * Time.fixedDeltaTime * ConstantList.speedPer;
            //移动动画
            if (moveDir != Vector2.zero)
                playerAnimator.SetBool("IsWalking", true);
            else
                playerAnimator.SetBool("IsWalking", false);
        }
        else
        {
            rb.velocity = Vector3.zero;
            playerAnimator.SetBool("IsWalking", false);
        }
        //如果正在跳跃
        if (isJump)
        {
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, targetPos.x, ref velocity.x, smoothTime), Mathf.SmoothDamp(transform.position.y, targetPos.y, ref velocity.y, smoothTime), transform.position.z);
        }
    }
    /// 角色跳跃
    /*     private void Jump()
        {
            //如果无法移动，而且正在跳跃，说明是交互E键
            if (!walkAble && isJump && targetPos != Vector3.back)
            {
                transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, targetPos.x, ref velocity.x, smoothTime), Mathf.SmoothDamp(transform.position.y, targetPos.y, ref velocity.y, smoothTime), transform.position.z);
            }
            if (!(velocity_Y < 0 && Vector3.Distance(playerFakeChild.position, transform.position) < 0.05f))
            {
                //重力模拟
                velocity_Y += ConstantList.g * Time.fixedDeltaTime;
                playerFakeChild.Translate(new Vector3(0, velocity_Y) * Time.fixedDeltaTime);
            }
        } */
    /// 角色图片翻转
    public void PlayerClip()
    {
        if (PressETarget != null)
        {

            if (PressETarget.transform.position.x > this.transform.position.x)
                sprite.flipX = false;
            else
                sprite.flipX = true;
        }
        else
        {
            if (moveDir.x >= 0.05f)
            {
                //sprite.transform.localScale = new Vector3(-1, 1, 1);
                sprite.flipX = false;
            }
            else if (moveDir.x <= -0.05f)
            {
                //sprite.transform.localScale = new Vector3(1, 1, 1);
                sprite.flipX = true;
            }
        }
    }
    /// 角色跳跃
    /*     private void ReadyToJump()
        {
            velocity_Y = Mathf.Sqrt(jumpHeight * -2f * ConstantList.g);
        } */
    /// 操控玩家跳跃
    public void PlayerJump(Vector3 target)
    {
        if (!isJump)
        {
            isJump = true;
            //禁止移动
            walkAble = false;
            //暂且关掉碰撞
            collider.isTrigger = true;
            //玩家朝向
            moveDir = target - transform.position;
            //ReadyToJump();
            MoveToTarget(target);

            //跳跃动画
            playerAnimator.SetTrigger("Jump");
        }
    }
    public void MoveToTarget(Vector3 target)
    {

        targetPos = target;
    }
    public void dontWalkAPI()
    {
        //walkAble = false;
        //print("cannotmove");
        //playerAnimator.SetFloat("Vertical", 0);
        //playerAnimator.SetFloat("Horizontal", 0);
        playerAnimator.SetFloat("Speed", 0);
        reactAble = false;
        //        canNotMove = true;
        Invoke("resetWalkAble", 1.0f);
    }
    private void resetWalkAble()
    {
        reactAble = true;
        //        canNotMove = false;
    }
    /*     private void MoveToTarget (Vector3 target) {
            //TODO:获得路径，走向目标 应该不需要
            List<PathNode> pathList = GridManager.Instance.FindPath (this.transform.position, target);
            if (pathList.Count <= 1) return;
        }
     */

}