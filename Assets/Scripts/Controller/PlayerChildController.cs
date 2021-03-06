using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class PlayerChildController : MonoBehaviour
{
    //�����
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;
    [Tooltip("伤害数值")]
    public int[] attackRatio = new int[] { 1, 5 };
    [Tooltip("伤害间隔")]
    public float attackInterval = 0.4f;
    //伤害类型
    public BreakLevel attackType;
    //武器使用次数限制
    private int armorTimes;

    [HideInInspector]
    public bool isAttack;
    //private int heavyAttack = 1;
    //private string attackType;
    private int comboStep = 0;
    private float timer;
    private Animator playerAnimator;
    private SpriteRenderer mySprit;
    private PlayerController controller;
    //是否可以进行攻击操作
    private bool fightLimit;
    public AudioSource attackAudio;
    public AudioClip attackClip1;
    public AudioClip attackClip2;
    public AudioClip attackClip3;
    public AudioClip attackClip4;
    public AudioClip HeavyAttackClip;
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
        fightLimit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fightLimit)
            Attack();

    }
    private void FixedUpdate()
    {

    }
    private void Attack()
    {
        //        childCorrectScale.x = mySprit.flipX ? 1 : -1;
        //        this.transform.GetChild(0).localScale = childCorrectScale;
        if (Input.GetKeyDown(KeyCode.J) && !isAttack && Time.timeScale != 0)
        {

            //缺少将hitonece = false玩家第二段攻击失效
            isAttack = true;
            if (attackType == BreakLevel.easy)
            {

                switch (comboStep)
                {
                    case 0:
                        attackAudio.PlayOneShot(attackClip1);
                        break;
                    case 1:
                        attackAudio.PlayOneShot(attackClip2);
                        break;
                    case 2:
                        attackAudio.PlayOneShot(attackClip3);
                        break;
                    case 3:
                        attackAudio.PlayOneShot(attackClip4);
                        break;
                }
                //attackAudio.Play ();
                //                Debug.Log ("easyAttack");
                playerAnimator.SetTrigger("LightAttack");
                playerAnimator.SetInteger("ComboStep", comboStep + 1);
                comboStep = (comboStep + 1) % 4;
                controller.SetSpeed(controller.moveSpeed * controller.lightAttackMoveSpeedPer);
            }
            else if (attackType == BreakLevel.hard)
            {
                //       Debug.Log ("heavyAttack");
                playerAnimator.SetTrigger("HeavyAttack");
                attackAudio.PlayOneShot(HeavyAttackClip);
                controller.SetSpeed(controller.moveSpeed * controller.heavyAttackMoveSpeedPer);
            }
        }

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
        //attackAudio.Pause();

        timer = attackInterval;
        isAttack = false;
        controller.ResetSpeed();
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
                //使用次数减1,检查武器的使用次数
                armorTimes--;
                CheckArmor();
            }

            if (other.CompareTag("Enemy"))
            {
                if (attackType == BreakLevel.easy)
                    other.GetComponent<FSMBase>().TakenDamage(attackRatio[0], other.transform.position - this.transform.position);
                if (attackType == BreakLevel.hard)
                    other.GetComponent<FSMBase>().TakenDamage(attackRatio[1], other.transform.position - this.transform.position);

                /*                 if (mySprit.transform.localScale.x > 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.left);
                                else if (mySprit.transform.localScale.x <= 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                 */
            }
            if (other.CompareTag("Breakable"))
            {
                if (other.GetComponent<Breakable_Trigger>() != null)
                {
                    BreakLevel level = other.GetComponent<Breakable_Trigger>().level;
                    //如果攻击比他强
                    if ((int)attackType >= (int)level)
                        other.GetComponent<Breakable_Trigger>().Action();
                }
            }
        }

    }
    public void CheckArmor()
    {
        //        Debug.Log("使用武器，剩余次数："+armorTimes);
        if (armorTimes <= 0)
        {
            this.GetComponentInParent<PlayerController>().SetArmor(false, -1);
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
        if (controller.PressETarget.GetComponent<ConditionTrigger>() != null)
        {
            controller.PressETarget.GetComponent<ConditionTrigger>().StartTrigger();
        }
        //否则就是直接执行trigger 
        else
        {
            controller.PressETarget.GetComponent<ActiveTrigger>().StartTrigger();
        }
        StartCoroutine(pressDelay());
    }
    IEnumerator pressDelay()
    {
        yield return new WaitForSeconds(0.2f);
        controller.PressETarget = null;
    }
    public void SetBreakLevel(bool isArmor, int useNUm)
    {
        if (isArmor)
            attackType = BreakLevel.hard;
        else
            attackType = BreakLevel.easy;
        armorTimes = useNUm;
        //        Debug.Log("attackType:" + attackType.ToString());
    }
    public void fightController(float time)
    {
        fightLimit = true;
        Invoke("resetFightLimit", time);
    }
    private void resetFightLimit()
    {
        fightLimit = false;
    }

}