using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class PlayerChildController : MonoBehaviour
{
    //�����
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;
    [Tooltip("伤害间隔")]
    public float attackInterval = 0.4f;
    private BreakLevel attackType;
    [HideInInspector]
    public bool isAttack;
    private int heavyAttack = 1;
    //private string attackType;
    private int comboStep = 0;
    private float timer;
    private Animator playerAnimator;
    private SpriteRenderer mySprit;
    private PlayerController controller;
    //    private Vector3 childCorrectScale = new Vector3(1, 1, 1);
    private void Awake()
    {
        mySprit = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

    }
    private void FixedUpdate()
    {

    }
    private void Attack()
    {
        //        childCorrectScale.x = mySprit.flipX ? 1 : -1;
        //        this.transform.GetChild(0).localScale = childCorrectScale;
        if (Input.GetKeyDown(KeyCode.J) && !isAttack)
        {


            //缺少将hitonece = false玩家第二段攻击失效

            isAttack = true;
            attackType = BreakLevel.easy;
            print(comboStep + 1);
            playerAnimator.SetTrigger("LightAttack");
            playerAnimator.SetInteger("ComboStep", comboStep + 1);
            comboStep = (comboStep + 1) % 4;
        }
        /*         if (Input.GetKeyDown(KeyCode.K) && !isAttack)
                {

                    hitOnce = false;

                    isAttack = true;
                    attackType = BreakLevel.hard;
                    playerAnimator.SetTrigger("HeavyAttack");
                    playerAnimator.SetInteger("ComboStep", heavyAttack);
                } */
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer > -10)
        {
            timer = -999;
            comboStep = 0;
        }
    }
    public void AttackOver()
    {
        timer = attackInterval;
        isAttack = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Breakable") || other.CompareTag("Enemy")))
        {
            if (attackType == BreakLevel.easy)
            {
                AttackSense.Instance.HitPause(lightPause);
                AttackSense.Instance.CameraShake(shakeTime, lightStrength);


            }
            else if (attackType == BreakLevel.hard)
            {
                AttackSense.Instance.HitPause(heavyPause);
                AttackSense.Instance.CameraShake(shakeTime, heavyStrength);
            }

            if (other.CompareTag("Enemy"))
            {
                Debug.Log("触碰到Enemy");
                other.GetComponent<FSMBase>().TakenDamage((int)attackType + 1);
                /*                 if (mySprit.transform.localScale.x > 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.left);
                                else if (mySprit.transform.localScale.x <= 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                 */
            }
            if (other.CompareTag("Breakable"))
            {
                Debug.Log("触碰到Interactive");
                if (other.GetComponent<Breakable_Trigger>() != null)
                {
                    BreakLevel level = other.GetComponent<Breakable_Trigger>().level;
                    //如果攻击比他强
                    Debug.Log("attackType:" + (int)attackType + "," + (attackType >= level));
                    if ((int)attackType >= (int)level)
                        other.GetComponent<Breakable_Trigger>().Action();
                }
            }
        }

    }
    public void SetBoolDown(string animationFlag)
    {
        playerAnimator.SetBool(animationFlag, false);
    }
    public void SetBoolUp(string animationFlag)
    {
        playerAnimator.SetBool(animationFlag, false);
    }
    public void PressEAction()
    {
        Debug.Log("Press E Action");
        if (controller.PressETarget.GetComponent<ConditionTrigger>() != null)
        {
            controller.PressETarget.GetComponent<ConditionTrigger>().StartTrigger();
        }
        //否则就是直接执行trigger 
        else
        {
            controller.PressETarget.GetComponent<ActiveTrigger>().StartTrigger();
        }
        controller.PressETarget = null;
    }
}

