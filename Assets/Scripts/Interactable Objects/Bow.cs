using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : GrabbableObject
{

    [SerializeField]
    private BowDrawString m_BowDrawString;

    private void Awake()
    {
        // Ignore collision between the bow and the draw string
        Physics.IgnoreCollision(m_BowDrawString.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_BowDrawString.GetComponent<Collider>().enabled = true; // Enable draw string to be grabbable only if the player is holding the bow
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_BowDrawString.GetComponent<Collider>().enabled = false;
    }

}
