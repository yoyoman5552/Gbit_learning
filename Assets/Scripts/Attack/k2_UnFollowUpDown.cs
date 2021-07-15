/* using UnityEngine;

public class k2_UnFollowUpDown : IAttack
{
    [Header("共有变量")]
    [Tooltip("起点")]
    [Range(6, 12)]
    public float startDir = 12;
    [Tooltip("终点")]
    [Range(6, 12)]
    public float endDir = 6;
    [Tooltip("旋转角度")]
    public float angleInterval = 15f;
    [Tooltip("间隔时间")]
    public float intervalTime = 0.25f;
    [Header("私有变量")]
    private int dir;
    private float currentAngle;
    private void Start()
    {
        if (startDir < endDir) dir = 1;
        else dir = -1;
       Init();
    }
    public override void Action(GameObject target)
    {
        
        //生成子弹实例
        GameObject bullet = GameObject.Instantiate(ChangeBullet(), this.transform.position, Quaternion.identity);
        Vector3 targetDir = new Vector3(Mathf.Sin(currentAngle), Mathf.Tan(currentAngle));

        //发射子弹
        Debug.Log("current Dir:" + targetDir);
        bullet.GetComponent<bulletController>().bulletFire(targetDir);

        //改变下一发的角度
        currentAngle += angleInterval * dir;
    }
    public override void Init()
    {
        currentAngle = startDir * 15;
    }
} */