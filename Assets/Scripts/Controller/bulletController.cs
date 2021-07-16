using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class bulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float setBulletFalseTimer;
    public float initSetBulletFalseTimer;
    private bool isShoot = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        setBulletFalseTimer = initSetBulletFalseTimer;
    }
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShoot)
        {
            setBulletFalseTimer -= Time.deltaTime;
            if(setBulletFalseTimer<=0)
            {
                
                this.gameObject.SetActive(false);
                initTimeFlag();
            }
        }
    }
    public void bulletFire(Vector3 playerPosition,float bulletSpeed)
    {
        isShoot = true;
        playerPosition = playerPosition.normalized;
        //this.transform.position = Vector3.Lerp(transform.position, playerPosition, bulletSpeed * Time.deltaTime);
        if(rb!=null) rb.AddForce(playerPosition*bulletSpeed*ConstantList.speedPer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            this.gameObject.SetActive(false);
            initTimeFlag();
            
        }
        else if(collision.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            initTimeFlag();
        }
    }
    private void initTimeFlag()
    {
        isShoot = false;
        setBulletFalseTimer = initSetBulletFalseTimer;
    }

}
