using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultSlimeAttack : MonoBehaviour {

    public Animator anim;

    private float m_Time;
    public float m_DesiredTime;

    [HideInInspector]
    public bool m_CanAttack;

	// Use this for initialization
	void Start () {
        m_Time = 0.0f;
        m_CanAttack = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(m_CanAttack)
            m_Time += Time.deltaTime;

        if (m_Time > m_DesiredTime && m_CanAttack)
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
        AudioManager.Instance.Play3D("catapultfire", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = 0,volume = .7f });
    }

    public void PlayTension()
    {
        AudioManager.Instance.Play3D("bowpull", transform.position, AudioManager.AudioType.Additive);
    }
}
