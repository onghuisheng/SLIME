using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : GrabbableObject
{

    [SerializeField]
    private BowString m_BowDrawString;
        
    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
        m_BowDrawString.GetComponent<Collider>().enabled = true; // Enable draw string to be grabbable only if the player is holding the bow
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        base.OnGrabReleased(currentController);
        m_BowDrawString.GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
}