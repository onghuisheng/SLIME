using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeBase : MonoBehaviour
{

    public int m_MaxHealth; // max health
    public int m_MaxAttack; // max attack

    private int m_Health; // health
    private int m_Attack; // attack

    public Animator anim;

    public GameObject m_DeathParticles;

    [HideInInspector]
    public bool toDespawn;

    public float m_OriginalSpeed;

    [SerializeField]
    private Transform m_BaseJoint;

    private bool m_IsStunned = false;

    // Use this for initialization
    void Start()
    {
        toDespawn = false;

        m_Health = m_MaxHealth;
        m_Attack = m_MaxAttack;

        var agent = GetComponent<NavMeshAgent>();

        if (agent)
            m_OriginalSpeed = agent.speed;
    }

    public void DeductHealth(int toDeduct)
    {
        m_Health -= toDeduct;

        if (GetComponent<GolemSlimeDefend>() != null)
        {
            GetComponent<GolemSlimeDefend>().ChangeToDefend();
        }
    }

    public int GetHealth()
    {
        return m_Health;
    }

    public void ApplyConfusion(float duration, GameObject confusionParticlePrefab)
    {
        if (m_IsStunned)
        {
            StopCoroutine(ConfusionRemovalRoutine(null, 0));
        }

        if (confusionParticlePrefab != null && m_BaseJoint != null)
        {
            anim.speed = 0;

            var boxCollider = GetComponent<BoxCollider>();

            GameObject particle = null;

            if (!m_IsStunned)
            {
                particle = Instantiate(confusionParticlePrefab, m_BaseJoint);

                Vector3 spawnPos = m_BaseJoint.transform.position;

                var agent = GetComponent<NavMeshAgent>();
                if (agent)
                    spawnPos.y += agent.height;
                else
                {
                    spawnPos.y += Mathf.Abs(m_BaseJoint.InverseTransformPoint(boxCollider.bounds.max).y * 1.1f);
                    particle.transform.localScale = Vector3.one;
                }

                particle.transform.position = spawnPos;
                particle.transform.rotation = Quaternion.identity;

                m_IsStunned = true;
            }

            var movement = GetComponent<Movement>();
            if (movement)
            {
                movement.Stop();
            }

            StartCoroutine(ConfusionRemovalRoutine(particle, duration));
        }
    }

    IEnumerator ConfusionRemovalRoutine(GameObject confusionParticle, float duration)
    {
        yield return new WaitForSeconds(duration);
        anim.speed = 1;
        m_IsStunned = false;

        if (confusionParticle)
            Destroy(confusionParticle);
    }

}
