using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EveryFunc;
public class bulletController : MonoBehaviour
{
    public float seeV;
    private Rigidbody2D rb;
    private float setBulletFalseTimer;
    public float initSetBulletFalseTimer;
    private bool isShoot = false;
    public int damage = 1;
    private Material material;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        setBulletFalseTimer = initSetBulletFalseTimer;
        material = GetComponent<SpriteRenderer>().material;
    }
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //测试用
        bulletFire(Vector3.left,3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            setBulletFalseTimer -= Time.deltaTime;
            if (setBulletFalseTimer <= 0)
            {

                this.gameObject.SetActive(false);
                initTimeFlag();
            }
        }
    }
    public void bulletFire(Vector3 playerPosition, float bulletSpeed)
    {
        isShoot = true;
        playerPosition = playerPosition.normalized;

        //子弹转向
        /*
        if (playerPosition.x > 0.05f)
            material.SetFloat("RotateDir", 1);
        else if (playerPosition.x < -0.05f)
            material.SetFloat("RotateDir", -1);
        */
        //this.transform.position = Vector3.Lerp(transform.position, playerPosition, bulletSpeed * Time.deltaTime);
        if (rb != null) rb.AddForce(playerPosition * bulletSpeed * ConstantList.speedPer);
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
        {
            this.gameObject.SetActive(false);
            initTimeFlag();

        }
        else if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>().TakenDamage(damage, collider.transform.position - transform.position);
            this.gameObject.SetActive(false);
            initTimeFlag();
        }
    }
    private void initTimeFlag()
    {
        isShoot = false;
        setBulletFalseTimer = initSetBulletFalseTimer;
    }
    public  void bulletInit()
    {
        Vector3 initV = new Vector3(0, 0, 0);
        rb.velocity = initV;
    }

}
