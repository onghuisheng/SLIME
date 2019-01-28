using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellString : GrabbableObject
{
    [SerializeField]
    private Transform m_StringPivotPoint;

    [SerializeField]
    private GameObject m_Chute;

    private Vector3 m_DefaultStringPosition;

    private bool m_IsGrabbing;

    private const float m_MaxSwingAngle = 55;

    private Collider m_StringCollider;

    private void Start()
    {
        m_StringCollider = m_StringPivotPoint.GetComponent<Collider>();
        m_DefaultStringPosition = transform.GetChild(0).position;
        Physics.IgnoreCollision(GetComponent<Collider>(), m_StringPivotPoint.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(GetComponent<Collider>(), m_Chute.GetComponent<Collider>(), true);
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_IsGrabbing = true;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        if (GetAngleBetweenHandle() > m_MaxSwingAngle)
        {
            currentController.DetachCurrentObject(false);
        }
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_IsGrabbing = false;
    }

    private float GetAngleBetweenHandle()
    {
        return Vector3.Angle(m_DefaultStringPosition - m_StringPivotPoint.position, transform.GetChild(0).position - m_StringPivotPoint.position);
    }

    private void Update()
    {
        if (GetAngleBetweenHandle() > m_MaxSwingAngle)
        {
            m_StringCollider.enabled = false;
        }
        else if (!m_StringCollider.enabled)
        {
            m_StringCollider.enabled = true;
        }
    }

}
