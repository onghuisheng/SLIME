using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimePatrol : MonoBehaviour {

    public List<GameObject> m_WaypointList;
    private int m_Index;

	// Use this for initialization
	void Start () {
        GetComponent<NavMeshAgent>().Move(m_WaypointList[0].transform.position);
        m_Index = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_WaypointList[m_Index].transform.position) < 0.1f)
        {
            m_Index++;

            if (m_Index > m_WaypointList.Count - 1)
                m_Index = 0;

            GetComponentInChildren<Animator>().Play("Walking");
        }
    }

    /// <summary>
    /// Move when the slime is jumping (animation)
    /// </summary>
    public void Move()
    {
        if (GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<NavMeshAgent>().SetDestination(m_WaypointList[m_Index].transform.position);
        }
    }

    /// <summary>
    /// Stop when the slime is not jumping
    /// </summary>
    public void Stop()
    {
        if (GetComponent<NavMeshAgent>().enabled)
            GetComponent<NavMeshAgent>().SetDestination(transform.position);

    }
}
