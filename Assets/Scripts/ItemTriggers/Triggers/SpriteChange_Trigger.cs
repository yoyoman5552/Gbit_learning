
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteChange_Trigger : ITrigger
{
    //TODO:动画的需求还得考虑
    public bool flag = true;
    public Sprite changeToSprite;
    private SpriteRenderer sprite;
    private Sprite originalSPrite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalSPrite = sprite.sprite;
    }

    public override void Action()
    {
        if (flag)
        {
            sprite.sprite = changeToSprite;
            transform.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
            sprite.sprite = originalSPrite;
    }



}