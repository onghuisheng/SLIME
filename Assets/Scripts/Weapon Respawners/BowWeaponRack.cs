using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Trash codes, written during last minute
/// </summary>
public class BowWeaponRack : MonoBehaviour
{
    
    private void OnTriggerStay(Collider other)
    {
        Bow bow = other.GetComponent<Bow>();

        if (bow != null)
        {
            if (bow.GetComponent<FixedJoint>() == null)
            {
                bow.transform.position = transform.position;
                bow.transform.rotation = Quaternion.identity;
                bow.GetComponent<Rigidbody>().useGravity = false;
                bow.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

}
