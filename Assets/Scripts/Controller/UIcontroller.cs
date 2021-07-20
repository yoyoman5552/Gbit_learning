using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public Sprite detailSprite;
    private int getNumForManager = 0;
    private ItemTrigger activeTrigger;
    public string detailName;
    public string detailIndex;
    public string jigsawName;
    public string jigsawIndex;
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
                UIManager.Instance.CallDetailUI("", "", detailSprite);
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
