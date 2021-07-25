using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public Sprite detailSprite;
    private int getNumForManager = 0;
    private ItemTrigger activeTrigger;
    public string detailName;
    public string detailTitle;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            getNumForManager = UIManager.Instance.getAllWindow();
            if (getNumForManager == 2)
            {
                UIManager.Instance.CallDetailUI(detailName,detailTitle, detailIndex, detailSprite,true);

            }
            else if(getNumForManager == 1)
            {
                UIManager.Instance.CallJigsawUI(jigsawName,jigsawIndex);
            }
            else
                UIManager.Instance.resetTimeScale();
            if (!UIManager.Instance.CheckContinueTrigger())
            {
                this.gameObject.SetActive(false);
            }
            //否则此对话框不关闭
        }

    }
}
