using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNonCentered : GrabbableObject
{

    public override void OnGrab(MoveController currentController)
    {
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

}