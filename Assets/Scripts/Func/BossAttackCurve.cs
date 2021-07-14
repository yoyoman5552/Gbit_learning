using UnityEngine;
using EveryFunc;
public class BossAttackCurve
{
    [Tooltip("状态列表")]
    public int[] stateList;
    [Tooltip("持续时间")]
    public float durationTime;
    public BossAttackCurve()
    {
        stateList = new int[ConstantList.batteryCount];
    }
}
