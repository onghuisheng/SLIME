using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> goSpawnObjectList; // list of object to be spawned
    public GameObject goNodeHolder; // gameobject that holds the path node

    public List<GameObject> listObjectInScene; // list that holds the objects spawned

    public float fTimer; // time that will increase by deltaTime
    public float fTimeToSpawn; // when the time is reached it will spawn
    public int iLimit; // limit for how many slimes the user want in the scene

    public int iRandomSlime;

	// Use this for initialization
	void Start () {
        fTimer = 0.0f;
        listObjectInScene = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if(listObjectInScene.Count < iLimit)
            fTimer += Time.deltaTime;

        if(fTimer >= fTimeToSpawn)
        {
            Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
            iRandomSlime = Random.Range(0, goSpawnObjectList.Count - 1);
            GameObject Slime = Instantiate(goSpawnObjectList[iRandomSlime], this.transform.position, this.transform.rotation);
            fTimer = 0.0f;

            foreach(Transform Node in goNodeHolder.transform)
            {
                Slime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
            }

            listObjectInScene.Add(Slime);
        }
	}
}
