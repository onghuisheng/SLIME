using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRespawn : MonoBehaviour {

    [SerializeField]
    private List<GameObject> m_ItemsList;

    private List<Vector3> m_ItemsPos = new List<Vector3>();

    // Use this for initialization
    void Start () {
		foreach(GameObject GO in m_ItemsList)
        {
            m_ItemsPos.Add(GO.transform.position);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        int i = 0;

        foreach(GameObject GO in m_ItemsList)
        {
            if (GO == other.gameObject)
            {
                var controller = MoveController.GetControllerThatHolds(GO);
                if (controller != null)
                {
                    //detach, without transfering velocity 
                    controller.DetachCurrentObject(false);
                }
                //set item pos back to original position
                GO.transform.position = m_ItemsPos[i];
            }
            i++; //loop counter
        }
    }
}
