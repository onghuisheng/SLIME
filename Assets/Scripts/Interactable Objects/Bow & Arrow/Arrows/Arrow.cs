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
            transform.parent = other.transform; //set as parent before calling onshot, if anything goes wrong, shift this line below OnShot HAHAH
            shootable.OnShot(this); // Interface callback
        }

        DestroyArrow(); // If collide with environment, remove this arrow after X seconds
    }

}