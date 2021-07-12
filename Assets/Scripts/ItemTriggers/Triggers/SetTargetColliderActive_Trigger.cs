using UnityEngine;

public class SetTargetColliderActive_Trigger : ITrigger
{
    public GameObject target;
    public bool flag;
    private Collider2D[] colliders;
    private void Awake()
    {
        colliders = target.GetComponents<Collider2D>();
    }
    public override void Action()
    {
        foreach (var collider in colliders)
        {
            collider.enabled = flag;
        }
    }
}