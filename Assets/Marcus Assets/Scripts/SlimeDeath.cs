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
            GameObject temp = Instantiate(m_SlimeBase.m_DeathParticles, Slime.transform.position, m_SlimeBase.m_DeathParticles.gameObject.transform.rotation);
            Destroy(temp, 3);
        }

        Destroy(Slime);
    }
}
