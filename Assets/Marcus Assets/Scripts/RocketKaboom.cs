using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketKaboom : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        //math 
        float nextTime = 1 / GetComponent<ParticleSystem>().emission.rateOverTime.constant;
        float waitTime = GetComponent<ParticleSystem>().main.startLifetime.constant * nextTime;

        if (waitTime > GetComponent<ParticleSystem>().main.startLifetime.constant)
            waitTime = GetComponent<ParticleSystem>().main.startLifetime.constant;

        StartCoroutine(PlaySound(waitTime, nextTime));
    }

    // Update is called once per frame
    void Update () {
    }

    IEnumerator PlaySound(float m_WaitTime, float nextTime)
    {
        WaitForSeconds m_Time = new WaitForSeconds(m_WaitTime);
        WaitForSeconds m_Next = new WaitForSeconds(nextTime);

        float temp = Mathf.Abs(nextTime - m_WaitTime);
        WaitForSeconds m_NewNext = new WaitForSeconds(temp);

        yield return m_Next;

        while (true)
        {
            AudioManager.Instance.Play3D("whee", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
            yield return m_Time;
            AudioManager.Instance.Play3D("dynexp", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { randomPitchRange = .2f });
            yield return m_NewNext;
        }
    }
}
