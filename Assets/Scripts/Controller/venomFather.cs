using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class venomFather : MonoBehaviour
{
    public bool enter;
    public bool firstIn;
    // Start is called before the first frame update
    void Start()
    {
        firstIn = true; ;
        enter = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enter = true;
        if (firstIn)
            firstIn = false; 

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        enter = false;
        firstIn = true;
    }
}
