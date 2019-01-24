using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitLever : StationaryObject
{

    Vector3? m_DefaultControllerPosition = null;
    Quaternion m_DefaultLocalRotation;

    bool m_IsEnabled = true;

    public override bool hideControllerOnGrab { get { return true; } }

    private void Awake()
    {
        m_DefaultLocalRotation = transform.localRotation;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);

        if (m_DefaultControllerPosition == null)
            m_DefaultControllerPosition = currentController.transform.position;
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);

        if (m_IsEnabled)
        {
            Vector3 toDefault = m_DefaultControllerPosition.Value - transform.position;
            Vector3 toController = currentController.transform.position - transform.position;
            toController.Set(toDefault.x, toController.y, toDefault.z);

            float angle = Vector3.Angle(toDefault, toController);
            transform.localRotation = m_DefaultLocalRotation;
            transform.Rotate(0, 0, -angle, Space.Self);

            if (angle >= 50)
            {
                m_IsEnabled = false;
                AudioManager.Instance.Play3D("leverclick", transform.position, AudioManager.AudioType.Additive, 0, () =>
                {
                    // DO scene change here 
                });
            }

            Debug.Log(angle);
        }
    }

}