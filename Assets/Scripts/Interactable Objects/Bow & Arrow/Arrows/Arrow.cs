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
            transform.parent = other.transform;
        }

        DestroyArrow(); // If collide with environment, remove this arrow after X seconds
    }

}