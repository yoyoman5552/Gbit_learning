using System;
using System.Collections;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
//状态基类
public abstract class FSMState
{
    //状态类编号
    public FSMStateID stateID { set; get; }
    //条件列表
    public List<FSMTrigger> triggers;
    //要求子类必须初始化条件，为编号赋值：abstract只能给方法，而abstract的方法要求子类必须重定义
    public abstract void Init();
    //映射表：字典
    public Dictionary<FSMTriggerID, FSMStateID> map;
    public FSMState()
    {
        //初始化
        map = new Dictionary<FSMTriggerID, FSMStateID>();
        triggers = new List<FSMTrigger>();
        Init();
    }
    //检测当前状态的条件是否满足:满足就切换状态
    public void DetectTriggers(FSMBase fsm)
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            //判断该条件是否满足
            if (triggers[i].HandleTrigger(fsm))
            {
                //从映射表中获得该条件的映射状态
                FSMStateID nextState = map[triggers[i].triggerID];
                //切换状态
                fsm.ChangeActiveState(nextState);
                return;
            }
        }
    }
    //由状态机调用
    //为映射表和条件列表赋值
    public void addMap(FSMTriggerID triggerID, FSMStateID stateID)
    {
        //添加映射
        map.Add(triggerID, stateID);
        //添加条件对象
        CreateTriggerObject(triggerID);
    }
    public void ClearAll()
    {
        map.Clear();
        triggers.Clear();
    }
    private void CreateTriggerObject(FSMTriggerID triggerID)
    {
        //创建条件对象
        //命名规则：EveryFunc.FSM.+ triggerID + Trigger
        Type type = Type.GetType(triggerID + "Trigger");
        //创建新的条件对象
        FSMTrigger triggerOBJ = Activator.CreateInstance(type) as FSMTrigger;
        //添加对象
        triggers.Add(triggerOBJ);
    }
    //为具体类提供可选实现
    public virtual void EnterState(FSMBase fsm) { }
    public virtual void ActionState(FSMBase fsm) { }
    public virtual void ExitState(FSMBase fsm) { }
}