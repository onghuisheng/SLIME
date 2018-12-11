using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour {

    public List<GameObject> m_SlimeInScene;
    public int m_limit;

	// Use this for initialization
	void Start () {
        m_SlimeInScene = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Remove()
    {
        foreach(GameObject go in m_SlimeInScene)
        {
            if (!go.activeInHierarchy)
            {
                m_SlimeInScene.Remove(go);
                GameObject.Destroy(go);
                break;
            }
        }
    }
}
