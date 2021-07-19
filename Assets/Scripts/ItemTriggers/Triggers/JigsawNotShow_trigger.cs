using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawNotShow_trigger : ITrigger
{
    // Start is called before the first frame update
    public override void Action()
    {
        //this.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        //print("aaaaaaaaaaaaaaaaaaa");
    }
}
