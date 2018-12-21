using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSlot : StationaryObject
{

    GameObject m_AttachedObject = null;
    public bool isFull { get { return m_AttachedObject != null; } }


    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);
    }

    private void OnTriggerEnter(Collider other)
    {
        MoveController currentController = other.GetComponent<MoveController>();

        if (isFull || currentController == null)
            return;

        GameObject currentObject = currentController.GetCurrentHandObject();

        if (currentObject == null)
            return;

        currentController.DetachCurrentObject(false);

        GrabbableObject grabbable = currentObject.GetComponent<GrabbableObject>();
        IStorable storable = currentObject.GetComponent<IStorable>();

        if (grabbable != null && storable != null)
        {

            Debug.Log("Detached");

            grabbable.transform.position = transform.position;
            grabbable.transform.rotation = Quaternion.identity;

            m_AttachedObject = grabbable.gameObject;
            m_AttachedObject.GetComponent<Collider>().isTrigger = true;

            // To make the object follow the belt
            grabbable.transform.parent = transform;
            grabbable.GetComponent<Rigidbody>().isKinematic = true;
            grabbable.GetComponent<Rigidbody>().useGravity = false;

            GetComponent<Collider>().enabled = false;

            storable.OnStore(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MoveController currentController = other.GetComponent<MoveController>();

        if (currentController == null)
            return;

        if (currentController.GetCurrentHandObject() == m_AttachedObject)
        {
            m_AttachedObject = null;

            GetComponent<Collider>().enabled = true;
        }
    }

}
