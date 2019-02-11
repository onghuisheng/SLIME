using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRespawn : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> m_ItemsList;

    [SerializeField, Range(0.0f, 5.0f)]
    private float m_RespawnDelay = 0;

    [SerializeField]
    private bool m_UseOnCollisionEnter = false;

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
        if (m_UseOnCollisionEnter)
            return;

        RespawnObject(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_UseOnCollisionEnter)
        {
            RespawnObject(collision.gameObject);
        }
    }

    private void RespawnObject(GameObject obj)
    {
        int i = 0;

        foreach (GameObject GO in m_ItemsList)
        {
            if (GO == obj)
            {
                var controller = MoveController.GetControllerThatHolds(GO);
                if (controller != null)
                {
                    //detach, without transfering velocity 
                    controller.DetachCurrentObject(false);
                }
                //set item pos back to original position
                StartCoroutine(Respawn(GO, m_ItemsPos[i], ((GO.tag == "Grenade") ? 3 : 0.1f)));
            }
            i++; //loop counter
        }
    }

    private IEnumerator Respawn(GameObject target, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(m_RespawnDelay + delay);
        //set item pos back to original position
        target.transform.position = pos;
    }

}
