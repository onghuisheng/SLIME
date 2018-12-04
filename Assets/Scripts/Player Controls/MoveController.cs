using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PS4.VR;
using UnityEngine.PS4;


public enum MoveControllerOrientation
{
    Left,
    Right
}

public class MoveController : MonoBehaviour
{

    [SerializeField]
    private GameObject m_HandModel;

    private MoveControllerOrientation m_Orientation;    

    private Animator m_HandAnimator;

    private int m_HandleNumber = -1;

    private bool m_IsRegistered = false;

    void Start()
    {

    }

    void Update()
    {
        if (m_IsRegistered)
        {
            Vector3 pos;
            if (Tracker.GetTrackedDevicePosition(m_HandleNumber, out pos) == PlayStationVRResult.Ok)
            {
                transform.position = pos;
            }

            Quaternion rot;
            if (Tracker.GetTrackedDeviceOrientation(m_HandleNumber, out rot) == PlayStationVRResult.Ok)
            {
                transform.rotation = rot;
            }
            
            //TODO: Controller get key down
        }
    }

    public void RegisterController(MoveControllerOrientation orientation, int handleNumber)
    {
        m_IsRegistered = true;
        m_HandleNumber = handleNumber;
        m_Orientation = orientation;
        Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceMove, handleNumber, PlayStationVRTrackingType.Absolute, PlayStationVRTrackerUsage.Default);
    }

}
