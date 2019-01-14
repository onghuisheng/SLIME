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

        if (confusionParticlePrefab != null)
        {
            anim.speed = 0;

            var boxCollider = GetComponent<BoxCollider>();

            GameObject particle = null;

            if (!m_IsStunned)
            {
                particle = Instantiate(confusionParticlePrefab, (m_BaseJoint != null) ? m_BaseJoint : anim.transform);
                particle.transform.position = m_BaseJoint.transform.position;
                particle.transform.rotation = Quaternion.identity;
                // particle.transform.Translate(0, boxCollider.bounds.max.y, 0, Space.Self); // TODO: SET THE PARTICLE TO BE ON TOP OF ENEMY

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
