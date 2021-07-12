using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//创建弹幕对象池
public class bulletPool : MonoBehaviour
{
    public static bulletPool bulletPoolInstance;
    public GameObject bulletPrefab;
    private List<GameObject> bulletObjects = new List<GameObject>();
    public int bulletAmount;
    // Start is called before the first frame update
    private void Awake()
    {
        bulletPoolInstance = this;
    }
    void Start()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bulletCreate = Instantiate(bulletPrefab);
            bulletCreate.SetActive(false);
            bulletObjects.Add(bulletCreate);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject askForBullet()
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
        return newBullet;
    }
}

