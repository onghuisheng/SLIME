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
        StopCoroutine(ConfusionRemovalRoutine(null, 0));

        if (confusionParticlePrefab != null)
        {
            anim.speed = 0;

            var boxCollider = GetComponent<BoxCollider>();
            Vector3 spawnPos = boxCollider.bounds.center;
            spawnPos.y = boxCollider.bounds.max.y;

            GameObject particle = Instantiate(confusionParticlePrefab, anim.transform);
            particle.transform.position = spawnPos;

            StartCoroutine(ConfusionRemovalRoutine(particle, duration));
        }
    }

    IEnumerator ConfusionRemovalRoutine(GameObject confusionParticle, float duration)
    {
        yield return new WaitForSeconds(duration);
        anim.speed = 1;
        Destroy(confusionParticle);
    }

}
