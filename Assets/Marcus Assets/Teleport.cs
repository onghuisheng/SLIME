using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour {

    public GameObject Exit;
    private GameObject ObjToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
            return;

        ObjToTeleport = other.gameObject;
        ObjToTeleport.SetActive(false);

        Invoke("TeleportAfterTime", 3);
    }

    void TeleportAfterTime()
    {
        ObjToTeleport.SetActive(true);
        var agent = ObjToTeleport.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.Warp(Exit.transform.position);
        }

    }
}
