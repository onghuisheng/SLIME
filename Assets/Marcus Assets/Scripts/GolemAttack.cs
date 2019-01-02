using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    public Animator anim;

    private float m_AttackTime;

	// Use this for initialization
	void Start () {
        m_AttackTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (anim.GetBool("IsAttack"))
        {
            m_AttackTime += Time.deltaTime;

            if (m_AttackTime > 3.0f && m_AttackTime < 5.0f && !anim.GetBool("IsAttacking"))
            {
                anim.SetBool("IsAttacking", true);
            }

            else if (m_AttackTime > 5.0f && anim.GetBool("IsAttacking"))
            {
                anim.SetBool("IsAttacking", false);
                m_AttackTime = 0.0f;
            }
        }
    }
}
