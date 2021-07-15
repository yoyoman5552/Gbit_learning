using UnityEngine;
using EveryFunc;
public class k1_UnFollowDownUp : IAttack
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
    [Tooltip("间隔时间")]
    public float intervalTime = 0.25f;
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
        //如果在冷却
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        //生成子弹实例
        GameObject bullet = GameObjectPool.Instance.Instantiate(ChangeBullet().name, this.transform.position, Quaternion.identity);
        Vector3 targetDir = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad));

        //发射子弹
        //Debug.Log("current Dir:" + targetDir);
        bullet.GetComponent<bulletController>().bulletFire(targetDir, bulletSpeed);

        if (Mathf.Abs(currentAngle - (endDir * ConstantList.ClockRatio)) < angleInterval)
        {
            isAction = true;
        }
        //改变下一发的角度
        currentAngle += angleInterval * dir;
        timer = intervalTime;
    }
    public override void Init()
    {
        currentAngle = startDir * ConstantList.ClockRatio;
        timer = 0;
        isAction = false;
    }
}