using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRespawn : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> m_ItemsList;

    private List<Vector3> m_ItemsPos = new List<Vector3>();

    // Use this for initialization
    void Start()
    {
        foreach (GameObject GO in m_ItemsList)
        {
            m_ItemsPos.Add(GO.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int i = 0;

        foreach (GameObject GO in m_ItemsList)
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
                StartCoroutine(Respawn(GO, m_ItemsPos[i], 0.5f));
            }
            i++; //loop counter
        }
    }

    private IEnumerator Respawn(GameObject target, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        //set item pos back to original position
        target.transform.position = pos;
    }

}
