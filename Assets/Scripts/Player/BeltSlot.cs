using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSlot : MonoBehaviour
{

    GameObject m_AttachedObject = null;
    public bool isFull { get { return m_AttachedObject != null; } }

    private void OnTriggerEnter(Collider other)
    {
        if (isFull)
            return;

        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
        IStorable storable = other.GetComponent<IStorable>();

        if (grabbable != null && storable != null)
        {
            MoveController holder = MoveController.GetControllerThatHolds(other.gameObject);

            if (holder != null)
            {
                holder.DetachCurrentObject(false);
            }

            Debug.Log("Attached");

            if (grabbable.GetComponent<FixedJoint>())
            {
                Destroy(grabbable.GetComponent<FixedJoint>());
            }

            grabbable.transform.position = transform.position;
            grabbable.transform.rotation = Quaternion.identity;

            m_AttachedObject = grabbable.gameObject;

            m_AttachedObject.GetComponent<Collider>().isTrigger = true;

            // To make the object follow the belt
            FixedJoint joint = m_AttachedObject.AddComponent<FixedJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
            joint.breakForce = Mathf.Infinity;
            joint.breakTorque = Mathf.Infinity;
            joint.enablePreprocessing = false;
            
            storable.OnStore(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (m_AttachedObject != null && m_AttachedObject == other.gameObject)
        //{
        //    m_AttachedObject.GetComponent<Collider>().isTrigger = false;
        //    m_AttachedObject.GetComponent<Rigidbody>().isKinematic = false;
        //    m_AttachedObject = null;
        //    Debug.Log("Detached");
        //}
    }

}
