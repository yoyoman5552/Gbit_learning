using System.Collections.Generic;
using System;
using UnityEngine;
namespace EveryFunc.FSM
{
    public class BossFSM : FSMBase
    {
        [Header("Boss相关变量")]
        [Tooltip("受伤阈值")]
        public float hurtedThreshold;
        //当前受伤值
        [HideInInspector]
        public float m_hurtedCount;
        [Tooltip("炮台表，第一个为核心")]
        public GameObject[] batteryArray;
        public override void ConfigFSM()
        {
            if (statesList != null) return;
            statesList = new List<FSMState>();
            //创建状态对象
            FSMState idle = new BossIdleState();
            FSMState weak = new BossWeakState();
            FSMState attack = new BossAttackState();
            FSMState dead = new BossDeadState();
            //FSMState dead = new DeadState();
            //添加映射(AddMap) 
            //idle的映射
            idle.addMap(FSMTriggerID.IdleDone, FSMStateID.BossAttack);

            //attack的映射
            attack.addMap(FSMTriggerID.BossHurtedDone, FSMStateID.BossWeak);
            attack.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);

            //weak的映射
            weak.addMap(FSMTriggerID.BossWeakDone, FSMStateID.BossAttack);
            weak.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);



            /*             idle.addMap (FSMTriggerID.NoHealth, FSMStateID.Dead);
                        idle.addMap (FSMTriggerID.IsHurted, FSMStateID.Hurted);
                        idle.addMap (FSMTriggerID.DetectTarget, FSMStateID.Chase);
                        idle.addMap (FSMTriggerID.CompleteIdle, FSMStateID.Patrol);
             */

            //加入状态机
            statesList.Add(idle);
            statesList.Add(attack);
            statesList.Add(weak);
            statesList.Add(dead);

            BossInit();
        }
        private void BossInit()
        {
            m_hurtedCount = 0;
        }
    }
}