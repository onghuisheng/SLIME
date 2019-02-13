using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AttackPlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag != "Enemy")
                return;

            other.GetComponentInChildren<Animator>().SetBool("IsAttack", true);
            other.GetComponentInChildren<Animator>().SetBool("IsDefending", false);
            other.transform.DOLookAt(new Vector3(transform.position.x, other.transform.position.y, transform.position.z), 1, AxisConstraint.Y);
            other.GetComponent<NavMeshAgent>().enabled = false;

            var golemAttack = other.GetComponentInChildren<GolemAttack>();

            if (golemAttack != null)
                golemAttack.m_AttackPlayer = true;
        }
    }

}
