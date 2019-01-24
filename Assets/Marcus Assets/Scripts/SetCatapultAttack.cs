using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetCatapultAttack : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;


        if(other.GetComponentInChildren<CatapultSlimeAttack>())
        {
            other.GetComponent<SlimeBase>().anim.SetBool("IsMove", false);
            other.GetComponentInChildren<CatapultSlimeAttack>().m_CanAttack = true;
            other.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
