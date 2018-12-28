using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> m_SpawnObjectList; // list of object to be spawned
    public GameObject GolemSlime; // for specifically spawning golem slime

    public GameObject m_PathHolder; // gameobject that holds the path node

    public float m_Timer; // time that will increase by deltaTime
    public float m_TimeToSpawn; // when the time is reached it will spawn

    public int m_RandomSlime;
    
    public GameObject m_SlimeHolder;

    public GameObject m_DayAndNight;

	// Use this for initialization
	void Start () {
        m_Timer = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {
        if (SlimeManager.instance.m_FinishSpawnWave == false
            && !(m_DayAndNight.GetComponent<DayAndNight>().dot > 0.0f && m_DayAndNight.GetComponent<DayAndNight>().dot < 0.5f))
            m_Timer += Time.deltaTime;

        else if (SlimeManager.instance.m_SlimeInWave.Count == SlimeManager.instance.m_SlimeWaves[SlimeManager.instance.m_CurrentWave]
            && SlimeManager.instance.m_GolemSlimeInWave.Count == SlimeManager.instance.m_GolemSlimeWaves[SlimeManager.instance.m_CurrentWave]
            && SlimeManager.instance.m_FinishSpawnWave == false)
            SlimeManager.instance.m_FinishSpawnWave = true;

        if (m_Timer >= m_TimeToSpawn)
        {
            //Debug.Log(System.DateTime.Now.Millisecond);
            Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
            int GolemOrNot = Random.Range(0, 1); // random to decide spawn golem or normal slime

            // if golem already spawn then auto set to normal
            if(SlimeManager.instance.m_GolemSlimeInWave.Count == SlimeManager.instance.m_GolemSlimeWaves[SlimeManager.instance.m_CurrentWave] 
                && SlimeManager.instance.m_SlimeInWave.Count < SlimeManager.instance.m_SlimeWaves[SlimeManager.instance.m_CurrentWave])
            {
                GolemOrNot = 0;
            }
            // if golem not spawned and normal all spawn then set to spawn golem
            else if (SlimeManager.instance.m_GolemSlimeInWave.Count < SlimeManager.instance.m_GolemSlimeWaves[SlimeManager.instance.m_CurrentWave]
                && SlimeManager.instance.m_SlimeInWave.Count == SlimeManager.instance.m_SlimeWaves[SlimeManager.instance.m_CurrentWave])
            {
                GolemOrNot = 1;
            }


            if (GolemOrNot == 0) // dont spawn golem
            {
                if (SlimeManager.instance.m_SlimeInWave.Count < SlimeManager.instance.m_SlimeWaves[SlimeManager.instance.m_CurrentWave])
                {
                    //Debug.Log(System.DateTime.Now.Millisecond);
                    Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
                    m_RandomSlime = Random.Range(0, m_SpawnObjectList.Count);
                    GameObject Slime = Instantiate(m_SpawnObjectList[m_RandomSlime], this.transform.position, this.transform.rotation);
                    foreach (Transform Node in m_PathHolder.transform)
                    {
                        Slime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                    }

                    Slime.transform.SetParent(m_SlimeHolder.transform);
                    SlimeManager.instance.m_SlimeInScene.Add(Slime);
                    SlimeManager.instance.m_SlimeInWave.Add(Slime);

                    m_Timer = 0.0f;
                }
            }
            else // spawn golem
            {
                if (GolemSlime)
                {
                    if (SlimeManager.instance.m_GolemSlimeInWave.Count < SlimeManager.instance.m_GolemSlimeWaves[SlimeManager.instance.m_CurrentWave])
                    {
                        GameObject Slime = Instantiate(GolemSlime, this.transform.position, this.transform.rotation);
                        foreach (Transform Node in m_PathHolder.transform)
                        {
                            Slime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                        }

                        Slime.transform.SetParent(m_SlimeHolder.transform);
                        SlimeManager.instance.m_SlimeInScene.Add(Slime);
                        SlimeManager.instance.m_GolemSlimeInWave.Add(Slime);

                        m_Timer = 0.0f;
                    }
                }
            }
        }
    }
}
