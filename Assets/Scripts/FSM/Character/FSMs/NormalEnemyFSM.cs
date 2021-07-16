using System.Collections.Generic;

namespace EveryFunc.FSM
{
    public class NormalEnemyFSM : FSMBase
    {
        public override void ConfigFSM()
        {
            if (statesList != null) return;
            statesList = new List<FSMState>();
            //创建状态对象
            FSMState idle = new IdleState();
            //FSMState dead = new DeadState();
            FSMState patrol = new PatrolState();
            FSMState chase = new ChaseState();
            FSMState attack = new AttackState();
            FSMState hurted = new HurtedState();
            FSMState dead = new DeadState();
            //添加映射(AddMap) 
            //idle的映射
            idle.addMap(FSMTriggerID.Dead, FSMStateID.Dead);
            idle.addMap(FSMTriggerID.GetHurted, FSMStateID.Hurted);
            idle.addMap(FSMTriggerID.IdleDone, FSMStateID.Patrol);
            idle.addMap(FSMTriggerID.TargetFound, FSMStateID.Chase);

            /*             idle.addMap (FSMTriggerID.NoHealth, FSMStateID.Dead);
                        idle.addMap (FSMTriggerID.IsHurted, FSMStateID.Hurted);
                        idle.addMap (FSMTriggerID.DetectTarget, FSMStateID.Chase);
                        idle.addMap (FSMTriggerID.CompleteIdle, FSMStateID.Patrol);
             */
            //patrol的映射
            patrol.addMap(FSMTriggerID.Dead, FSMStateID.Dead);
            patrol.addMap(FSMTriggerID.GetHurted, FSMStateID.Hurted);
            patrol.addMap(FSMTriggerID.PatrolDone, FSMStateID.Idle);
            patrol.addMap(FSMTriggerID.TargetFound, FSMStateID.Chase);
            //patrol.addMap(FSMTriggerID.TargetLost, FSMStateID.Idle);

            //chase的映射
            chase.addMap(FSMTriggerID.Dead, FSMStateID.Dead);
            chase.addMap(FSMTriggerID.GetHurted, FSMStateID.Hurted);
            chase.addMap(FSMTriggerID.TargetLost, FSMStateID.Idle);
            chase.addMap(FSMTriggerID.TargetGet, FSMStateID.Attack);
            //TODO:attack和Dead
            //attack的映射
            attack.addMap(FSMTriggerID.Dead, FSMStateID.Dead);
            attack.addMap(FSMTriggerID.GetHurted, FSMStateID.Hurted);
            attack.addMap(FSMTriggerID.TargetLost, FSMStateID.Idle);
            attack.addMap(FSMTriggerID.OutOfAttackRange, FSMStateID.Chase);

            //hurted的映射
            hurted.addMap(FSMTriggerID.Dead, FSMStateID.Dead);

            //加入状态机
            statesList.Add(idle);
            statesList.Add(patrol);
            statesList.Add(chase);
            statesList.Add(attack);
            statesList.Add(hurted);
            statesList.Add(dead);
        }
    }
}