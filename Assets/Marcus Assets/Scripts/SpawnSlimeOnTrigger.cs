using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlimeOnTrigger : MonoBehaviour {

    public GameObject m_PathHolder; // gameobject that holds the path node
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.GetComponent<SpawnSlimeAfterShot>().enabled == false || other.tag != "SlimeProjectile")
            return;

        GameObject tempSlime;

        switch (other.GetComponent<SpawnSlimeAfterShot>().Color)
        {
            case SpawnSlimeAfterShot.SlimeColor.Blue:
                {
                    tempSlime = Instantiate(other.GetComponent<SpawnSlimeAfterShot>().BlueSlime, transform.position, Quaternion.identity);
                    foreach (Transform Node in m_PathHolder.transform)
                    {
                        tempSlime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                    }
                    break;
                }

            case SpawnSlimeAfterShot.SlimeColor.Red:
                {
                    tempSlime = Instantiate(other.GetComponent<SpawnSlimeAfterShot>().RedSlime, transform.position, Quaternion.identity);
                    foreach (Transform Node in m_PathHolder.transform)
                    {
                        tempSlime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                    }
                    break;
                }

            case SpawnSlimeAfterShot.SlimeColor.Pink:
                {
                    tempSlime = Instantiate(other.GetComponent<SpawnSlimeAfterShot>().PinkSlime, transform.position, Quaternion.identity);
                    foreach (Transform Node in m_PathHolder.transform)
                    {
                        tempSlime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                    }
                    break;
                }

            case SpawnSlimeAfterShot.SlimeColor.Yellow:
                {
                    tempSlime = Instantiate(other.GetComponent<SpawnSlimeAfterShot>().YellowSlime, transform.position, Quaternion.identity);
                    foreach (Transform Node in m_PathHolder.transform)
                    {
                        tempSlime.GetComponent<Movement>().m_pathList.Add(Node.gameObject);
                    }
                    break;
                }

            default:
                break;
        }


        Destroy(other.gameObject);
    }
}
