using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellString : GrabbableObject
{
    [SerializeField]
    private Transform m_StringPivotPoint;

    private Vector3 m_DefaultStringPosition, m_InitialOffset;

    private const float m_MaxSwingAngle = 55;

    private void Start()
    {
        m_DefaultStringPosition = transform.GetChild(0).position;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_InitialOffset = currentController.transform.position - transform.position;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        // GetComponent<Rigidbody>().position = currentController.transform.position - m_InitialOffset;
        // transform.position = currentController.transform.position - m_InitialOffset;

        if (GetAngleBetweenHandle() > m_MaxSwingAngle)
        {
            currentController.DetachCurrentObject(false);
            var rb = GetComponent<Rigidbody>();

            float adjustedVelocity = 3;

            rb.velocity = rb.velocity.normalized * adjustedVelocity;
            rb.angularVelocity = rb.angularVelocity.normalized * adjustedVelocity;
        }
    }

    private void LateUpdate()
    {
        if (GetAngleBetweenHandle() >= m_MaxSwingAngle)
        {
            var rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private float GetAngleBetweenHandle()
    {
        return Vector3.Angle(m_DefaultStringPosition - m_StringPivotPoint.position, transform.GetChild(0).position - m_StringPivotPoint.position);
    }

}
