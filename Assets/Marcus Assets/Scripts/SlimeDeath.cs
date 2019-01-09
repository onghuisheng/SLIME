using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeath : MonoBehaviour {

    public SlimeBase m_SlimeBase;
    public GameObject Slime;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveFromScene()
    {
        if (m_SlimeBase.m_DeathParticles != null)
        {
            AudioManager.Instance.Play3D("basicdeath", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D(){ randomPitchRange = 0.4f, volume = .5f});

            AudioManager.Instance.Play3D("slimesplatter", transform.position, AudioManager.AudioType.Additive, new AudioSourceData3D() { volume = .2f }, .1f);

            GameObject temp = Instantiate(m_SlimeBase.m_DeathParticles, Slime.transform.position, m_SlimeBase.m_DeathParticles.gameObject.transform.rotation);
            Destroy(temp, 3);
        }

        Destroy(Slime);
    }
}
