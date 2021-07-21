using EveryFunc;
using UnityEngine;
public class AttackState : FSMState
{

    //攻击间隔：远程攻击变量
    //子弹发射攻速
    //private float initShootTimeGap = 0.4f;
    private float shootTimeGap;
    //判断当前是否已经发射子弹
    private bool hadShoot = false;


    //近战攻击变量
    //近战攻击：冲刺后近身攻击，当玩家与怪物距离至某一段距离时，恢复冲刺判断
    private bool firstDetectPlayer;

    //冲刺终点
    private Vector3 firstDetectPosition;

    //冲刺是否结束
    private bool finishAttack;

    //冲刺技能加载时间//可加感叹号供玩家反应
    private float sprintSkillCD;
    private float initSprintCDTimer = 8.0f;



    //防止在冲刺状态中被卡住后无法再次冲刺
    private float AttackEndTimer;
    private float initAttackEndTimer;


    //冲刺加载时间
    private float loadSprintTimer;
    private float initLoadSprintTimer = 1.0f;
 

    //冲刺后的近战普通攻击的加载时间
    private float meleeTimer;
    //private float initMeleeTimer = 0.5f;
    //private bool hadMelee = false;
    private float originRadius;



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
        

        //翻转贴图方向
        float dir = fsm.targetTF.position.x - fsm.transform.position.x;
        fsm.textureClip(dir);

