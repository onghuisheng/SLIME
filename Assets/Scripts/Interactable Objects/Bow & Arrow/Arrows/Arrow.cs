using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ArrowBase
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("bleh: " + other.gameObject.name);

       if (!m_IsFired)
           return;

        IShootable shootable = other.GetComponent<IShootable>();

        if (shootable != null)
        {
            transform.parent = other.transform; //set as parent before calling onshot
            shootable.OnShot(this); // Interface callback
        }

       DestroyArrow(); // If collide with environment, remove this arrow after X seconds

    }

}