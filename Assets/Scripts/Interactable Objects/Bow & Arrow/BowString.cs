using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : GrabbableObject
{

    [SerializeField]
    private ArrowBase m_BowArrowPrefab;

    [SerializeField]
    private Transform m_ArrowFacingPoint;

    private bool m_IsGrabbing = false;

    private Vector3 m_DefaultLocalPos;

    private Vector3 m_CurrentDrawDistance;

    private ArrowBase m_SpawnedArrow = null;

    private void Awake()
    {
        m_DefaultLocalPos = transform.localPosition;
    }
    
    public override void OnGrab(MoveController currentController)
    {
        m_SpawnedArrow = Instantiate(m_BowArrowPrefab.gameObject, transform).GetComponent<ArrowBase>();
        ResetArrowOrientation();
    }

    public override void OnGrabStay(MoveController currentController)
    {
        m_IsGrabbing = true;
        base.OnGrabStay(currentController);
        transform.position = currentController.transform.position;

        // Constantly face towards the pivot point when an arrow is drawn
        if (m_SpawnedArrow != null)
        {
            ResetArrowOrientation();
        }
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        m_IsGrabbing = false;

        float arrowStrength = m_CurrentDrawDistance.magnitude;

        if (m_SpawnedArrow != null)
        {
            // Launch the arrow, multiply the strength a bit?
            m_SpawnedArrow.LaunchArrow(arrowStrength * 50);
            m_SpawnedArrow = null;
        }
    }

    public override void OnControllerExit(MoveController currentController)
    {
        base.OnControllerExit(currentController);

        if (m_SpawnedArrow != null && m_SpawnedArrow.transform.parent != null)
        {
            m_SpawnedArrow.DestroyArrow();
        }
    }

    private void Update()
    {
        // If Player is not pulling the string, fling it back to the origin
        if (!m_IsGrabbing && transform.localPosition != m_DefaultLocalPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_DefaultLocalPos, Time.deltaTime * 10);

            // When the string has fully recovered
            if (transform.localPosition == m_DefaultLocalPos)
            {
                m_CurrentDrawDistance = Vector3.zero;
            }
        }
        else
        {
            m_CurrentDrawDistance = transform.localPosition - m_DefaultLocalPos;
        }
    }

    // Reset the spawned arrow to make it face the pivot
    private void ResetArrowOrientation()
    {
        m_SpawnedArrow.transform.localPosition = Vector3.zero;
        m_SpawnedArrow.transform.LookAt(m_ArrowFacingPoint.transform.position);
        m_SpawnedArrow.transform.Rotate(90, 0, 0);
    }

}
