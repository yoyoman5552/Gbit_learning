using System;
using EveryFunc;
using UnityEngine;
//使人跳跃
public class CharacterJump_Trigger : ITrigger {
    // public JumpLevel jumpLevel;
    public Transform jumpToPos;
    public override void Action () 
    {
        GameManager.Instance.playerController.PlayerJump (jumpToPos.position);
    }
}