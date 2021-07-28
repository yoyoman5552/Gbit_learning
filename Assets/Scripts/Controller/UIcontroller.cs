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
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("IsShow"))
            SetUIfalse();
    }
    private void SetUIfalse()
    {
//        Debug.Log("currentUI:" + this.gameObject.name + "," + Input.GetKeyDown(KeyCode.E));
        if (Input.GetKeyDown(KeyCode.E))
        {
            getNumForManager = UIManager.Instance.getAllWindow();
            if (getNumForManager == 2)
            {
                UIManager.Instance.CallDetailUI(detailName, detailTitle, detailIndex, detailSprite, true);

            }
            else if (getNumForManager == 1)
            {
                UIManager.Instance.CallJigsawUI(jigsawName,  jigsawIndex);
            }
            else
                UIManager.Instance.resetTimeScale();
            if (!UIManager.Instance.CheckContinueTrigger())
            {
                if (this.GetComponent<Animator>() != null)
                    this.GetComponent<Animator>().SetBool("IsShow", false);
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            //否则此对话框不关闭
        }

    }
}
