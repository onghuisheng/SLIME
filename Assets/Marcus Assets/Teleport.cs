using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour {

    public GameObject Exit;

    private void OnTriggerEnter(Collider other)
    {
        var agent = other.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.Warp(Exit.transform.position);
        }
        
    }
}
