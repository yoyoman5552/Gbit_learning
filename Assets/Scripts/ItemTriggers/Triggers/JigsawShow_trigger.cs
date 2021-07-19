using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawShow_trigger : ITrigger
{
    // Start is called before the first frame update
    public bool canShow = false;
    public override void Action()
    {
        //this.gameObject.SetActive(true);
        canShow = true;
    }
}
