using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCentered : GrabbableObject, IStorable
{

    public override bool centerMeshOnGrab { get { return true; } }

    public void OnStore(BeltSlot slot)
    {
    }

    public void OnUnStore(BeltSlot slot)
    {
    }

}
