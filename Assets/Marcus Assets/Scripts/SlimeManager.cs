using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeManager : MonoBehaviour {

    public List<GameObject> m_SlimeInScene;
    public int m_limit;

    public static SlimeManager instance = null;

	// Use this for initialization
	void Start () {
        m_SlimeInScene = new List<GameObject>();

        if (instance == null)
            instance = GetComponent<SlimeManager>();
	}

    public void Remove()
    {
        foreach(GameObject go in m_SlimeInScene)
        {
            if (go.GetComponent<SlimeBase>().toDespawn)
            {
                m_SlimeInScene.Remove(go);
                go.GetComponent<SlimeBase>().anim.SetBool("IsDead", true);
                go.GetComponent<NavMeshAgent>().enabled = false;
                break;
            }
        }
    }
}
