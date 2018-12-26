using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> m_SpawnObjectList; // list of object to be spawned
    public GameObject m_PathHolder; // gameobject that holds the path node

    public float m_Timer; // time that will increase by deltaTime
    public float m_TimeToSpawn; // when the time is reached it will spawn

    public int m_RandomSlime;
    
    public GameObject m_SlimeHolder;

	// Use this for initialization
	void Start () {
        m_Timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (SlimeManager.instance.m_SlimeInScene.Count < SlimeManager.instance.m_Waves[SlimeManager.instance.m_CurrentWave] && SlimeManager.instance.m_FinishSpawnWave == false)
            m_Timer += Time.deltaTime;

        else if (SlimeManager.instance.m_SlimeInScene.Count == SlimeManager.instance.m_Waves[SlimeManager.instance.m_CurrentWave] && SlimeManager.instance.m_FinishSpawnWave == false)
            SlimeManager.instance.m_FinishSpawnWave = true;

        if (m_Timer >= m_TimeToSpawn)
        {
            //Debug.Log(System.DateTime.Now.Millisecond);
            Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
            m_RandomSlime = Random.Range(0, m_SpawnObjectList.Count);
            GameObject Slime = Instantiate(m_SpawnObjectList[m_RandomSlime], this.transform.position, this.transform.rotation);
            m_Timer = 0.0f;

            foreach (Transform Node in m_PathHolder.transform)
            {
                Slime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
            }

            Slime.transform.SetParent(m_SlimeHolder.transform);
            SlimeManager.instance.m_SlimeInScene.Add(Slime);
        }
	}
}
