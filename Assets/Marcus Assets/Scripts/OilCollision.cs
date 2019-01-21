using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OilCollision : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
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

        if(other.transform.GetComponent<OilStep>() != null)
        {
            other.transform.GetComponent<OilStep>().PlayOilStep(other.transform.position);
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
