using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCatapult : MonoBehaviour {

    public GameObject m_Catapult;
    public GameObject m_Destination;

    public int m_WaveToSpawn;

    private bool m_IsSpawned;

	// Use this for initialization
	void Start () {
        m_IsSpawned = false;

    }
	
	// Update is called once per frame
	void Update () {
		if(SlimeManager.instance.m_CurrentWave == m_WaveToSpawn)
        {
            SpawnCatapultInScene();
        }
	}

    public void SpawnCatapultInScene()
    {
        if (m_IsSpawned)
            return;

        GameObject catapult = Instantiate(m_Catapult, transform.position, Quaternion.identity);
        catapult.GetComponent<NavMeshAgent>().SetDestination(m_Destination.transform.position);
        catapult.GetComponent<SlimeBase>().anim.SetBool("IsMove", true);
        SlimeManager.instance.m_SlimeInScene.Add(catapult);
        m_IsSpawned = true;
    }
}
