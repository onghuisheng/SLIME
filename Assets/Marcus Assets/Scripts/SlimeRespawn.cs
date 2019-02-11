using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRespawn : MonoBehaviour {

    public GameObject m_Slime;
    public GameObject m_SceneSlime;
    public List<GameObject> m_Waypoints;
    private float m_Delay;

	// Use this for initialization
	void Start () {
        m_Waypoints = m_SceneSlime.GetComponent<SlimePatrol>().m_WaypointList;
        m_Delay = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_SceneSlime == null)
        {
            m_Delay += Time.deltaTime;

            if (m_Delay > 2.0f)
            {
                m_SceneSlime = Instantiate(m_Slime, transform.position, transform.rotation);
                m_SceneSlime.GetComponent<SlimePatrol>().m_WaypointList = m_Waypoints;
                m_Delay = 0.0f;
            }
        }
	}
}
