using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeManager : MonoBehaviour {

    public List<GameObject> m_SlimeInScene;
    public int m_limit;

    public static SlimeManager instance = null;

    public List<int> m_Waves;
    
    //[HideInInspector]
    public int m_CurrentWave;

    [HideInInspector]
    public bool m_FinishSpawnWave;

	// Use this for initialization
	void Start () {
        m_SlimeInScene = new List<GameObject>();

        if (instance == null)
            instance = GetComponent<SlimeManager>();

        m_CurrentWave = 0;
        m_FinishSpawnWave = false;
    }

    public void Remove()
    {
        foreach(GameObject go in m_SlimeInScene)
        {
            if (go.GetComponent<SlimeBase>().toDespawn)
            {
                m_SlimeInScene.Remove(go);
                break;
            }
        }

        if(m_SlimeInScene.Count == 0 && m_FinishSpawnWave == true)
        {
            if (m_CurrentWave < m_Waves.Count - 1)
                m_CurrentWave++;

            m_FinishSpawnWave = false;
        }
    }
}
