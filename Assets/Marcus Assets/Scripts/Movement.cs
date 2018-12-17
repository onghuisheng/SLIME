using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{

    public List<GameObject> m_pathList; // list of the path that the ai can travel

    public int m_index; // index for which node the ai should travel to next
    
    // Use this for initialization
    void Start()
    {
        m_index = Random.Range(0, m_pathList.Count);
        GetComponent<NavMeshAgent>().SetDestination(m_pathList[m_index].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_pathList[m_index].transform.position) < 0.1f)
        {
            m_index = Random.Range(0, m_pathList.Count);
            //GetComponent<NavMeshAgent>().SetDestination(m_pathList[m_index].transform.position);

            GetComponentInChildren<Animator>().Play("Walking");
        }
    }

    /// <summary>
    /// Move when the slime is jumping (animation)
    /// </summary>
    public void Move() 
    {
        GetComponent<NavMeshAgent>().SetDestination(m_pathList[m_index].transform.position);
    }

    /// <summary>
    /// Stop when the slime is not jumping
    /// </summary>
    public void Stop()
    {
        GetComponent<NavMeshAgent>().SetDestination(transform.position);

    }
}
