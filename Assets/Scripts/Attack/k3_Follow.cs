using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class k3_Follow : IAttack
{
    [Header("共有变量")]
    [Tooltip("间隔时间")]
    public float intervalTime = 0.1f;
    [Tooltip("发射总个数")]
    public int shootTotalNum = 20;

    [Header("私有变量")]
    private float timer;

    private float currentNum;
    private void Start()
    {
        Init();
    }
    public override void Action(GameObject target)
    {
        if (currentNum >= shootTotalNum)
        {
            isAction = true;
            return;

        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        //生成子弹实例
        GameObject bullet = GameObjectPool.Instance.Instantiate(ChangeBullet().name, this.transform.position, Quaternion.identity);
        //发射子弹
        bullet.GetComponent<bulletController>().bulletFire(target.transform.position - this.transform.position,bulletSpeed);
        timer = intervalTime;
        currentNum++;
    }
    public override void Init()
    {
        isAction = false;
        currentNum = 0;
    }

}