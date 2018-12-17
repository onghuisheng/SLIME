using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (go.GetComponent<SlimeBase>().m_DeathParticles != null)
                {
                    GameObject temp = Instantiate(go.GetComponent<SlimeBase>().m_DeathParticles, go.transform.position, go.transform.rotation);
                    GameObject.Destroy(temp, 3);
                }
                GameObject.Destroy(go);
                break;
            }
        }
    }
}
