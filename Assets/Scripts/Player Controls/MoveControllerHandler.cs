using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif

public class MoveControllerHandler : MonoBehaviour
{

    [SerializeField]
    private MoveController m_MovePrimaryController, m_MoveSecondaryController;

#if UNITY_PS4
    PS4Input.LoggedInUser m_UserInfo;
#endif

    int m_UserSlot = -1;

    // Use this for initialization
    void Start()
    {
#if UNITY_PS4
        // PS4 Settings Init
        PlayStationVRSettings.robustnessLevel = PlayStationVRTrackerRobustnessLevel.Legacy;

        for (int i = 0; i < 4; i++)
        {
            var user = PS4Input.GetUsersDetails(i);
            if (user.status != 0)
            {
                m_UserInfo = user;
                m_UserSlot = i;
                break;
            }
        }

        // TODO: Check if there are two move controllers currently active, time out after a certain period and halt the game
        //UnregisterControllers();
        RegisterControllers();
#endif
    }

    // Remove the registered devices from tracking and reset the transform
    void UnregisterControllers()
    {
        int[] primaryHandles = new int[1];
        int[] secondaryHandles = new int[1];


#if UNITY_PS4
        // There are no documentation on PS4Input functions so I don't really understand PS4Input.MoveGetUsersMoveHandles(....)
        // But (I think) the m_primaryHandle and m_secondaryHandle are set to -1 because in Update(),
        // there is an if statement to check if m_primaryHandle and m_secondaryHandle are 0 and above.
        PS4Input.MoveGetUsersMoveHandles(1, primaryHandles, secondaryHandles);

        // The name of this function is straightforward so (I think) it is to remove the move controller from being tracked.
        // But I do not understand how the variable handles are being used and more, sorry.
        // The 2 steps after this function is to reset the position and rotation.
        // primaryController and secondaryController are public variables so the objects in these variables are in the Editor.
        Tracker.UnregisterTrackedDevice(primaryHandles[0]);
        Tracker.UnregisterTrackedDevice(secondaryHandles[0]);
#endif

        m_MovePrimaryController.transform.localPosition = Vector3.zero;
        m_MovePrimaryController.transform.localRotation = Quaternion.identity;
        m_MoveSecondaryController.transform.localPosition = Vector3.zero;
        m_MoveSecondaryController.transform.localRotation = Quaternion.identity;
    }

    void RegisterControllers()
    {
#if UNITY_PS4
        m_MovePrimaryController.RegisterController(MoveControllerOrientation.Left, m_UserSlot, m_UserInfo.move0Handle);
        m_MoveSecondaryController.RegisterController(MoveControllerOrientation.Right, m_UserSlot, m_UserInfo.move1Handle);
        Debug.LogFormat("Registered Move Controllers - Slot: {0} (First Handle: {1}) (Second Handle: {2})", m_UserSlot, m_UserInfo.move0Handle, m_UserInfo.move1Handle);
#endif
    }

    int[] GetControllerSlots()
    {
        int[] slots = new int[2];
        slots[0] = -1;
        slots[1] = -1;

        bool firstFound = false;

        // Check through all slots of the PS4 to see which slot the MoveController is connected to
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
#if UNITY_PS4
                if (PS4Input.MoveIsConnected(i, j))
                {
                    if (!firstFound)
                    {
                        slots[0] = i;
                        firstFound = true;
                    }
                    else
                    {
                        slots[1] = i;
                        break;
                    }
                }
#endif
            }
        }

        return slots;
    }

}
