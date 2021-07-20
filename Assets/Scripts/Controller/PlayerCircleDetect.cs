using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
using System.Collections;
public class PlayerCircleDetect : MonoBehaviour
{
    private CircleCollider2D detectCollider;
    //交互物品列表
    public List<GameObject> interactList;
    private GameObject curTarget;
    private Coroutine showCoroutine;
    private void Awake()
    {
        detectCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        SortList();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactive") || other.gameObject.CompareTag("QuickInteractive"))
        {

            if (!interactList.Contains(other.gameObject))
            {
                interactList.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactive") || other.gameObject.CompareTag("QuickInteractive"))
        {
            if (interactList.Contains(other.gameObject))
            {
                interactList.Remove(other.gameObject);
            }
        }
    }
    public void SortList()
    {
        interactList.Sort((s, x) => Vector3.Distance(s.transform.position, this.transform.position).CompareTo(Vector3.Distance(x.transform.position, this.transform.position)));
        if (interactList.Count > 0)
        {
            ShowButtonE(interactList[0]);
        }
        else
        {
            ShowButtonE(null);
        }
    }
    //显示EButton
    public void ShowButtonE(GameObject target)
    {
        if (curTarget != target)
            SetButtonShow(false);
        curTarget = target;
        SetButtonShow(true);
    }
    //设置EButton
    public void SetButtonShow(bool flag)
    {
        if (curTarget != null)
        {
            curTarget.transform.Find("EButton").gameObject.SetActive(flag);
            //curTarget.GetComponentInChildren<SpriteRenderer>().material.SetFloat("IsActive", flag ? 1 : 0);
            if (flag)
            {
                if (showCoroutine == null)
                {
                    showCoroutine = StartCoroutine(SmoothShow(1,curTarget));
                }
            }
            else
            {
                if (showCoroutine != null)
                {
                    StartCoroutine(SmoothShow(0,curTarget));
                    showCoroutine = null;
                }
            }

        }
    }
    IEnumerator SmoothShow(int flag,GameObject target)
    {
        float x = 1 - flag;
        int dir = x > flag ? -1 : 1;
        while (x * dir < flag)
        {
            x = x + dir * Time.deltaTime * 4;
            yield return new WaitForSeconds(Time.deltaTime);
            target.GetComponentInChildren<SpriteRenderer>().material.SetFloat("IsActive", x);
        }
    }
    //获得最近的物体
    public GameObject GetFirst()
    {
        return curTarget;
    }
}