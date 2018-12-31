using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ding ding
public class Bell : GrabbableObject
{

    [SerializeField, Header("Random items that might fall from ringing the bell")]
    private List<GameObject> m_ItemDropList;

    [SerializeField, Range(1, 60)]
    private int m_RingCooldown = 5;

    [SerializeField]
    private Transform m_ItemDropPosition;

    private Vector3 m_DefaultLocalPos, m_InitialOffset;

    private float m_NextSpawnTime = 0;

    private void Start()
    {
        m_DefaultLocalPos = transform.localPosition;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);

        m_InitialOffset = currentController.transform.position - transform.position;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        transform.position = currentController.transform.position - m_InitialOffset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        if (Time.time < m_NextSpawnTime || m_ItemDropList.Count == 0)
            return;

        // Spawn a random object from the list with a randomized rotation
        Instantiate(m_ItemDropList[Random.Range(0, m_ItemDropList.Count)], m_ItemDropPosition.position, Quaternion.Euler(Random.Range(0, 360.0f), Random.Range(0, 360.0f), Random.Range(0, 360.0f)));

        m_NextSpawnTime = Time.time + m_RingCooldown;
    }

}