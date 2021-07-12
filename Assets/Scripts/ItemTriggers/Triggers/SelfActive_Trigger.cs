
using UnityEngine;

public class SelfActive_Trigger : ITrigger
{
    public bool flag = true;
    private void Start()
    {
        //transform.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.SetActive(flag);
    }
 
    public override void Action()
    {
        throw new System.NotImplementedException();
    }


} 