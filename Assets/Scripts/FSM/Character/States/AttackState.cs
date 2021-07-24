using EveryFunc;
using UnityEngine;
using System.Collections;
public class AttackState : FSMState
{

    //攻击间隔：远程攻击变量
    //子弹发射攻速
    //private float initShootTimeGap = 0.4f;
    private float shootTimeGap;
    //判断当前是否已经发射子弹
    private bool hadShoot = false;


    //近战变量
    //加载冲刺时间
    private float loadSprintTimer;
    //冲刺时间
    private float sprintTimer;
//   private bool loading = true;
    private bool isHurtPlayer;

    public override void Init()
    {

        stateID = FSMStateID.Attack;
        //        throw new System.NotImplementedException();

        //倒计时变量的初始化
        /*         shootTimeGap = initShootTimeGap;
                sprintTimer = initSprintTimer;
                AttackEndTimer = initAttackEndTimer;
         */        //        meleeTimer = initMeleeTimer;
    }

    public override void EnterState(FSMBase fsm)
    {
        if (fsm.AttackStyle)
        {
            //如果是远程
            fsm.initSprintTimer = -1;
            shootTimeGap = fsm.attackInterval;
            fsm.animator.SetBool("attack", true);
            
        }
        else
        {
            //近战初始化
            isHurtPlayer = false;
            loadSprintTimer = fsm.initLoadStimer;
            sprintTimer = fsm.initSprintTimer;
            fsm.animator.SetBool("Sprint", true);
        }

        
    }

    public override void ActionState(FSMBase fsm)
    {



        if (fsm.AttackStyle)
        {
            //远程攻击
            RemoteAttack(fsm);
        }
        else
        {
            //近战攻击
            MeleeAttack(fsm);

        }

    }

    public override void ExitState(FSMBase fsm)
    {
        //        Debug.Log("attack state out");
        if (fsm.AttackStyle)
            fsm.animator.SetBool("attack", false);
        else
        {
            fsm.animator.SetBool("Sprint", false);
        }
        fsm.initLoadStimer = loadSprintTimer;
        fsm.initSprintTimer = sprintTimer;
        fsm.Sprinting = false;
    }
    /* private IEnumerator CheckDamage(FSMBase fsm)
    {
        while (fsm.initSprintTimer > 0)
        {
            fsm.initSprintTimer -= Time.deltaTime;
            //yield return new WaitForSeconds();
        }
    } */
    //远程攻击接口
    private void RemoteAttack(FSMBase fsm)
    {
        if (!hadShoot)
        {
            if (rayDetect(fsm))
            {
                float dir = fsm.targetTF.position.x - fsm.transform.position.x;
                fsm.textureClip(dir);
                hadShoot = true;
                remoteAttack_Achieve(fsm);
                fsm.enemyAudio.PlayOneShot(fsm.attackClip);
            }
        }
        else
        {
            //fsm.animator.SetBool("attack", false);
            shootTimeGap -= Time.deltaTime;
            if (shootTimeGap <= 0)
            {
                shootTimeGap = fsm.attackInterval;
                hadShoot = false;
            }
        }
    }
    //近战攻击接口

