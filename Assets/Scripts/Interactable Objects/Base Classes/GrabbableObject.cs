using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects that can be grabbed and tossed around e.g bow, barrels
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{

    public virtual bool hideControllerOnGrab { get { return false; } }

    public virtual bool centerMeshOnGrab { get { return false; } }

    public virtual void OnControllerEnter(MoveController currentController) { }

    public virtual void OnControllerExit(MoveController currentController)
    {
        transform.GetComponent<Collider>().enabled = true;
    }

    public virtual void OnControllerStay(MoveController currentController) { }

    public virtual void OnGrab(MoveController currentController)
    {
        transform.position = currentController.transform.position;
        transform.rotation = currentController.transform.localRotation;
        transform.GetComponent<Collider>().enabled = false;

        if (centerMeshOnGrab)
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                transform.position += (currentController.transform.position - renderer.bounds.center);
            }
            else
            {
                renderer = GetComponentInChildren<Renderer>();

                if (renderer != null)
                    transform.position += (currentController.transform.position - renderer.bounds.center);
            }
        }
    }

    public virtual void OnGrabStay(MoveController currentController) { }

    public virtual void OnGrabReleased(MoveController currentController)
    {
        transform.GetComponent<Collider>().enabled = true;
    }

    public virtual void OnUseDown(MoveController currentController) { }

    public virtual void OnUse(MoveController currentController) { }

    public virtual void OnUseUp(MoveController currentController) { }

    protected virtual void OnCollisionEnter(Collision collision) { }

    protected virtual void OnTriggerEnter(Collider other) { }

}
