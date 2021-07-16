using UnityEngine;
using EveryFunc;
using System.Collections;

public class Core1_UnFollow : IAttack
{
    [Header("共有变量")]
    [Tooltip("起点")]
    [Range(6, 12)]
    public int startDir = 6;
    [Tooltip("终点")]
    [Range(6, 12)]
    public int endDir = 12;
    [Tooltip("旋转角度")]
    public int angleInterval = 15;
    [Tooltip("连续发射间隔时间")]
    public float bulletBetweenTime = 0.1f;
    [Tooltip("连续发射的子弹数")]
    public int bulletNum = 10;
    [Header("私有变量")]
    private int dir;
    private int currentAngle;
    private float timer;
    private float currentCount;
    private void Start()
    {
        if (startDir < endDir) dir = 1;
        else dir = -1;
        Init();
    }
    public override void Init()
    {
        timer = bulletBetweenTime;
        currentCount = 0;
        isAction = false;
    }
    public override void Action(GameObject target)
    {
        if (currentCount >= bulletNum)
        {
            isAction = true;
            return;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = bulletBetweenTime;
        currentCount++;
        //发射子弹
        currentAngle = startDir * ConstantList.ClockRatio;
        GameObject bullet = GameObjectPool.Instance.Instantiate(ChangeBullet().name, this.transform.position, Quaternion.identity);
        Vector3 targetDir = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad));
        bullet.GetComponent<bulletController>().bulletFire(targetDir, bulletSpeed);
        while (Mathf.Abs(currentAngle - endDir * ConstantList.ClockRatio) >= angleInterval)
        {
            currentAngle += angleInterval * dir;
            bullet = GameObjectPool.Instance.Instantiate(ChangeBullet().name, this.transform.position, Quaternion.identity);
            targetDir = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad));
            //发射子弹
            bullet.GetComponent<bulletController>().bulletFire(targetDir, bulletSpeed);
        }
    }

    private IEnumerator ContinuousBullet()
    {
        int count = 0;
        while (count < bulletNum)
        {
            GameObject bullet = GameObject.Instantiate(ChangeBullet(), this.transform.position, Quaternion.identity);
            Vector3 targetDir = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad));
            //发射子弹
            bullet.GetComponent<bulletController>().bulletFire(targetDir, bulletSpeed);
            yield return new WaitForSeconds(bulletBetweenTime);
        }
        currentAngle += angleInterval * dir;
    }
/*     public override GameObject ChangeBullet()
    {
        return bullets[0];
    } */

}