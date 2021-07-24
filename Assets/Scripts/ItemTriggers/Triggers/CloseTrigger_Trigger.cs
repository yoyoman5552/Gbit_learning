using EveryFunc;
using UnityEngine;
using System.Collections;
//关闭EButton，关闭执行
public class CloseTrigger_Trigger : ITrigger
{
    public override void Action()
    {
        this.transform.Find("EButton").GetComponent<SpriteRenderer>().enabled = false;
//        StartCoroutine(SmoothShow(-1, this.gameObject));
        foreach (var trigger in this.GetComponents<ItemTrigger>())
        {
            trigger.isActive = false;
        }
    }
/*     IEnumerator SmoothShow(int flag, GameObject target)
    {
        float x = 1 - flag;
        int dir = x > flag ? -1 : 1;
        while (x * dir < flag)
        {
            x = x + dir * Time.deltaTime * 4;
            yield return new WaitForSeconds(Time.deltaTime);
            target.GetComponentInChildren<SpriteRenderer>().material.SetFloat("IsActive", x);
        }
    } */
}