using UnityEngine;
using System;
using System.Collections;
using EveryFunc;
public class Core3_Follow : IAttack
{
    [Header("共有变量")]
    [Tooltip("起点")]
    [Range(6, 12)]
    public int startDir = 6;
    [Tooltip("终点")]
    [Range(6, 12)]
    public int endDir = 12;
    [Tooltip("旋转角度")]
    public int angleInterval = 5;
    [Tooltip("连续发射间隔时间")]
    public float bulletBetweenTime = 0.3f;
    [Tooltip("连续发射的子弹数")]
    public int bulletNum = 10;

    [Header("私有变量")]
    private int dir;
    private float currentAngle;
    private float timer;
    private void Start()
    {
        if (startDir < endDir) dir = 1;
        else dir = -1;
        Init();
    }
    public override void Action(GameObject target)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        //开始连续子弹携程
        StartCoroutine(ContinuousBullet());
        timer = bulletBetweenTime * bulletNum;
    }

    public override void Init()
    {
        isAction = false;
        currentAngle = startDir * 15;
    }
    private IEnumerator ContinuousBullet()
    {
        int count = 0, flag = startDir > endDir ? -1 : 1;
        while (count < bulletNum)
        {
            int dir = startDir * ConstantList.ClockRatio;
            do
            {
                GameObject bullet = GameObject.Instantiate(ChangeBullet(), this.transform.position, Quaternion.identity);
                Vector3 targetDir = new Vector3(Mathf.Sin(dir), Mathf.Tan(dir));
                //发射子弹
                bullet.GetComponent<bulletController>().bulletFire(targetDir,bulletSpeed);
                yield return new WaitForSeconds(bulletBetweenTime);
                dir += flag * angleInterval;
            } while (dir != endDir * ConstantList.ClockRatio);
            count++;
        }

    }
    public override GameObject ChangeBullet()
    {
        return bullets[0];
    }
}