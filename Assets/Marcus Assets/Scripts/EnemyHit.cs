using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHit : MonoBehaviour, IShootable
{

    [SerializeField]
    private SlimeBase m_SlimeBase;

    [SerializeField]
    private NavMeshAgent m_Agent;

    [SerializeField]
    private GolemSlimeDefend m_Defend;


    public int Damage;

    public virtual void OnShot(ArrowBase arrow)
    {
        m_SlimeBase.DeductHealth(Damage); // deduct 1hp when hit

        if (m_SlimeBase.GetHealth() <= 0) // die when 0 or lesser hp
        {
            m_SlimeBase.toDespawn = true;
            SlimeManager.instance.GetComponent<SlimeManager>().Remove();


            m_SlimeBase.anim.SetBool("IsDead", true);

            if (m_Defend != null)
            {
                m_SlimeBase.anim.SetBool("IsDefending", false);
            }

            if (m_Agent)
                m_Agent.enabled = false;
        }
    }
}