        //        Debug.Log("attack state in");
        loadSprintTimer = initLoadSprintTimer;
        meleeTimer = fsm.attackInterval;
        shootTimeGap = fsm.attackInterval;
        sprintSkillCD = fsm.attackInterval;
        initAttackEndTimer = fsm.attackInterval + initLoadSprintTimer;
        AttackEndTimer = initAttackEndTimer;
        finishAttack = false;
        firstDetectPlayer = true;

    }
    public override void ActionState(FSMBase fsm)
    {

        if (fsm.AttackStyle)
        {
            RemoteAttack(fsm);
        }
        else
        {
            MeleeAttack(fsm);
            //Debug.Log(loadSprintTimer);
        }

    }
    public override void ExitState(FSMBase fsm)
    {
        //        Debug.Log("attack state out");
        

    }

    //远程攻击接口
    private void RemoteAttack(FSMBase fsm)
    {
        if (!hadShoot)
        {
            if (rayDetect(fsm))
            {
                hadShoot = true;
                remoteAttack_Achieve(fsm);
            }
        }
        else
        {
            shootTimeGap -= Time.deltaTime;
            if (shootTimeGap <= 0)
            {
                shootTimeGap = fsm.attackInterval;
                hadShoot = false;
            }
        }
    }
    //近战攻击接口

    private void MeleeAttack(FSMBase fsm)
    {
        //近战攻击方法：冲刺撞击玩家后对玩家进行近身攻击

        //冲刺
        //现有问题：冲刺冷却时间在玩家离开攻击范围后不会继续倒计时：目前通过设置当玩家进入攻击范围时，冲刺技能自动冷却。
        if (finishAttack)
        {
            
            Debug.Log("finish_Attack:resetCD: " + sprintSkillCD);

            //冲刺加载时间，可加 ！ 供玩家预知敌人即将发起冲刺
            //TODO:加标志
            sprintSkillCD -= Time.deltaTime;
            if (sprintSkillCD <= 0)
            {

                sprintSkillCD = initSprintCDTimer;
                loadSprintTimer = initLoadSprintTimer;
                AttackEndTimer = initAttackEndTimer;
                finishAttack = false;
                //重新检测玩家位置
                firstDetectPlayer = true;
                Debug.Log("resetCD_finish");
            }

        }
        else
        {
            if (rayDetect(fsm))
            {
                
                //冲刺加载

                //获取当前玩家位置
                if (firstDetectPlayer&&loadSprintTimer<=0.5f)
                {
                    firstDetectPosition = GameManager.Instance.player.transform.position;
                    firstDetectPlayer = false;
                }
                //TODO:冲刺前提示玩家
                //Debug.Log("Ready_to_Attack:Loading attack:load scale:"+loadSprintTimer);
                loadSprintTimer -= Time.deltaTime;
                if (loadSprintTimer <= 0)
                {
                    
                    //Debug.Log("ready_to_attack");
                    Sprint_Achieve(fsm);
                }

            }
            //防止冲刺失败后无法重置冲刺
            AttackEndTimer -= Time.deltaTime;
            if (AttackEndTimer <= 0)
            {
                finishAttack = true;
                sprintSkillCD = initSprintCDTimer;
                loadSprintTimer = initLoadSprintTimer;
                AttackEndTimer = initAttackEndTimer;
            }

        }
        /*
        //冲刺后的近身攻击
        if (!fsm.meleeAttackStyle)
        {
            if (meleeTimer <= 0)
            {
                melee();
                meleeTimer = initMeleeTimer;
            }
            meleeTimer -= Time.deltaTime;
        }
        */
    }
    //远程攻击实现
    private void remoteAttack_Achieve(FSMBase fsm)
    {
        //寻找主人  
        Transform enemyTransform = fsm.transform;

        //寻找玩家
        Transform playerTransform = GameManager.Instance.player.transform;
        //if (playerTransform == null) Debug.Log(1);
        GameObject bullet = GameObjectPool.Instance.Instantiate("RedBullet", fsm.transform.position, Quaternion.identity);
        if (bullet != null)
        {
            bullet.SetActive(true);
            bullet.transform.position = enemyTransform.position;
            bullet.GetComponent<bulletController>().bulletFire(playerTransform.position - enemyTransform.position, 3f);
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


    //冲刺实现
    private void Sprint_Achieve(FSMBase fsm)
    {
        //RaycastHit2D ray = Physics2D.
        Transform EnemyTransform = fsm.transform;
  
        //近战攻击：冲刺

        //射线检测突进路径上的物体

        //与玩家直线上没有其他障碍物则进行冲刺

        //TODO：有障碍物时 缩小攻击范围，使其重新搜索玩家 或 进入待机状态

        //实现方法：通过chaseState中增加射线检测碰撞实现

        EnemyTransform.position = Vector3.Lerp(EnemyTransform.position, firstDetectPosition, 10 * Time.deltaTime);
        if (Vector3.Distance(EnemyTransform.position, firstDetectPosition) < 0.5f)
        {
            //fsm.attackRadius = 1.0f;
            //fsm.meleeAttackStyle = false;
            //未命中玩家，到达指定位置，攻击结束
            finishAttack = true;
            Debug.Log("Sprint_attack_finish");
        }

        //攻击范围检测
        var targetArray = Physics2D.OverlapCircleAll(fsm.transform.position, 1f);
        foreach (var target in targetArray)
        {
            if (target.CompareTag("Player"))
            {
                //命中玩家攻击结束
                finishAttack = true;
                Debug.Log("Sprint_attack_finish_attack_sucess");
                target.GetComponent<PlayerController>().TakenDamage(fsm.damage, 4 * (target.transform.position - fsm.transform.position));
                break;
            }
        }

    }

    private float detectDistance(FSMBase fsm)
    {
        Transform Player = GameManager.Instance.player.transform;
        Transform meleeEnemy = fsm.transform;
        //Debug.Log(Mathf.Sqrt((Player.position - meleeEnemy.position).sqrMagnitude));
        return (Vector3.Distance(Player.position, meleeEnemy.position));
    }

    private bool rayDetect(FSMBase fsm)
    {
        Vector3 rayDirection = fsm.targetTF.position - fsm.transform.position;
        Vector3 detectRayPosition = fsm.transform.position + 0.5f*rayDirection.normalized;
        RaycastHit2D hit = Physics2D.Raycast(detectRayPosition, rayDirection, detectDistance(fsm), LayerMask.GetMask("Default"));
        if (hit.collider != null && hit.collider.name == "PlayerCircleDetect")
        {
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