using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Last minute trash codes
/// </summary>
public class FloorRespawner : MonoBehaviour
{

    [SerializeField]
    private GameObject m_BowSpawnPoint;

    private float m_CurrentStayTime = 0;

    private void OnTriggerStay(Collider other)
    {
        Bow bow = other.GetComponent<Bow>();

        if (bow != null)
        {
            if (bow.GetComponent<FixedJoint>() == null)
            {
                m_CurrentStayTime += Time.deltaTime;

                if (m_CurrentStayTime > 2)
                {
                    bow.transform.position = m_BowSpawnPoint.transform.position;
                    bow.transform.rotation = m_BowSpawnPoint.transform.rotation;
                    bow.GetComponent<Rigidbody>().useGravity = false;
                    bow.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            else
            {
                m_CurrentStayTime = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Bow bow = other.GetComponent<Bow>();

        if (bow != null)
        {
            if (bow.GetComponent<FixedJoint>() == null)
            {
                m_CurrentStayTime = 0;
            }
        }
    }

}
