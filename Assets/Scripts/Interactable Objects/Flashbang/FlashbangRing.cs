using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashbangRing : GrabbableObject
{

    private Flashbang m_FlashBang;
    private Vector3 m_DefaultLocalPos;

    private bool m_IsTriggered = false;

    // Use this for initialization
    void Start()
    {
        m_FlashBang = transform.parent.GetComponent<Flashbang>();
        m_DefaultLocalPos = transform.localPosition;
    }

    public override void OnGrab(MoveController currentController)
    {
        base.OnGrab(currentController);

        if (!m_IsTriggered)
        {
            transform.parent = null;
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            m_FlashBang.StartFuse();
            AudioManager.Instance.Play3D("flashbangpull", transform.position, AudioManager.AudioType.Additive);
            m_IsTriggered = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            transform.parent = null;
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            m_FlashBang.StartFuse();
            AudioManager.Instance.Play3D("flashbangpull", transform.position, AudioManager.AudioType.Additive);
            m_IsTriggered = true;
        }
    }

    public void ResetRing(Transform parent)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = parent;
        transform.localPosition = m_DefaultLocalPos;
        transform.localRotation = Quaternion.identity;
        m_IsTriggered = false;
    }

}