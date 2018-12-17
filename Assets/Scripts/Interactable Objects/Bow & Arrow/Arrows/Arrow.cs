using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ArrowBase
{


    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsFired)
            return;

        IShootable shootable = other.GetComponent<IShootable>();

        if (shootable != null)
        {
            shootable.OnShot(this); // Interface callback

            //Rigidbody rb = other.GetComponent<Rigidbody>();

            //if (rb != null)
            //{
            //    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            //    joint.connectedBody = rb;
            //    joint.breakForce = Mathf.Infinity;
            //    joint.breakTorque = Mathf.Infinity;
            //}

            transform.parent = other.transform;
        }

        DestroyArrow(); // If collide with environment, remove this arrow after X seconds
    }

}