using System.Collections;
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

    public bool m_OnFire;
    
    private void Start()
    {
        m_OnAnim = true;
        m_OnFire = false;
    }

    public virtual void OnShot(ArrowBase arrow)
    {
        if (arrow != null && arrow.arrowType == ArrowBase.ArrowType.None && m_SlimeBase.slimeType == SlimeBase.SlimeType.Catapult) // Normal arrow shouldnt damage catapult slimes
            return;

        m_SlimeBase.DeductHealth(Damage); // deduct 1hp when hit

        if (m_SlimeBase.GetHealth() <= 0) // die when 0 or lesser hp
        {
            m_SlimeBase.anim.speed = 1; // Resume animation if flashed
            m_SlimeBase.toDespawn = true;

            if(SlimeManager.instance != null)
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

                SlimeDeath m_slimeDeath = GetComponentInParent<SlimeDeath>();

                if (m_slimeDeath == null)
                {
                    m_slimeDeath = GetComponentInChildren<SlimeDeath>();
                }

                if (m_slimeDeath.m_Despawn)
                    m_slimeDeath.m_Despawn = false;

                m_slimeDeath.RemoveFromScene();
            }

            if (m_Agent)
                m_Agent.enabled = false;
        }
    }
}
