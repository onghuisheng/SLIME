﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHit : MonoBehaviour, IShootable
{

    [SerializeField]
    private SlimeBase m_SlimeBase;
    public SlimeBase slimeBase { get { return m_SlimeBase; } }

    [SerializeField]
    private NavMeshAgent m_Agent;

    [SerializeField]
    private GolemSlimeDefend m_Defend;


    public int Damage;

    public bool m_OnAnim;


    private void Start()
    {
        m_OnAnim = true;
    }

    public virtual void OnShot(ArrowBase arrow)
    {
        m_SlimeBase.DeductHealth(Damage); // deduct 1hp when hit

        if (m_SlimeBase.GetHealth() <= 0) // die when 0 or lesser hp
        {
            m_SlimeBase.anim.speed = 1; // Resume animation if flashed
            m_SlimeBase.toDespawn = true;
            SlimeManager.instance.GetComponent<SlimeManager>().Remove();

            if (m_OnAnim)
            {
                m_SlimeBase.anim.SetBool("IsDead", true);

                if (m_Defend != null)
                {
                    m_SlimeBase.anim.SetBool("IsDefending", false);
                    foreach (Collider m_Collider in m_SlimeBase.gameObject.GetComponentsInChildren<Collider>())
                    {
                        m_Collider.enabled = false;
                    }
                }
            }

            else if (!m_OnAnim)
            {
                if (GetComponentInParent<GolemDeath>() != null)
                    GetComponentInParent<GolemDeath>().RemoveSlimeBody();

                GetComponentInParent<SlimeDeath>().RemoveFromScene();
            }

            if (m_Agent)
                m_Agent.enabled = false;
        }
    }
}
