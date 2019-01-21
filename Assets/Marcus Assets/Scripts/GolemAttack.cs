using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : MonoBehaviour {

    public Animator anim;
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    public void DoAttack()
    {
        anim.SetBool("IsAttacking", true);
    }

    public void ResetAttack()
    {
        anim.SetBool("IsAttacking", false);
    }

    public void GolemAttackAudio() //play in event
    {
        AudioManager.Instance.Play3D("golemhit", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
    }
}
