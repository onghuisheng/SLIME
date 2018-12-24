using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSlot : MonoBehaviour
{

    GameObject m_AttachedObject = null;
    public bool isFull { get { return transform.childCount > 0; } }

    Vector3 m_AttachedObjectPreviousScale;

    private void OnTriggerEnter(Collider other)
    {
        MoveController currentController = other.GetComponent<MoveController>();
        
        if (isFull || currentController == null)
            return;

        if (!currentController.GetButton(MoveControllerHotkeys.buttonGrab))
            return;

        GameObject currentObject = currentController.GetCurrentHandObject();
        
        if (currentObject == null)
            return;

        GrabbableObject grabbable = currentObject.GetComponent<GrabbableObject>();
        IStorable storable = currentObject.GetComponent<IStorable>();
        
        if (grabbable != null && storable != null)
        {
            currentController.DetachCurrentObject(false);

            grabbable.transform.position = transform.position;
            grabbable.transform.rotation = Quaternion.identity;

            m_AttachedObject = grabbable.gameObject;
            m_AttachedObject.GetComponent<Collider>().isTrigger = true;
            m_AttachedObjectPreviousScale = grabbable.transform.localScale;

            // To make the object follow the belt
            grabbable.transform.parent = transform;
            grabbable.GetComponent<Rigidbody>().isKinematic = true;
            grabbable.GetComponent<Rigidbody>().useGravity = false;

            storable.OnStore(this);
        }
    }

    public void DetachObject()
    {
        if (!isFull)
        {
            Debug.LogError("Trying to detach a null belt object!");
            return;
        }

        Rigidbody rb = m_AttachedObject.GetComponent<Rigidbody>();
        rb.GetComponent<Collider>().isTrigger = false;
        rb.transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        // rb.transform.localScale = m_AttachedObjectPreviousScale;

        m_AttachedObject.GetComponent<IStorable>().OnUnStore(this);
        m_AttachedObject = null;
    }

}
