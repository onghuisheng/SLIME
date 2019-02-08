using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    public Animator anim;
    public bool m_Attack;

    public bool m_AttackPlayer;
    
	// Use this for initialization
	void Start () {
        m_Attack = false;
        m_AttackPlayer = false;
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    public void DoAttack()
    {
        anim.SetBool("IsAttacking", true);
        m_Attack = true;
    }

    public void ResetAttack()
    {
        anim.SetBool("IsAttacking", false);
        m_Attack = false;
    }

    public void GolemAttackAudio() //play in event
    {
        if (m_AttackPlayer == false)
            AudioManager.Instance.Play3D("golemhit", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
        else
            AudioManager.Instance.Play3D("SSSSergh", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });

    }
}
