using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public SceneController myScenController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void myClick()
    {
        myScenController.mouseClick = true;
        //Debug.Log("click");
    }
}
