using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    public Animator anim;

    public float m_AttackTime;

	// Use this for initialization
	void Start () {
        m_AttackTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (anim.GetBool("IsAttack"))
        {
            m_AttackTime += Time.deltaTime;

            if (m_AttackTime > 2.0f && !anim.GetBool("IsAttacking"))
            {
                anim.SetBool("IsAttacking", true);
            }
        }
    }

    public void ResetAttack()
    {
        anim.SetBool("IsAttacking", false);
        m_AttackTime = 0.0f;
    }

    public void GolemAttackAudio() //play in event
    {
        AudioManager.Instance.Play3D("golemhit", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
    }
}
