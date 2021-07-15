using UnityEngine;

public abstract class IAttack : MonoBehaviour
{
    [Tooltip("使用的子弹")]
    public GameObject[] bullets;
    [Tooltip("是否完成攻击")]
    [HideInInspector]
    public bool isAction;

    /// <summary>
    /// 攻击方法
    /// </summary>
    public abstract void Action();
    
    /// <summary>
    /// 切换子弹方法，默认为随机切换,返回切换的子弹
    /// </summary>
    public virtual GameObject ChangeBullet()
    {
        return bullets[Random.Range(0, bullets.Length)];
    }
}