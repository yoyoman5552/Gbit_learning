using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class bulletPool : MonoBehaviour
{
    public static bulletPool bulletPoolInstance;
    public GameObject bulletPrefab;
    private List<GameObject> bulletObjects = new List<GameObject>();
    private int bulletAmount = 0;
    public int initBulletAmount;
    private bool destroying;
    private void Awake()
    {
        if (bulletPoolInstance != null)
        {
            Debug.Log("bulletPool重复实例");
            Destroy(gameObject);
            return;
        }
        bulletPoolInstance = this;
    }
    void Start()
    {
        initBulletPool();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void initBulletPool()
    {
        if(bulletAmount!=0)
        {

        }
        bulletAmount = initBulletAmount;
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bulletCreate = Instantiate(bulletPrefab);
            bulletCreate.SetActive(false);
            bulletObjects.Add(bulletCreate);
        }
        destroying = false;
    }

    public GameObject askForBullet()
    {
        if (!destroying)
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                if (!bulletObjects[i].activeInHierarchy)
                {
                    return bulletObjects[i];
                }
            }
            GameObject newBullet = Instantiate(bulletPrefab);
            bulletObjects.Add(newBullet);
            bulletAmount++;
            //print(bulletObjects.Count);
            return newBullet;
        }
        return null;
    }
    public void DestroyBulletsInPool()
    {
        destroying = true;
        int lenght = bulletAmount;
        for (int i = 0; i < lenght; i++)
        {
            GameObject bulletDestroy = bulletObjects[bulletAmount-1];
            bulletObjects.Remove(bulletObjects[bulletAmount-1]);
            Destroy(bulletDestroy);
            bulletAmount--;

        }
        initBulletPool();
    }
}

