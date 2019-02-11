using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRespawn : MonoBehaviour {

    public GameObject m_Slime;
    public GameObject m_SceneSlime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_SceneSlime == null)
        {
            m_SceneSlime = Instantiate(m_Slime, transform.position, transform.rotation);
        }
	}
}
