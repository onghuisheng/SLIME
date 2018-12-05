using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{

    /// <summary>
    /// Hide the controller model upon grabbing?
    /// </summary>
    bool hideControllerOnGrab { get; }

    /// <summary>
    /// Called once when the player grabs the object using Trigger button
    /// </summary>
    void OnGrab(MoveController currentController);

    /// <summary>
    /// Called once every Update while the player is grabbing the object
    /// </summary>
    void OnGrabStay(MoveController currentController);

    /// <summary>
    /// Called once when the player releases the Trigger button WHILE grabbing it
    /// </summary>
    void OnGrabReleased(MoveController currentController);

}
