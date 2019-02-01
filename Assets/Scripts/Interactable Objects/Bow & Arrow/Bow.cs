using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : GrabbableObject
{

    [SerializeField]
    private BowString m_BowDrawString;

    public System.Action<GameObject> OnBowGrabbed;

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);

        transform.position = currentController.transform.position;
        transform.rotation = currentController.transform.localRotation;

        m_BowDrawString.GetComponent<Collider>().enabled = true; // Enable draw string to be grabbable only if the player is holding the bow
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = false;

        if (OnBowGrabbed != null)
            OnBowGrabbed.Invoke(gameObject);
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_BowDrawString.GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;

        MoveController controller = MoveController.GetControllerThatHolds(m_BowDrawString.gameObject);

        if (controller)
        {
            controller.DetachCurrentObject(false);
        }
    }

}