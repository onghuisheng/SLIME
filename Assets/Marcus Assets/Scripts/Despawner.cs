using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Despawner : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag != "Enemy")
                return;

            if (other.GetComponent<GolemSlimeDefend>() == null)
            {
                other.GetComponent<SlimeBase>().toDespawn = true;
                SlimeManager.instance.GetComponent<SlimeManager>().Remove();

                other.GetComponent<SlimeBase>().anim.SetBool("IsDead", true);

                if (other.GetComponent<GolemSlimeDefend>() != null)
                {
                    other.GetComponent<SlimeBase>().anim.SetBool("IsDefending", false);
                }

                other.GetComponent<NavMeshAgent>().enabled = false;
            }

            else
            {
                other.GetComponentInChildren<Animator>().SetBool("IsAttack", true);
                other.GetComponentInChildren<Animator>().SetBool("IsDefend", false);
                other.transform.forward = transform.forward;
            }
        }
    }
}
