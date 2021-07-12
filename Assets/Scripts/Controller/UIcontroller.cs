using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
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
            this.gameObject.SetActive(false);
            UIManager.Instance.resetTimeScale();
        }

    }
}
