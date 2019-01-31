using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCatapult : MonoBehaviour
{

    public GameObject m_Catapult;
    public GameObject m_Destination;

    public int m_WaveToSpawn;

    private bool m_IsSpawned;

    // Use this for initialization
    void Start()
    {
        m_IsSpawned = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (SlimeManager.instance.m_CurrentWave == m_WaveToSpawn && !m_IsSpawned)
        {
            StartCoroutine(SpawnCatapultInScene());
        }
    }

    private IEnumerator SpawnCatapultInScene()
    {
        m_IsSpawned = true;

        yield return new WaitForSeconds(Random.Range(7.0f, 12.0f));

        GameObject catapult = Instantiate(m_Catapult, transform.position, Quaternion.identity);
        catapult.GetComponent<NavMeshAgent>().SetDestination(m_Destination.transform.position);
        catapult.GetComponent<SlimeBase>().anim.SetBool("IsMove", true);
        SlimeManager.instance.m_SlimeInScene.Add(catapult);

        // Determine left/right position
        if (Camera.main.transform.position.x > transform.position.x)
        {
            CommanderSpeaker.Instance.PlaySpeaker("npc_catapultl" + Random.Range(1, 3), AudioManager.AudioType.Additive);
        }
        else
        {
            CommanderSpeaker.Instance.PlaySpeaker("npc_catapultr" + Random.Range(1, 3), AudioManager.AudioType.Additive);
        }
    }
}
