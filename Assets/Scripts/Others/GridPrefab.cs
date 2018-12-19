using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this to grid prefab :o
public class GridPrefab : MonoBehaviour,IShootable {

    Collider m_collider;

    public void OnShot(ArrowBase arrow)
    {
        arrow.gameObject.transform.parent = null; // prevent arrow from despawning
        Destroy(gameObject);
       // throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
