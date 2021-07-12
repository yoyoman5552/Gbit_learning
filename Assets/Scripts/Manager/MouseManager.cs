using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/* 
/// <summary>
/// 拖拽的方式
/// </summary>
[Serializable]
public class EventVector3 : UnityEvent<Vector3> { } */
public class MouseManager : MonoBehaviour
{
    public Texture2D point, doorway, attack, target, arrow;
    private Texture2D currentCursorTexture;
    public static MouseManager Instance;
    private void Awake()
    {
        //单例模式：仅有一个实例
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
        currentCursorTexture = null;
    }
    RaycastHit2D hitInfo;

    //事件：鼠标事件、敌人事件
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    private void Update()
    {
        SetCursorTexture();
        MouseControll();
    }
    void SetCursorTexture()
    {
        //        Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z=0f;
        hitInfo = Physics2D.Raycast(mousePos, Vector3.right*-1, 1f);
        if (hitInfo.collider != null)
        {
            //TODO:切换鼠标贴图
            /* //切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    ChangeCursorTexture(target);
                    break;
                case "Enemy":
                    ChangeCursorTexture(attack);
                    break;
            } */
        }
    }
    void ChangeCursorTexture(Texture2D nextTexture)
    {
        if (currentCursorTexture != nextTexture)
        {
            currentCursorTexture = nextTexture;
            Cursor.SetCursor(nextTexture, new Vector2(16, 16), CursorMode.Auto);
        }
    }
    void MouseControll()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                Debug.Log("hit ground:"+hitInfo.collider.transform.position);
                OnMouseClicked?.Invoke(hitInfo.collider.transform.position);
            }
            else if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }
}