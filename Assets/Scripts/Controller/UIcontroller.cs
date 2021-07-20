using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public Sprite mysprite;
    private int getNumForManager = 0;
    private ItemTrigger activeTrigger;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetUIfalse();
    }
    private void SetUIfalse()
    {
        if (Input.anyKeyDown)
        {
            getNumForManager = UIManager.Instance.getAllWindow();
            if (getNumForManager == 2)
            {
                UIManager.Instance.CallDetailUI("", "", mysprite);
            }
            else if(getNumForManager == 1)
            {
                UIManager.Instance.CallJigsawUI("","");
            }
            else
                UIManager.Instance.resetTimeScale();
            this.gameObject.SetActive(false);
            UIManager.Instance.CheckContinueTrigger();
        }

    }
}
