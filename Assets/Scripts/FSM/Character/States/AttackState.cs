using EveryFunc;
using UnityEngine;
public class AttackState : FSMState
{
    //攻击间隔：远程攻击变量
    private float initShootTimeGap = 0.4f;
    private float shootTimeGap;
    private bool hadShoot = false;

    //近战攻击变量
    private bool firstDetectPlayer = true;
    private Vector3 firstDetectPosition;
    private bool finishAttack = false;
    private float meleeTimer;
    private float initMeleeTimer = 5.0f;
    private float AttackEndTimer;
    private float initAttackEndTimer = 2.0f;
    
    //用于暂存初始攻击距离的变量
    private float attackArea = 3;


    public override void Init()
    {
        stateID = FSMStateID.Attack;
        //        throw new System.NotImplementedException();
        shootTimeGap = initShootTimeGap;
        meleeTimer = initMeleeTimer;
        AttackEndTimer = initAttackEndTimer;
    }
    public override void EnterState(FSMBase fsm)
    {
        Debug.Log("attack state in");
       
    }
    public override void ActionState(FSMBase fsm)
    {
        
        //TODO:远程攻击方法：现在为测试近战，暂时取消
        if (!hadShoot)
        {
            hadShoot = true;
            //remoteAttack(fsm);
        }
        else
        {
            shootTimeGap -= Time.deltaTime;
            if(shootTimeGap<=0)
            {
                shootTimeGap = initShootTimeGap;
                hadShoot = false;
            }
        }



        //近战攻击方法
        if(finishAttack)
        {
            meleeTimer -= Time.deltaTime;
            if(meleeTimer<=0)
            {
                meleeTimer = initMeleeTimer;
                finishAttack = false;
                //重新检测玩家位置
                firstDetectPlayer = true;
            }
        }
        else
        {
            //敌人短暂蓄力后突进
            //TODO:短暂蓄力
            Melee(fsm);
            AttackEndTimer -= Time.deltaTime;
            if(AttackEndTimer<=0)
            {
                finishAttack = true;
                AttackEndTimer = initAttackEndTimer;
            }
        }


    }
    public override void ExitState(FSMBase fsm)
    {
        Debug.Log("attack state out");
        
    }

    //远程攻击
    private void remoteAttack(FSMBase fsm)
    {
        //寻找主人  //暂时
        Transform enemyTransform = fsm.transform;

        //寻找玩家
        Transform playerTransform = GameManager.Instance.player.transform;
        //if (playerTransform == null) Debug.Log(1);
        GameObject bullet = bulletPool.bulletPoolInstance.askForBullet();
        if (bullet != null)
        {
            bullet.SetActive(true);
            bullet.transform.position = enemyTransform.position;
            bullet.GetComponent<bulletController>().bulletFire(playerTransform.position-enemyTransform.position);
            //bullet.transform.position = Vector3.Lerp(bullet.transform.position, playerTransform.position, 2f * Time.deltaTime);
        }

        //多线弹幕初步测试
        
        //偏转角度
        /*
        float offsetRight = 270;
        float offsetLeft = -270;
        GameObject leftBullet = bulletPool.bulletPoolInstance.askForBullet();
        if(leftBullet!=null)
        {
            leftBullet.SetActive(true);
            leftBullet.transform.position = enemyTransform.position;
            Vector3 fireDiretion = playerTransform.position - enemyTransform.position;
            float addAngleX = fireDiretion.x;
            float addAngleY = fireDiretion.y;
            fireDiretion.x = addAngleX * Mathf.Cos(offsetLeft) + addAngleY * Mathf.Sin(offsetLeft);
            fireDiretion.y = addAngleX * Mathf.Sin(-offsetLeft) + addAngleY * Mathf.Cos(offsetLeft);
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
            fireDiretion.x = addAngleX * Mathf.Cos(offsetRight) + addAngleY * Mathf.Sin(offsetRight);
            fireDiretion.y = addAngleX * Mathf.Sin(-offsetRight) + addAngleY * Mathf.Cos(offsetRight);
            //fireDiretion = (leftQuaternion + nowRotation).eulerAngles;
            rightBullet.GetComponent<bulletController>().bulletFire(fireDiretion);
            //playerTransform.up*leftQuaternion
        }
        */
    }
    private void sprintPlayerDetect()
    {

    }

    //近战
    private void Melee(FSMBase fsm)
    {
        //RaycastHit2D ray = Physics2D.
        Transform EnemyTransform = fsm.transform;
        if (firstDetectPlayer)
        {
            firstDetectPosition = GameManager.Instance.player.transform.position;
            firstDetectPlayer = false;
        }


        //近战攻击：冲刺

        //射线检测突进路径上的物体

        //与玩家直线上没有其他障碍物则进行冲刺

        //TODO：有障碍物时 缩小攻击范围，使其重新搜索玩家 或 进入待机状态


        Vector3 rayDirection = firstDetectPosition - EnemyTransform.position;
        Vector3 detectRayPosition = EnemyTransform.position + 0.5f*rayDirection.normalized;
        EnemyTransform.position = Vector3.Lerp(EnemyTransform.position, firstDetectPosition, 10 * Time.deltaTime);
        if ((EnemyTransform.position - firstDetectPosition).sqrMagnitude < 0.5f)
        {
            finishAttack = true;
            Debug.Log("Melee_attack_finish");
        }

    }
}