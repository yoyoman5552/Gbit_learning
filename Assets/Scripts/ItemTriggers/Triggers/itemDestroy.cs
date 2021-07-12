/* using UnityEngine;

public class itemDestroy : ITrigger
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
            collision.gameObject.GetComponent<PlayerController>().touchDestroyItem = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().touchDestroyItem = false;
        }
    }
}
 */