using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject target1, target2;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = target1.GetComponent<LineRenderer>();
        line.SetPosition(0, target1.transform.position);
        line.SetPosition(1, target2.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
