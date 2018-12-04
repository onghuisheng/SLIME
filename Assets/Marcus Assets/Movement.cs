using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {

    public List<GameObject> m_pathList; // list of the path that the ai can travel

    public int m_index; // index for which node the ai should travel to next

	// Use this for initialization
	void Start () {
        m_index = 0;
        GetComponent<NavMeshAgent>().SetDestination(m_pathList[m_index].transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        if (m_index < m_pathList.Count - 1)
        {
            if (Vector3.Distance(transform.position, m_pathList[m_index].transform.position) > 0.1f)
            {
                return;
            }

            m_index++;
            GetComponent<NavMeshAgent>().SetDestination(m_pathList[m_index].transform.position);
        }
	}
}
