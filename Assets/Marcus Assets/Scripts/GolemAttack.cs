using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    public Animator anim;
    public bool m_Attack;
    
	// Use this for initialization
	void Start () {
        m_Attack = false;

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
        AudioManager.Instance.Play3D("golemhit", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
    }
}
