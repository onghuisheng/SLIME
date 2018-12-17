using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour {

    public GameObject Exit;
    //private GameObject ObjToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
            return;

        other.gameObject.SetActive(false);

        StartCoroutine(TeleportAfterTime(other.gameObject, 3.0f));
    }

    IEnumerator TeleportAfterTime(GameObject ObjToTeleport, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        ObjToTeleport.SetActive(true);
        var agent = ObjToTeleport.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.Warp(Exit.transform.position);
        }

    }
}
