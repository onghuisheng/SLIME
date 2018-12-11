using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> goSpawnObjectList; // list of object to be spawned
    public GameObject goNodeHolder; // gameobject that holds the path node

    public float fTimer; // time that will increase by deltaTime
    public float fTimeToSpawn; // when the time is reached it will spawn

    public int iRandomSlime;

    public GameObject m_SlimeManager;

	// Use this for initialization
	void Start () {
        fTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if(m_SlimeManager.GetComponent<SlimeManager>().m_SlimeInScene.Count < m_SlimeManager.GetComponent<SlimeManager>().m_limit)
            fTimer += Time.deltaTime;

        if (fTimer >= fTimeToSpawn)
        {
            //Debug.Log(System.DateTime.Now.Millisecond);
            Random.InitState((int)System.DateTime.Now.Ticks); // make random more random
            iRandomSlime = Random.Range(0, goSpawnObjectList.Count);
            GameObject Slime = Instantiate(goSpawnObjectList[iRandomSlime], this.transform.position, this.transform.rotation);
            fTimer = 0.0f;

            foreach (Transform Node in goNodeHolder.transform)
            {
                Slime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
            }

            m_SlimeManager.GetComponent<SlimeManager>().m_SlimeInScene.Add(Slime);
        }

        //for (int i = 0; i < listObjectInScene.Count; i++)
        //{
        //    if (!listObjectInScene[i].activeInHierarchy || listObjectInScene == null)
        //    {
        //        listObjectInScene.RemoveAt(i);
        //        GameObject.Destroy(listObjectInScene[i]);
        //        break;
        //    }
        //}
	}
}
