using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this to grid prefab :o
public class GridPrefab : MonoBehaviour,IShootable {

    Collider m_collider;
    float timerFog = 10;

    public void OnShot(ArrowBase arrow)
    {
        arrow.gameObject.transform.parent = null; // prevent arrow from despawning
        //gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false); //set child (fog particle) inactive 
        //Destroy(gameObject);
       // throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            timerFog -= Time.deltaTime;
            if (timerFog < 0)
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }
}
