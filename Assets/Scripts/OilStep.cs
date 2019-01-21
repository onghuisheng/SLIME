using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilStep : MonoBehaviour {

    public GameObject m_OilStepParticle;
    //public ParticleSystem m_OilDripParticle;

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
            Destroy(temp, 5.0f);
        }
    }
}
