using System;
using System.Collections.Generic;
using EveryFunc;
using UnityEngine;
public class BossFSM : FSMBase
{
    [Header("Boss相关变量")]
    [Tooltip("受伤阈值")]
    public float hurtedThreshold;
    //当前受伤值
    //    [HideInInspector]
    public float m_hurtedCount;
    [Tooltip("炮台表，第一个为核心")]
    public GameObject[] batteryArray;
    //战斗状态
    [HideInInspector]
    public float attackStateNum = 0;
    [Tooltip("受伤的阈值")]
    public int[] hurtedMaxHP = new int[2];
    public GameObject generateWeapon;
    public GameObject generateItem;
    private int maxHP;
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
        //        idle.addMap(FSMTriggerID., FSMStateID.BossAttack);

        //attack的映射
        attack.addMap(FSMTriggerID.BossHurtedDone, FSMStateID.BossWeak);
        attack.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);
        //attack.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);

        //weak的映射
        weak.addMap(FSMTriggerID.BossWeakDone, FSMStateID.BossAttack);
        weak.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);
        // weak.addMap(FSMTriggerID.Dead, FSMStateID.BossDead);

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

        maxHP = HP;
        BossInit();
    }
    public override void TakenDamage(int damage, Vector3 dir)
    {
        //如果没血了
        if (HP <= 0) return;

        if (!isHurted)
        {
            HP = Mathf.Max(HP - damage, 0);
            m_hurtedCount += damage;
            isHurted = true;

            material.SetFloat("_FlashAmount", 0.4f);
            if (enemyAudio != null)
            {
                enemyAudio.clip = GetHurtClip;
                enemyAudio.Play();
            }
            StartCoroutine(hurtedContinus(hurtedTime));
        }
    }
    private void BossInit()
    {
        m_hurtedCount = 0;
        HP = maxHP;
        attackStateNum = 0;
        if (currentState != null)
            ChangeActiveState(FSMStateID.BossIdle);
    }
    public void GenerateWeapon()
    {
        GameObject obj = GameObject.Instantiate(generateWeapon, generateWeapon.transform.position, Quaternion.identity);
        obj.transform.SetParent(this.transform);
        obj.SetActive(true);
    }
}