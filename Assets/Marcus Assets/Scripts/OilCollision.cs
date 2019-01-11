using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OilCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Random.InitState((int)System.DateTime.Now.Ticks); // make random more random

        transform.localScale = new Vector3(Random.Range(1.0f, 3.0f), transform.localScale.y, Random.Range(1.0f, 3.0f));
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().speed /= 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().speed *= 10;
        }
    }

}
