using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Useful for stationary items that cant be picked up, e.g levers
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class StationaryObject : MonoBehaviour, IInteractable, IStationaryGrabbable
{

    public virtual bool hideControllerOnGrab { get { return false; } }

    public virtual bool centerMeshOnGrab { get { return false; } }

    public virtual void OnControllerEnter(MoveController currentController) { }

    public virtual void OnControllerExit(MoveController currentController) { }

    public virtual void OnControllerStay(MoveController currentController) { }

    public virtual void OnGrab(MoveController currentController) { }

    public virtual void OnGrabStay(MoveController currentController) { }

    public virtual void OnGrabReleased(MoveController currentController) { }

    public virtual void OnUse(MoveController currentController) { }

    public virtual void OnUseDown(MoveController currentController) { }

    public virtual void OnUseUp(MoveController currentController) { }

}