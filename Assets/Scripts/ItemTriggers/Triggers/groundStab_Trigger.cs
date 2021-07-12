using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class groundStab_Trigger : ITrigger
{
    [Tooltip("消失间隔")]
    public float initTimeStabDisappear = 1.5f;
    [Tooltip("出现时长")]
    public float initTimeStabAppear = 1.5f;
    [Tooltip("攻击频率")]
    public float attackRatio = 0.3f;
    [Tooltip("地刺物体")]
    public GameObject target;
    private float timeStabDisAppear;
    private float timeStabAppear;
    private bool getHurt = true;
    private bool attacked = false;
    private new Collider2D collider;
    //获取玩家进入方向的变量
    private Vector2 enterPosition;
    //是否是刚接触地刺
    private bool firstEnter;
    //玩家
    private Transform PlayerTransform;
    //地刺x y
    private float stabX;
    private float stabY;
    //玩家收到攻击
    private bool playerGetHurt;
    private void Awake()
    {
        //col = GetComponent<Collider2D>();
        PlayerTransform = GameManager.Instance.player.transform;
        collider = this.GetComponent<Collider2D>();
        stabX = collider.bounds.size.x;
        stabY = collider.bounds.size.y;
    }
    // Start is called before the first frame update
    void Start()
    {
        timeStabAppear = initTimeStabAppear;
        timeStabDisAppear = initTimeStabDisappear;
    }

    // Update is called once per frame
    void Update()
    {
        timeStabAppear -= Time.deltaTime;
        if (timeStabAppear < 0)
        {
            target.SetActive(false);
            collider.enabled = false;
            getHurt = false;
            timeStabDisAppear -= Time.deltaTime;
            if (timeStabDisAppear < 0)
            {
                target.SetActive(true);
                getHurt = true;
                collider.enabled = true;
                timeStabDisAppear = initTimeStabDisappear;
                timeStabAppear = initTimeStabAppear;
                
            }
        }

        detectDistance();
    }


    //距离检测
    private void detectDistance()
    {
        //right,left,up,down
        if (Mathf.Abs(PlayerTransform.position.x - transform.position.x) <= stabX/ 2.0f+0.5f && Mathf.Abs(PlayerTransform.position.y - transform.position.y) <= stabY / 2.0f+0.5f)
        {
            if (firstEnter)
            {
                enterPosition = PlayerTransform.position;
                firstEnter = false;
                //Debug.Log("InInIn");
            }

        }
        else
        {
            firstEnter = true;
            //Debug.Log("Out");
        }
    }
    public override void Action()
    {
        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && getHurt)
        {
            if (!attacked)
            {
                //TODO:玩家受伤
                Debug.Log("玩家受伤");
                attacked = true;
                collider.enabled = false;
                StartCoroutine(AttackDelay(attackRatio));
                //playerGetHurt = true;

                //TODO:返回进入点的方式：
                //直接返回进入点效果一般
                //所试方法：
                //1.延迟0.5秒返回进入点
                Invoke("resetPlayerPosition", 0.2f);

                //2.直接返回进入点，延迟时间
                //PlayerTransform.position = enterPosition;
                //TODO:人物受伤？短暂不能移动，体现被移出地刺范围
                /*Time.timeScale = 0.1f;
                 Invoke("resetTimeScale", 0.05f);*/
            }
        }
    }
    IEnumerator AttackDelay(float timer)
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            if (!getHurt)
            {
                break;
            }
        }
        attacked = false;
        //如果是攻击状态
        if (getHurt)
        {
            collider.enabled = true;
        }
    }


    //快速恢复时间
    private void resetTimeScale()
    {
        Time.timeScale = 1;
        PlayerTransform.position = enterPosition;
    }
    private void resetPlayerPosition()
    {
        PlayerTransform.position = enterPosition;
        PlayerTransform.gameObject.GetComponent<PlayerController>().dontWalkAPI();
    }    

}
