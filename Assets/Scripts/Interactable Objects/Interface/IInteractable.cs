using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{

    /// <summary>
    /// Called once when the vive controller touches the collider
    /// </summary>
    void OnControllerEnter(MoveController currentController);

    /// <summary>
    /// Called when the vive controller left the collider without grabbing the object
    /// </summary>
    void OnControllerExit(MoveController currentController);

    /// <summary>
    /// Called once every update frame when the vive controller is inside the collider
    /// </summary>
    void OnControllerStay(MoveController currentController);

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called once every update frame while held down
    /// </summary>
    void OnUseDown(MoveController currentController);

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called only once
    /// </summary>
    void OnUse(MoveController currentController);

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called once after the player released the Use button
    /// </summary>
    void OnUseUp(MoveController currentController);

}
