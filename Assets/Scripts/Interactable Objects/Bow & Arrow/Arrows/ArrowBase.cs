using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{

    enum ArrowType
    {

    }

    protected bool m_IsFired = false;

    bool isPendingDestruction = false;

    Rigidbody m_Rigidbody;

    public void LaunchArrow(float forceAmount)
    {
        if (m_IsFired)
            return;

        m_IsFired = true;

        transform.parent = null;

        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.AddForce(transform.up * forceAmount, ForceMode.Impulse);
    }

    public void DestroyArrow()
    {
        StartCoroutine(DestroyArrowRoutine());
    }

    private IEnumerator DestroyArrowRoutine()
    {
        if (isPendingDestruction)
            yield break;
        else
        {
            isPendingDestruction = true;
            m_Rigidbody.useGravity = false;
            m_Rigidbody.isKinematic = true;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate()
    {
        if (m_IsFired && m_Rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
            transform.Rotate(90, 0, 0);
        }
    }

}