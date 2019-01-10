using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultSlimeAttack : MonoBehaviour {

    public Animator anim;

    private float m_Time;
    public float m_DesiredTime;

	// Use this for initialization
	void Start () {
        m_Time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        //if(SlimeManager.instance.m_CurrentWave > 1)
            m_Time += Time.deltaTime;

        if (m_Time > m_DesiredTime)
        {
            anim.SetBool("IsAttack", true);
        }

	}

    public void SetToFalse()
    {
        anim.SetBool("IsAttack", false);
        m_Time = 0.0f;
    }

    public void PlayFire()
    {
        AudioManager.Instance.Play3D("catapultfire", transform.position, AudioManager.AudioType.Additive);
    }

    public void PlayTension()
    {
        AudioManager.Instance.Play3D("bowpull", transform.position, AudioManager.AudioType.Additive);
    }
}
