using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCatapult : MonoBehaviour {

    public GameObject m_Catapult;
    public GameObject m_Destination;

    private bool m_IsSpawned;

	// Use this for initialization
	void Start () {
        m_IsSpawned = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnCatapultInScene()
    {
        if (m_IsSpawned)
            return;

        GameObject catapult = Instantiate(m_Catapult, transform.position, Quaternion.identity);
        catapult.GetComponent<NavMeshAgent>().SetDestination(m_Destination.transform.position);
        catapult.GetComponent<SlimeBase>().anim.SetBool("IsMove", true);
        m_IsSpawned = true;
    }
}
