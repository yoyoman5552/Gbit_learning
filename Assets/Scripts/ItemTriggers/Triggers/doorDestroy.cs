/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorDestroy : ITrigger
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Action()
    {
        //throw new System.NotImplementedException();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //TODO:???????
            collision.GetComponent<PlayerController>().touchDoorCanBeDestroy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //TODO:???????
            collision.GetComponent<PlayerController>().touchDoorCanBeDestroy = false;
        }
    }
}
 */