using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class buttonController : MonoBehaviour
{

    // Û±Í—°‘Ò
    public GameObject canvas;
   
    public Button buttonEnter;
    public Button buttonExit;

    public Text buttonEnter_text;
    public Text buttonExit_text;

    private Color initColor;
    public Color changeColor;

    //º¸≈Ã—°‘Ò
    private int whichSelect;


    // Start is called before the first frame update
    void Start()
    {
        initColor = buttonEnter_text.color;
        whichSelect = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetOverUI(canvas);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

        }



    }


    private void GetOverUI(GameObject canvas)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            foreach(RaycastResult myresult in results)
            {
                if(myresult.gameObject.name == buttonEnter.name)
                {
                    buttonEnter_text.color = changeColor;
                }
                else
                {
                    buttonEnter_text.color = initColor;
                }
                if(myresult.gameObject.name == buttonExit.name)
                {
                    buttonExit_text.color = changeColor;
                }
                else
                {
                    buttonExit_text.color = initColor;
                }
            }
           

        }
        else
        {
            buttonExit_text.color = initColor;
            buttonEnter_text.color = initColor;
        }
        
    }

}
