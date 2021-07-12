using EveryFunc;
//条件基类
public abstract class FSMTrigger
{
    //编号
    public FSMTriggerID triggerID { set; get; }
    public FSMTrigger()
    {
        Init();
    }
    //要求子类必须初始化条件，为编号赋值：abstract只能给方法，而abstract的方法要求子类必须重定义
    public abstract void Init();
    //逻辑处理
    public abstract bool HandleTrigger(FSMBase fsm);
}