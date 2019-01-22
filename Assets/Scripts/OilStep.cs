using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilStep : MonoBehaviour {

    public GameObject m_OilStepParticle;
    public GameObject m_OilDripParticle;
    public GameObject m_SpawnParticlePos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayOilStep(Vector3 m_Position)
    {
        Debug.Log("PlayOilStep()");

        if (m_OilStepParticle != null) //if there's particles, play it & audio
        {
            AudioManager.Instance.Play3D("slimesplatter", m_Position, AudioManager.AudioType.Additive);

            GameObject temp = Instantiate(m_OilStepParticle, m_Position, m_OilStepParticle.transform.rotation);

            //Destroy temp after 5 sec
            Destroy(temp, 3.0f);
        }
    }

    public void PlayOilDrip()
    {
        if (m_OilDripParticle != null) //if there's particles, play it
        {
            GameObject temp = Instantiate(m_OilDripParticle, m_SpawnParticlePos.transform.position, m_OilDripParticle.transform.rotation);

            //Destroy temp after 5 sec
            Destroy(temp, 1.0f);
        }
    }
}
