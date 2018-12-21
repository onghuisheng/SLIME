using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSlot : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
        IStorable storable = other.GetComponent<IStorable>();

        if (grabbable != null && storable != null)
        {
            MoveController holder = MoveController.GetControllerThatHolds(other.gameObject);

            if (holder != null)
            {
                holder.DetachCurrentObject(false);

                storable.OnStore(this);
                grabbable.transform.position = transform.position;
                grabbable.transform.rotation = Quaternion.identity;

                // To make the object follow the belt
                //grabbable.GetComponent<Rigidbody>().useGravity = false;
                //grabbable.transform.parent = transform;
            }
        }
    }

}
