using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictory : MonoBehaviour {

    public GameObject m_FireWorks;
    public bool m_Spawn;

	// Use this for initialization
	void Start () {
        m_Spawn = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (SlimeManager.instance.m_CurrentWave == 2 && SlimeManager.m_GameType == SlimeManager.GameType.Normal && m_Spawn == false)
        {
            if (SlimeManager.instance.m_SlimeInScene.Count == 0 && SlimeManager.instance.m_FinishSpawnWave == true && SlimeManager.instance.m_SlimeInWave.Count == SlimeManager.instance.m_SlimeWaves[SlimeManager.instance.m_CurrentWave] && SlimeManager.instance.m_GolemSlimeInWave.Count == SlimeManager.instance.m_GolemSlimeWaves[SlimeManager.instance.m_CurrentWave])
            {
                SlimeManager.instance.m_GameFinish = true;
                m_Spawn = true;
                Instantiate(m_FireWorks, m_FireWorks.transform.position, m_FireWorks.transform.rotation);
            }
        }

	}
}
