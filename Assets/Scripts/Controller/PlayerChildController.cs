using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class PlayerChildController : MonoBehaviour {
    //�����
    public float shakeTime;
    public int lightPause;
    public float lightStrength;
    public int heavyPause;
    public float heavyStrength;
    [Tooltip ("伤害间隔")]
    public float attackInterval = 0.4f;
    public BreakLevel attackType;
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
    //    private Vector3 childCorrectScale = new Vector3(1, 1, 1);
    private void Awake () {
        mySprit = GetComponent<SpriteRenderer> ();
        playerAnimator = GetComponent<Animator> ();
        controller = GetComponentInParent<PlayerController> ();
    }
    // Start is called before the first frame update
    void Start () {
        timer = attackInterval;
        fightLimit = false;
    }

    // Update is called once per frame
    void Update () {
        if (!fightLimit)
            Attack ();

    }
    private void FixedUpdate () {

    }
    private void Attack () {
        //        childCorrectScale.x = mySprit.flipX ? 1 : -1;
        //        this.transform.GetChild(0).localScale = childCorrectScale;
        if (Input.GetKeyDown (KeyCode.J) && !isAttack) {

            //缺少将hitonece = false玩家第二段攻击失效
            isAttack = true;
            if (attackType == BreakLevel.easy) {
                Debug.Log ("easyAttack");
                playerAnimator.SetTrigger ("LightAttack");
                playerAnimator.SetInteger ("ComboStep", comboStep + 1);
                comboStep = (comboStep + 1) % 4;
            controller.SetSpeed (controller.moveSpeed * controller.lightAttackMoveSpeedPer);
            } else if (attackType == BreakLevel.hard) {
                Debug.Log ("heavyAttack");
                playerAnimator.SetTrigger ("HeavyAttack");
            controller.SetSpeed (controller.moveSpeed * controller.heavyAttackMoveSpeedPer);
            }
        }
        /*         if (Input.GetKeyDown(KeyCode.K) && !isAttack)
                {

                    hitOnce = false;

                    isAttack = true;
                    attackType = BreakLevel.hard;
                    playerAnimator.SetTrigger("HeavyAttack");
                    playerAnimator.SetInteger("ComboStep", heavyAttack);
                } */
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else if (timer > -10) {
            timer = -999;
            comboStep = 0;
        }
    }
    public void AttackOver () {
        timer = attackInterval;
        isAttack = false;
        controller.ResetSpeed ();
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if ((other.CompareTag ("Breakable") || other.CompareTag ("Enemy"))) {
            if (attackType == BreakLevel.easy) {
                AttackSense.Instance.HitPause (lightPause);
                AttackSense.Instance.CameraShake (shakeTime, lightStrength);

            } else if (attackType == BreakLevel.hard) {
                AttackSense.Instance.HitPause (heavyPause);
                AttackSense.Instance.CameraShake (shakeTime, heavyStrength);
            }

            if (other.CompareTag ("Enemy")) {
                other.GetComponent<FSMBase> ().TakenDamage ((int) attackType + 1, other.transform.position - this.transform.position);

                /*                 if (mySprit.transform.localScale.x > 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.left);
                                else if (mySprit.transform.localScale.x <= 0)
                                    other.GetComponent<Enemy>().GetHit(Vector2.right);
                 */
            }
            if (other.CompareTag ("Breakable")) {
                if (other.GetComponent<Breakable_Trigger> () != null) {
                    BreakLevel level = other.GetComponent<Breakable_Trigger> ().level;
                    //如果攻击比他强
                    if ((int) attackType >= (int) level)
                        other.GetComponent<Breakable_Trigger> ().Action ();
                }
            }
        }

    }
    public void SetBoolDown (string animationFlag) {
        playerAnimator.SetBool (animationFlag, false);
    }
    public void SetBoolUp (string animationFlag) {
        playerAnimator.SetBool (animationFlag, false);
    }
    public void PressEAction () {
        Debug.Log ("Press E Action");
        if (controller.PressETarget.GetComponent<ConditionTrigger> () != null) {
            controller.PressETarget.GetComponent<ConditionTrigger> ().StartTrigger ();
        }
        //否则就是直接执行trigger 
        else {
            controller.PressETarget.GetComponent<ActiveTrigger> ().StartTrigger ();
        }
        controller.PressETarget = null;
    }
    public void SetBreakLevel (bool isArmor) {
        if (isArmor)
            attackType = BreakLevel.hard;
        else
            attackType = BreakLevel.easy;
        Debug.Log ("attackType:" + attackType.ToString ());
    }
    public void fightController (float time) {
        fightLimit = true;
        Invoke ("resetFightLimit", time);
    }
    private void resetFightLimit () {
        fightLimit = false;
    }
}