using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.PS4.VR;

public class PlayerHandController : MonoBehaviour {

    private TrackedPoseDriver m_TrackedPoseDriver;


	// Use this for initialization
	void Start () {
        m_TrackedPoseDriver = GetComponent<TrackedPoseDriver>();

        Vector3 pos;

        var gg = Tracker.GetTrackedDevicePosition(0, out pos);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = m_TrackedPoseDriver.originPose.position;
	}

}
