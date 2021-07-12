using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class PlayerCircleDetect : MonoBehaviour {
    private CircleCollider2D detectCollider;
    //交互物品列表
    public List<GameObject> interactList;
    private GameObject curTarget;
    private void Awake () {
        detectCollider = GetComponent<CircleCollider2D> ();
    }
    private void Update () {
        SortList ();
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Interactive")||other.gameObject.CompareTag ("QuickInteractive")) {
            
            if (!interactList.Contains (other.gameObject)) {
                interactList.Add (other.gameObject);
            }
        }
    }
    private void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Interactive")||other.gameObject.CompareTag ("QuickInteractive")) {
            if (interactList.Contains (other.gameObject)) {
                interactList.Remove (other.gameObject);
            }
        }
    }
    public void SortList () {
        interactList.Sort ((s, x) => Vector3.Distance (s.transform.position, this.transform.position).CompareTo (Vector3.Distance (x.transform.position, this.transform.position)));
        if (interactList.Count > 0) {
            ShowButtonE (interactList[0]);
        } else {
            ShowButtonE(null);
        }
    }
    //显示EButton
    public void ShowButtonE (GameObject target) {
        if (curTarget != target)
            SetButtonShow (false);
        curTarget = target;
        SetButtonShow (true);
    }
    //设置EButton
    public void SetButtonShow (bool flag) {
        if (curTarget != null)
            curTarget.transform.Find ("EButton").gameObject.SetActive (flag);
    }
    //获得最近的物体
    public GameObject GetFirst () {
        return curTarget;
    }
}