using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//状态基类
public abstract class PlayerFSMState {
    //状态类编号
    public PlayerFSMStateID stateID { set; get; }
    //条件列表
    public List<PlayerFSMTrigger> triggers;
    //要求子类必须初始化条件，为编号赋值：abstract只能给方法，而abstract的方法要求子类必须重定义
    public abstract void Init ();
    //映射表：字典
    public Dictionary<PlayerFSMTriggerID, PlayerFSMStateID> map;
    public PlayerFSMState () {
        //初始化
        map = new Dictionary<PlayerFSMTriggerID, PlayerFSMStateID> ();
        triggers = new List<PlayerFSMTrigger> ();
        Init ();
    }
    //检测当前状态的条件是否满足:满足就切换状态
    public void DetectTriggers (PlayerFSMBase PlayerFSM) {
        for (int i = 0; i < triggers.Count; i++) {
            //判断该条件是否满足
            if (triggers[i].HandleTrigger (PlayerFSM)) {
                //从映射表中获得该条件的映射状态
                PlayerFSMStateID nextState = map[triggers[i].triggerID];
                //切换状态
                PlayerFSM.ChangeActiveState (nextState);
                return;
            }
        }
    }
    //由状态机调用
    //为映射表和条件列表赋值
    public void addMap (PlayerFSMTriggerID triggerID, PlayerFSMStateID stateID) {
        //添加映射
        map.Add (triggerID, stateID);
        //添加条件对象
        CreateTriggerObject (triggerID);
    }
    public void ClearAll () {
        map.Clear ();
        triggers.Clear ();
    }
    private void CreateTriggerObject (PlayerFSMTriggerID triggerID) {
        //创建条件对象
        //命名规则：EveryFunc.PlayerFSM.+ triggerID + Trigger
        Type type = Type.GetType (triggerID + "Trigger");
        //创建新的条件对象
        PlayerFSMTrigger triggerOBJ = Activator.CreateInstance (type) as PlayerFSMTrigger;
        //添加对象
        triggers.Add (triggerOBJ);
    }
    //为具体类提供可选实现
    public virtual void EnterState (PlayerFSMBase PlayerFSM) { }
    public virtual void ActionState (PlayerFSMBase PlayerFSM) { }
    public virtual void FixedActionState (PlayerFSMBase PlayerFSM) { }
    public virtual void ExitState (PlayerFSMBase PlayerFSM) { }
}