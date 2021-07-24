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


    //近战变量
    private float loadSprintTimer;
    
    private bool loading = true;
    private bool firstAttack = true;

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
        // Debug.Log("inAttack");
        loadSprintTimer = fsm.initLoadStimer;
        //翻转贴图方向
        
        //fsm.textureClip(dir);

        //        Debug.Log("attack state in");


        shootTimeGap = fsm.attackInterval;

        if(fsm.AttackStyle) fsm.animator.SetBool("attack", true);

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
            if(fsm.Sprinting&&firstAttack)
            {
                var targetArray = Physics2D.OverlapCircleAll(fsm.transform.position, 0.5f);
                foreach (var target in targetArray)
                {
                    if (target.CompareTag("Player"))
                    {
                        //命中玩家攻击结束
                        Debug.Log("Sprint_attack_finish_attack_sucess");
                        target.GetComponent<PlayerController>().TakenDamage(fsm.damage, 4 * (target.transform.position - fsm.transform.position));
                        firstAttack = false;
                        break;
                    }
                }
            }
        }

    }
    public override void ExitState(FSMBase fsm)
    {
        //        Debug.Log("attack state out");
        if(fsm.AttackStyle)
            fsm.animator.SetBool("attack", false);
    }

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

    private void MeleeAttack(FSMBase fsm)
    {
        if (!fsm.SprintUsed && rayDetect(fsm))
        {



            if (loading)
            {
                fsm.SprintDir = (fsm.targetTF.position - fsm.transform.position).normalized;
                loading = false;
            }


            //冲刺加载
            loadSprintTimer -= Time.deltaTime;
            //加载完成
            if (loadSprintTimer < 0)
                fsm.Sprinting = true;


        }
        else
        {
            firstAttack = true;
            loading = true;
            loadSprintTimer = fsm.initLoadStimer;
        }


        
    }
    //远程攻击实现
    private void remoteAttack_Achieve(FSMBase fsm)
    {
        //寻找主人  
        //Transform enemyTransform = fsm.transform;

        //寻找玩家
        Transform playerTransform = GameManager.Instance.player.transform;
        //if (playerTransform == null) Debug.Log(1);
        Vector3 FixEnemyPosition = fsm.transform.position;
        FixEnemyPosition.y += 0.5f;
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


    //冲刺实现

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
        Vector3 detectRayPosition = fsm.transform.position + 0.5f * rayDirection.normalized;
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