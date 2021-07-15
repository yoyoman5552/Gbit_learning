using UnityEngine;

public class k1_UnFollowUpDown : IAttack
{
    [Header("共有变量")]
    [Tooltip("起点")]
    [Range(6, 12)]
    public float startDir = 12;
    [Tooltip("终点")]
    [Range(6, 12)]
    public float endDir = 6;
    [Tooltip("旋转角度")]
    public float angle = 15f;
    [Tooltip("间隔时间")]
    public float intervalTime = 0.25f;
    [Header("私有变量")]
    private int dir;
    private void Start()
    {
        if (startDir < endDir) dir = 1;
        else dir = -1;
    }
    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}