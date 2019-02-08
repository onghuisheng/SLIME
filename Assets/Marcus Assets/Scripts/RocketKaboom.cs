using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketKaboom : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        StartCoroutine(PlaySound(GetComponent<ParticleSystem>().main.startLifetime.constant));
    }

    // Update is called once per frame
    void Update () {
    }

    IEnumerator PlaySound(float m_WaitTime)
    {
        WaitForSeconds m_Time = new WaitForSeconds(m_WaitTime);

        while(true)
        {
            yield return m_Time;
            AudioManager.Instance.Play3D("npc_goodjob", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });

        }
    }
}
