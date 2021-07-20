using System;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.U2D;
public class SetGlobalLight_Trigger : ITrigger {
    public Color color = Color.white;
    [Tooltip("范围0~1")]
    public float intensity=1;

    public override void Action () {
        //GameManager.Instance.globalLight.color = color;
        //GameManager.Instance.globalLight.intensity = intensity;
        //        target.GetComponent<ItemTrigger>().StartTrigger();
    }
}