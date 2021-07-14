using EveryFunc;
using UnityEngine;
public class AttackState : FSMState
{
    private float initShootTimeGap = 0.5f;
    private float shootTimeGap;
    private bool hadShoot = false;
    public override void Init()
    {
        stateID = FSMStateID.Attack;
        //        throw new System.NotImplementedException();
        shootTimeGap = initShootTimeGap;
        
    }
    public override void EnterState(FSMBase fsm)
    {
        Debug.Log("attack state in");
    }
    public override void ActionState(FSMBase fsm)
    {
        //TODO:攻击方法
        if (!hadShoot)
        {
            hadShoot = true;
            remoteAttack(fsm);
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
    }
    public override void ExitState(FSMBase fsm)
    {
        Debug.Log("attack state out");
    }
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
        
    }
}