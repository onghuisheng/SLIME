using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitLever : StationaryObject
{

    public Vector3 m_DefaultControllerPosition;
    public Quaternion m_DefaultLocalRotation;

    public GameObject m_;

    public override bool hideControllerOnGrab {
        get {
            return true;
        }
    }

    private void Awake()
    {
        m_DefaultLocalRotation = transform.localRotation;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_DefaultControllerPosition = currentController.transform.position;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        Vector3 toDefault = m_DefaultControllerPosition - transform.position;
        Vector3 toController = currentController.transform.position - transform.position;
        toController.Set(toDefault.x, toController.y, toDefault.z);

        float angle = Vector3.Angle(toDefault, toController);
        transform.localRotation = m_DefaultLocalRotation;
        transform.Rotate(0, 0, -angle, Space.Self);

        Debug.Log(angle);
    }

    private void Update()
    {
        
    }

}