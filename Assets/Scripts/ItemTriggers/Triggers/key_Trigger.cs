/* 
using UnityEngine;

public class key_Trigger : ITrigger
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
        if(collision.tag=="Player")
        {
            collision.GetComponent<PlayerController>().isMeetKeyorNot = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().isMeetKeyorNot = false;
        }
    }
}
 */