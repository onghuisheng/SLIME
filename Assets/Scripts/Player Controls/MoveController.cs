using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PS4.VR;
using UnityEngine.PS4;
using System;

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
    public MoveControllerOrientation orientation {
        get { return m_Orientation; }
        protected set { m_Orientation = value; }
    }

    private Animator m_HandAnimator;

    private Dictionary<MoveControllerButton, bool> m_LastButtonStates = new Dictionary<MoveControllerButton, bool>();

    private int m_HandleNumber = -1;

    private int m_ControllerIndex = -1;

    private bool m_IsRegistered = false;

    private static GameObject m_CurrentLeftObject = null;
    private static GameObject m_CurrentRightObject = null;

    private GameObject m_Player;

    private void Awake()
    {
        m_CurrentLeftObject = null;
        m_CurrentRightObject = null;
        m_Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (m_IsRegistered)
        {
            ProcessGrabbing();
            ProcessControllerMovement();
        }
    }

    private void LateUpdate()
    {
        foreach (var key in Enum.GetValues(typeof(MoveControllerButton)))
        {
            MoveControllerButton button = (MoveControllerButton)key;
            m_LastButtonStates[button] = GetButton(button);
        }
    }

    public void RegisterController(MoveControllerOrientation controllerOrientation, int handleNumber)
    {
        m_IsRegistered = true;
        m_HandleNumber = handleNumber;
        m_ControllerIndex = (controllerOrientation == MoveControllerOrientation.Left) ? 0 : 1;
        m_Orientation = controllerOrientation;

        Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceMove, handleNumber, PlayStationVRTrackingType.Absolute, PlayStationVRTrackerUsage.OptimizedForHmdUser);

        if (controllerOrientation == MoveControllerOrientation.Left)
            PS4Input.MoveSetLightSphere(0, 0, 0, 0, 255);
        else
            PS4Input.MoveSetLightSphere(0, 1, 255, 0, 0);
    }

    private void ProcessControllerMovement()
    {
        Vector3 pos;
        if (Tracker.GetTrackedDevicePosition(m_HandleNumber, PlayStationVRSpace.Unity, out pos) != PlayStationVRResult.Error)
        {
            transform.position = pos + m_Player.transform.localPosition;
        }

        Quaternion rot;
        if (Tracker.GetTrackedDeviceOrientation(m_HandleNumber, PlayStationVRSpace.Unity, out rot) != PlayStationVRResult.Error)
        {
            transform.rotation = rot;
        }
    }

    private void ProcessGrabbing()
    {
        var currentObject = GetCurrentHandObject();

        if (currentObject == null)
            return;

        var iObject = currentObject.GetComponent<IInteractable>();

        if (iObject == null)
            return;

        iObject.OnControllerStay(this);

        var grabbableObject = currentObject.GetComponent<IGrabbable>();

        if (grabbableObject == null)
            return;

        // On Grab (Trigger Down)
        if (GetButtonDown(MoveControllerHotkeys.buttonGrab))
        {
            if (grabbableObject.hideControllerOnGrab)
                m_HandModel.SetActive(false);

            grabbableObject.OnGrab(this);

            AssignObjectToHand(m_Orientation, currentObject.gameObject);

            if (currentObject.GetComponent<IStationaryGrabbable>() == null)
            {
                currentObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                Rigidbody rb = GetComponent<Rigidbody>();
                FixedJoint joint = currentObject.AddComponent<FixedJoint>();
                joint.connectedBody = rb;
                joint.breakForce = 7500;
                joint.breakTorque = Mathf.Infinity;
                joint.enablePreprocessing = false;
            }
        }

        // On Item First Use
        if (GetButtonDown(MoveControllerHotkeys.buttonUse))
        {
            iObject.OnUse(this);
        }

        // On Item Holding Down Use 
        if (GetButton(MoveControllerHotkeys.buttonUse))
        {
            iObject.OnUseDown(this);
        }

        // On Item Use Released
        if (GetButtonUp(MoveControllerHotkeys.buttonUse))
        {
            iObject.OnUseUp(this);
        }

        // On Grab Use
        if (GetButton(MoveControllerHotkeys.buttonGrab))
        {
            grabbableObject.OnGrabStay(this);
        }
        // On Grab Released (Trigger Up)
        if (GetButtonUp(MoveControllerHotkeys.buttonGrab))
        {
            DetachCurrentObject(true);
        }

    }

    protected void OnTriggerEnter(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (GetCurrentHandObject() == null && iObject != null && GetCurrentHandObject(true) != other.gameObject)
        {
            AssignObjectToHand(m_Orientation, other.gameObject);
            iObject.OnControllerEnter(this);
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (GetCurrentHandObject() == null && iObject != null && GetCurrentHandObject() != other.gameObject && GetCurrentHandObject(true) != other.gameObject)
        {
            AssignObjectToHand(m_Orientation, other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            if (other.gameObject == GetCurrentHandObject())
            {
                iObject.OnControllerExit(this);
                DetachCurrentObject(false);
            }
        }
    }

    /// <summary>
    /// Detaches the object that this controller is currently holding
    /// </summary>
    /// <param name="transferVelocity">Transfer the current velocity of the controller to the object?</param>
    public void DetachCurrentObject(bool transferVelocity)
    {
        var currentObject = GetCurrentHandObject();

        if (currentObject != null)
        {
            var grabbableObject = currentObject.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                if (grabbableObject.hideControllerOnGrab)
                    m_HandModel.SetActive(true);

                grabbableObject.OnGrabReleased(this);

                AssignObjectToHand(m_Orientation, null);

                if (currentObject.GetComponent<FixedJoint>() != null)
                    Destroy(currentObject.GetComponent<FixedJoint>());

                if (currentObject.GetComponent<IStationaryGrabbable>() == null && transferVelocity)
                {
                    Rigidbody rb = currentObject.GetComponent<Rigidbody>();
                    rb.velocity = GetVelocity();
                    rb.angularVelocity = GetAngularVelocity();
                }
            }
        }
    }

    private void AssignObjectToHand(MoveControllerOrientation hand, GameObject go)
    {
        if (hand == MoveControllerOrientation.Left)
            m_CurrentLeftObject = go;
        else
            m_CurrentRightObject = go;
    }

    public GameObject GetCurrentHandObject(bool otherHand = false)
    {
        if (orientation == MoveControllerOrientation.Left)
            return (otherHand) ? m_CurrentRightObject : m_CurrentLeftObject;
        else
            return (otherHand) ? m_CurrentLeftObject : m_CurrentRightObject;
    }

    public bool GetButton(MoveControllerButton button)
    {
        if (button != MoveControllerButton.BackTrigger)
            return PS4Input.MoveGetButtons(0, m_ControllerIndex) == (GetButtonIndex(button));
        else
            return GetTriggerDown(0); // if it's back trigger
    }

    public bool GetButtonUp(MoveControllerButton button)
    {
        return (m_LastButtonStates[button] == true) && (GetButton(button) == false);
    }

    public bool GetButtonDown(MoveControllerButton button)
    {
        return (m_LastButtonStates[button] == false) && (GetButton(button) == true);
    }

    /// <summary>
    /// Returns TRUE if the analog trigger is held down
    /// </summary>
    /// <param name="downThreshold">How far to the trigger is down before returning TRUE</param>    
    public bool GetTriggerDown(int downThreshold)
    {
        // Move controllers use an API for their analog buttons, DualShock 4 uses an axis name for R2
        if (m_Orientation == MoveControllerOrientation.Left)
            return (PS4Input.MoveGetAnalogButton(0, 0) > downThreshold ? true : false);
        else
            return (PS4Input.MoveGetAnalogButton(0, 1) > downThreshold ? true : false);
    }

    public Vector3 GetVelocity()
    {
        Vector3 value;
        Tracker.GetTrackedDeviceVelocity(m_HandleNumber, out value);
        return value;
    }

    public Vector3 GetAngularVelocity()
    {
        Vector3 value;
        Tracker.GetTrackedDeviceAngularVelocity(m_HandleNumber, out value);
        return value;
    }

    public Vector3 GetMoveDelta()
    {
        return PS4Input.GetLastMoveGyro(0, m_ControllerIndex);
    }

    public int GetControllerHandle()
    {
        return m_HandleNumber;
    }

    public int GetControllerIndex()
    {
        return m_ControllerIndex;
    }

    private int GetButtonIndex(MoveControllerButton button)
    {
        // By: Isaac
        // For Dualshock controllers
        // * X                -0 
        // * Circle           -1 
        // * Square           -2
        // * Triangle         -3
        // * BackTrigger      -4
        // * MiddleButton     -5
        // * TouchPad         -6
        // * Start            -7
        switch (button)
        {
            case MoveControllerButton.X:
                return 64;
            case MoveControllerButton.Circle:
                return 32;
            case MoveControllerButton.Square:
                return 128;
            case MoveControllerButton.Triangle:
                return 16;
            case MoveControllerButton.BackTrigger:
                return 2;
            case MoveControllerButton.MiddleButton:
                return 4;
            case MoveControllerButton.Start:
                return 8;
            default:
                return 0;
        }
    }

}