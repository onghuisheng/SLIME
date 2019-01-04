using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Despawner : MonoBehaviour {

    public GameObject m_LookAtPoint;

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

            }

            else
            {
                other.GetComponentInChildren<Animator>().SetBool("IsAttack", true);
                other.GetComponentInChildren<Animator>().SetBool("IsDefending", false);
                other.transform.DOLookAt(new Vector3(transform.position.x, other.transform.position.y, transform.position.z), 1, AxisConstraint.Y);
            }
        }

        
        other.GetComponent<NavMeshAgent>().enabled = false;

    }
}
