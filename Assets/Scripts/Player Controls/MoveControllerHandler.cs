using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PS4;
using UnityEngine.SpatialTracking;
using UnityEngine.PS4.VR;

public class MoveControllerHandler : MonoBehaviour
{

    [SerializeField]
    private MoveController m_MovePrimaryController, m_MoveSecondaryController;

    // Use this for initialization
    void Start()
    {
        // TODO: Check if there are two move controllers currently active, time out after a certain period and halt the game
        RegisterControllers();
    }

    void RegisterControllers()
    {
        int[] primaryHandles = new int[1];
        int[] secondaryHandles = new int[1];

        PS4Input.MoveGetUsersMoveHandles(1, primaryHandles, secondaryHandles);

        int movePrimaryHandle = primaryHandles[0];
        int moveSecondaryHandle = secondaryHandles[0];

        m_MovePrimaryController.RegisterController(MoveControllerOrientation.Left, movePrimaryHandle);
        m_MoveSecondaryController.RegisterController(MoveControllerOrientation.Right, moveSecondaryHandle);
    }



}
