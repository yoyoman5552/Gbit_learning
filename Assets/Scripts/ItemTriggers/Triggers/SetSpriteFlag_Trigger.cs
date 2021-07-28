using System;
using UnityEngine;

public class SetSpriteFlag_Trigger : ITrigger
{
    public SpriteRenderer targetSprite;
    public bool flag = true;

    public override void Action()
    {
        targetSprite.enabled=flag;
    }
}