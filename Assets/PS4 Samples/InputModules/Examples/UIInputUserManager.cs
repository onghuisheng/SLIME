using UnityEngine;

public class UIInputUserManager : MonoBehaviour
{
    uint m_InputUserID = 0;     // The ID of the user who currently has UI control.
    int m_InputUserSlot = -1;   // The input slot number of the user who currently has UI control.

    void Start ()
    {
 #if UNITY_PS4 && !UNITY_EDITOR
        // Setup the user logout/login event handler.
        UnityEngine.PS4.PS4Input.OnUserServiceEvent += OnUserServiceEvent;

        // Give control of the UI and the simulated mouse to the user slot that represents the primary user.
        AssignPrimaryUserToUIInput();
#else
        // Use PC input mappings.
        GetComponent<InputModules.PS4InputModule>().m_AxisNameSuffix = "_PC";
#endif
    }

    // If "User Management" is enabled in the PS4 publish settings then this event handler assigns
    // control to the new primary user if the user who currently has control logs out of the PS4.
    void OnUserServiceEvent(uint eventType, uint userid)
    {
        switch (eventType)
        {
            case 0: // Logged in, nothing to do.
                break;

            case 1: // Logged out
                // Is the user that logged out the same user that has UI control? Then switch control to the primary user.
                if (userid == m_InputUserID)
                {
                    AssignPrimaryUserToUIInput();
                }
                break;
        }
    }

    int FindPrimaryUserSlot()
    {
#if UNITY_PS4
        for (int userSlot = 0; userSlot < 4; userSlot++)
        {
            UnityEngine.PS4.PS4Input.LoggedInUser user = UnityEngine.PS4.PS4Input.GetUsersDetails(userSlot);
            if (user.status != 0 && user.primaryUser)
            {
                return userSlot;
            }
        }
#endif
        return -1;
    }

    void SetInputUserSlot(int userSlot)
    {
        m_InputUserSlot = userSlot;

#if UNITY_PS4
        m_InputUserID =  (uint)UnityEngine.PS4.PS4Input.GetUsersDetails(userSlot).userId;

        Debug.Log("Assigned userSlot " + userSlot + " to UI input.");
        GetComponent<InputModules.PS4InputModule>().m_AxisNameSuffix = "_" + m_InputUserSlot.ToString();
        UnityEngine.PS4.PS4Input.MouseSetUIFocused(m_InputUserSlot, 0, true);
#endif
    }

    void AssignPrimaryUserToUIInput()
    {
        // Find the primary user in the user array...
        int userSlot = FindPrimaryUserSlot();
        if (userSlot < 0)
        {
            throw (new System.Exception("Unable to find primary user slot for UI input."));
        }
        // ...and give the primary user control of the UI and mouse simulation.
        SetInputUserSlot(userSlot);
    }

    void Update()
    {
    }
}
