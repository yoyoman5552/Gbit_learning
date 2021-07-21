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
    [Tooltip("蓝/黄毒气")]
    public GameObject Fog_Blue;
    public GameObject Fog_Yellow;
    [Tooltip("蓝/黄毒气选择,true为黄，false为蓝")]
    public bool YellowOrBlue;

    //队列记录位置
    //Queue<Vector3> pathPositon = new Queue<Vector3>();
    //private float timeInterval = 0.2f;
    private venomFather fatherState;
    private GameObject Fog_Select;
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
    //private Transform  GameManager.Instance.player.transform;
    //地刺x y
    private float stabX;
    private float stabY;
    //玩家收到攻击
    private bool playerGetHurt;
    private void Awake()
    {
        //col = GetComponent<Collider2D>();
        fatherState = this.GetComponentInParent<venomFather>();
        collider = this.GetComponent<Collider2D>();
        stabX = collider.bounds.size.x;
        stabY = collider.bounds.size.y;
    }
    // Start is called before the first frame update
    void Start()
    {
        timeStabAppear = initTimeStabAppear;
        timeStabDisAppear = initTimeStabDisappear;
        if (YellowOrBlue)
            Fog_Select = Fog_Blue;
        else
            Fog_Select = Fog_Yellow;
        Fog_Yellow.SetActive(false);
        Fog_Blue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeStabAppear -= Time.deltaTime;
        if (timeStabAppear < 0)
        {
            target.SetActive(false);
            Fog_Select.SetActive(false);
            collider.enabled = false;
            getHurt = false;
            timeStabDisAppear -= Time.deltaTime;
            if (timeStabDisAppear < 0)
            {
                target.SetActive(true);
                Fog_Select.SetActive(true);
                getHurt = true;
                collider.enabled = true;
                timeStabDisAppear = initTimeStabDisappear;
                timeStabAppear = initTimeStabAppear;

            }
        }
        if(this.name!="毒液地刺")
            detectDistance();

    }
    /*
    private void QueueWorking()
    {
        
        pathPositon.Enqueue(GameManager.Instance.player.transform.position);
        if (pathPositon.Count>5)
        {
            pathPositon.Dequeue();
        }
        
    }
    */
    //距离检测
    private void detectDistance()
    {
        
        //right,left,up,down
        if (Mathf.Abs(GameManager.Instance.player.transform.position.x - transform.position.x) <= stabX / 2.0f + 0.5f && Mathf.Abs(GameManager.Instance.player.transform.position.y - transform.position.y) <= stabY / 2.0f + 0.5f)
        {
            if (firstEnter)
            {
                enterPosition = GameManager.Instance.player.transform.position;
                firstEnter = false;
                Debug.Log("InInIn");
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
                //Debug.Log("玩家受伤");
                GameManager.Instance.playerController.TakenDamage(1, Vector3.zero);
                attacked = true;
                collider.enabled = false;
                StartCoroutine(AttackDelay(attackRatio));
                //playerGetHurt = true;

                //TODO:返回进入点的方式：
                //直接返回进入点效果一般
                //所试方法：
                //1.延迟0.5秒返回进入点
                if (this.name == "毒液地刺" )
                { 
                    enterPosition = GameManager.Instance.player.transform.position + 0.4f * (GameManager.Instance.player.transform.position - transform.position).normalized;
                }


                Invoke("resetPlayerPosition", 0.2f);

                //2.直接返回进入点，延迟时间
                // GameManager.Instance.player.transform.position = enterPosition;
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
        GameManager.Instance.player.transform.position = enterPosition;
    }
    private void resetPlayerPosition()
    {
        
        GameManager.Instance.player.transform.position = enterPosition;
        GameManager.Instance.player.transform.gameObject.GetComponent<PlayerController>().dontWalkAPI(1.0f);
    }

}
