using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowDrawString : GrabbableObject
{

    private bool m_IsGrabbing = false;

    private Vector3 m_DefaultLocalPos;

    private void Awake()
    {
        m_DefaultLocalPos = transform.localPosition;
    }

    public override void OnGrab(MoveController currentController)
    {
    }

    public override void OnGrabStay(MoveController currentController)
    {
        m_IsGrabbing = true;
        base.OnGrabStay(currentController);
        transform.position = currentController.transform.position;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        m_IsGrabbing = false;
    }

    private void Update()
    {
        if (!m_IsGrabbing && transform.localPosition != m_DefaultLocalPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_DefaultLocalPos, Time.deltaTime * 10);
        }
    }

}