    private void StartSprint(FSMBase fsm)
    {
        //开始冲刺
        fsm.enemyAudio.PlayOneShot(fsm.attackClip);
        fsm.Sprinting = true;
        fsm.SprintDir = (fsm.targetTF.position - fsm.transform.position).normalized;
        fsm.initSprintTimer = sprintTimer;
        fsm.textureClip(fsm.SprintDir.x);
    }
    //远程攻击实现
    private void remoteAttack_Achieve(FSMBase fsm)
    {
        //寻找主人  
        //Transform enemyTransform = fsm.transform;

        //寻找玩家
        Transform playerTransform = fsm.targetTF;
        //if (playerTransform == null) Debug.Log(1);
        Vector3 FixEnemyPosition = fsm.transform.position;
        FixEnemyPosition.y += 0.5f;
        //Debug.Log("target:" + fsm.targetTF.name + "," + fsm.targetTF.position + ",self:" + FixEnemyPosition);
        GameObject bullet = GameObjectPool.Instance.Instantiate("RedBullet", FixEnemyPosition, Quaternion.identity);
        if (bullet != null)
        {
            bullet.SetActive(true);
            //bullet.transform.position = enemyTransform.position;
            bullet.GetComponent<bulletController>().bulletFire(playerTransform.position - FixEnemyPosition, 3f);
            //bullet.transform.position = Vector3.Lerp(bullet.transform.position, playerTransform.position, 2f * Time.deltaTime);
        }

        //发射多条弹幕
        #region description
        //多线弹幕初步测试

        /*
        //偏转角度
        
        float offsetRight = 15;
        float offsetLeft = -15;
        
        GameObject leftBullet = bulletPool.bulletPoolInstance.askForBullet();
        if(leftBullet!=null)
        {
            leftBullet.SetActive(true);
            leftBullet.transform.position = enemyTransform.position;
            Vector3 fireDiretion = playerTransform.position - enemyTransform.position;
            fireDiretion = fireDiretion.normalized;
            
            float addAngleX = fireDiretion.x;
            float addAngleY = fireDiretion.y;

            fireDiretion.x = addAngleX * Mathf.Cos(offsetLeft*Mathf.Deg2Rad) + addAngleY * Mathf.Sin(offsetLeft * Mathf.Deg2Rad);
            fireDiretion.y = addAngleX * -Mathf.Sin(offsetLeft * Mathf.Deg2Rad) + addAngleY * Mathf.Cos(offsetLeft * Mathf.Deg2Rad);

            //fireDiretion = (leftQuaternion + nowRotation).eulerAngles;
            leftBullet.GetComponent<bulletController>().bulletFire(fireDiretion);
            //playerTransform.up*leftQuaternion
        }
        GameObject rightBullet = bulletPool.bulletPoolInstance.askForBullet();
        if (rightBullet != null)
        {
            rightBullet.SetActive(true);
            rightBullet.transform.position = enemyTransform.position;
            Vector3 fireDiretion = playerTransform.position - enemyTransform.position;
            float addAngleX = fireDiretion.x;
            float addAngleY = fireDiretion.y;
            fireDiretion.x = addAngleX * Mathf.Cos(offsetRight * Mathf.Deg2Rad) + addAngleY * Mathf.Sin(offsetRight * Mathf.Deg2Rad);
            fireDiretion.y = addAngleX * -Mathf.Sin(offsetRight * Mathf.Deg2Rad) + addAngleY * Mathf.Cos(offsetRight * Mathf.Deg2Rad);
            //fireDiretion = (leftQuaternion + nowRotation).eulerAngles;
            rightBullet.GetComponent<bulletController>().bulletFire(fireDiretion);
            //playerTransform.up*leftQuaternion
        }
        */
        #endregion
    }
    //近战攻击
    private void MeleeAttack(FSMBase fsm)
    {
        //如果不是冲刺中
        if (!fsm.Sprinting)
        {
            //延迟cd还没结束
            if (fsm.initLoadStimer > 0)
            {
                fsm.initLoadStimer -= Time.deltaTime;
            }
            //开始冲刺
            else
            {
                //Debug.Log("开始冲刺");
                StartSprint(fsm);
            }
        }
        //是冲刺中，判断伤害
        else
        {
            //判断伤害
            CheckDamage(fsm);
            //计算cd
            fsm.initSprintTimer -= Time.deltaTime;
            if (fsm.initSprintTimer <= 0)
            {
                //Debug.Log("冲刺结束");
                fsm.initLoadStimer = loadSprintTimer;
                fsm.Sprinting = false;
            }
        }
    }
    private void CheckDamage(FSMBase fsm)
    {
        if (isHurtPlayer) return;
        var targetArray = Physics2D.OverlapCircleAll(fsm.transform.position, 0.5f);
        foreach (var target in targetArray)
        {
            if (target.CompareTag("Player"))
            {
                isHurtPlayer = true;
                fsm.StartCoroutine(HurtWait(1f));
                //命中玩家攻击结束
                Debug.Log("Sprint_attack_finish_attack_sucess");
                target.GetComponent<PlayerController>().TakenDamage(fsm.damage, (target.transform.position - fsm.transform.position));
                break;
            }
        }
    }
    IEnumerator HurtWait(float timer)
    {
        yield return new WaitForSeconds(timer);
        isHurtPlayer = false;
    }

    private float detectDistance(FSMBase fsm)
    {
        return (Vector3.Distance(fsm.targetTF.position, fsm.transform.position));
    }

    private bool rayDetect(FSMBase fsm)
    {
        Vector3 rayDirection = fsm.targetTF.position - fsm.transform.position;
        Vector3 detectRayPosition = fsm.transform.position + 0.5f * rayDirection.normalized;
        RaycastHit2D hit = Physics2D.Raycast(detectRayPosition, rayDirection, detectDistance(fsm), LayerMask.GetMask("Default"));
        if (hit.collider != null && hit.collider.name == "PlayerCircleDetect")
        {
            //            fsm.attackRadius =;
            return true;
        }
        else
        {
            /*             if (hit.collider != null)
                            Debug.Log("hit:" + hit.collider.name);
             */
            fsm.attackRadius = 0;
            return false;
        }
    }
    private void melee()
    {
        //TODO:近战攻击动画
        //玩家受伤

    }
}