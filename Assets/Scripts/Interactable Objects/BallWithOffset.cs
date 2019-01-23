using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWithOffset : GrabbableObject
{

    [SerializeField]
    private Vector3 m_LocalPosWhenGrabbed;

    [SerializeField]
    private Vector3 m_RotWhenGrabbed;

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        transform.position = currentController.transform.TransformPoint(m_LocalPosWhenGrabbed);
        transform.rotation = currentController.transform.rotation;
        transform.Rotate(m_RotWhenGrabbed, Space.Self);
    }
    
}
