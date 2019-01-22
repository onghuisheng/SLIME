using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Despawner : MonoBehaviour {
    
    public GameObject m_Canvas;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag != "Enemy")
                return;

            // Normal Slime
            if (other.GetComponent<GolemSlimeDefend>() == null)
            {
                other.GetComponent<SlimeBase>().toDespawn = true;
                SlimeManager.instance.GetComponent<SlimeManager>().Remove();

                other.GetComponent<SlimeBase>().anim.SetBool("IsDead", true);

                other.GetComponentInChildren<SlimeDeath>().m_Canvas = m_Canvas;
                other.GetComponentInChildren<SlimeDeath>().m_Despawn = true;
            }

            else
            {
                if (other.GetComponent<Movement>().m_Player == null)
                {
                    other.GetComponentInChildren<Animator>().SetBool("IsAttack", true);
                    other.GetComponentInChildren<Animator>().SetBool("IsDefending", false);
                    other.transform.DOLookAt(new Vector3(transform.position.x, other.transform.position.y, transform.position.z), 1, AxisConstraint.Y);
                }
            }
        }
        other.GetComponent<NavMeshAgent>().enabled = false;

    }
    
    
}
