/* using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EButtonShow_Trigger : ITrigger
{
    private GameObject ButtonE;
    private void Awake()
    {
        ButtonE = this.transform.Find("EButton").gameObject;
        ButtonE.SetActive(false);
    }
    public override void Action()
    {
        ButtonE.SetActive(true);
    }
    
} */