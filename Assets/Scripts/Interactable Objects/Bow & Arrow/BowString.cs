using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : GrabbableObject
{

    [SerializeField]
    private ArrowBase m_BowArrowPrefab;

    [SerializeField]
    private Transform m_ArrowFacingPoint;

    private bool m_IsGrabbing = false;

    private Vector3 m_DefaultLocalPos;

    private Vector3 m_InitialOffset; 

    private Vector3 m_CurrentDrawDistance;

    private ArrowBase m_SpawnedArrow = null;

    #region Bow Aesthetic Stuff
    [SerializeField]
    private Transform m_UpperBowLimb1, m_UpperBowLimb2, m_LowerBowLimb1, m_LowerBowLimb2;

    private Quaternion m_UpperBowLimb1_DefaultRot, m_UpperBowLimb2_DefaultRot, m_LowerBowLimb1_DefaultRot, m_LowerBowLimb2_DefaultRot;

    private const float m_OuterLimbRotationRate = 180;
    private const float m_InnerLimbRotationRate = 180;
    #endregion


    private void Awake()
    {
        m_DefaultLocalPos = transform.localPosition;
        m_UpperBowLimb1_DefaultRot = m_UpperBowLimb1.localRotation;
        m_UpperBowLimb2_DefaultRot = m_UpperBowLimb2.localRotation;
        m_LowerBowLimb1_DefaultRot = m_LowerBowLimb1.localRotation;
        m_LowerBowLimb2_DefaultRot = m_LowerBowLimb2.localRotation;
    }

    public override void OnGrab(MoveController currentController)
    {
        m_SpawnedArrow = Instantiate(m_BowArrowPrefab.gameObject, transform).GetComponent<ArrowBase>();
        m_InitialOffset = transform.InverseTransformDirection(currentController.transform.position - transform.position);
        ResetArrowOrientation();
    }

    public override void OnGrabStay(MoveController currentController)
    {
        base.OnGrabStay(currentController);
        // transform.position = currentController.transform.position - m_InitialOffset;
        transform.position = currentController.transform.position - m_InitialOffset;
        m_IsGrabbing = true;
        BendBow();

        // Constantly face towards the pivot point when an arrow is drawn
        if (m_SpawnedArrow != null)
        {
            ResetArrowOrientation();
        }
    }

    public override void OnGrabReleased(MoveController currentController)
    {
        m_IsGrabbing = false;

        float arrowStrength = m_CurrentDrawDistance.magnitude;

        if (m_SpawnedArrow != null)
        {
            // Launch the arrow, multiply the strength a bit?
            m_SpawnedArrow.LaunchArrow(arrowStrength * 75);
            m_SpawnedArrow = null;
        }
    }

    //public override void OnControllerExit(MoveController currentController)
    //{
    //    base.OnControllerExit(currentController);

    //    if (m_SpawnedArrow != null && m_SpawnedArrow.transform.parent != null)
    //    {
    //        m_SpawnedArrow.DestroyArrow();
    //    }
    //}

    private void Update()
    {
        // If Player is not pulling the string, fling it back to the origin
        if (!m_IsGrabbing)
        {
            if (transform.localPosition != m_DefaultLocalPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_DefaultLocalPos, Time.deltaTime * 10);

                // When the string has fully recovered
                if (transform.localPosition == m_DefaultLocalPos)
                    m_CurrentDrawDistance = Vector3.zero;
            }

            UnbendBow();
        }
        else // Player is pulling the bow string
        {
            m_CurrentDrawDistance = transform.localPosition - m_DefaultLocalPos;
        }
    }

    // Reset the spawned arrow to make it face the pivot
    private void ResetArrowOrientation()
    {
        m_SpawnedArrow.transform.localPosition = Vector3.zero;
        m_SpawnedArrow.transform.LookAt(m_ArrowFacingPoint.transform.position);
        m_SpawnedArrow.transform.Rotate(90, 0, 0);
    }

    // Reset the bow's bend over time
    private void UnbendBow()
    {
        if (m_UpperBowLimb1.localRotation != m_UpperBowLimb1_DefaultRot)
        {
            m_UpperBowLimb1.localRotation = Quaternion.RotateTowards(m_UpperBowLimb1.localRotation, m_UpperBowLimb1_DefaultRot, m_OuterLimbRotationRate * Time.deltaTime);
            m_LowerBowLimb1.localRotation = Quaternion.RotateTowards(m_LowerBowLimb1.localRotation, m_LowerBowLimb1_DefaultRot, m_OuterLimbRotationRate * Time.deltaTime);
        }

        if (m_UpperBowLimb2.localRotation != m_UpperBowLimb2_DefaultRot)
        {
            m_UpperBowLimb2.localRotation = Quaternion.RotateTowards(m_UpperBowLimb2.localRotation, m_UpperBowLimb2_DefaultRot, m_InnerLimbRotationRate * Time.deltaTime);
            m_LowerBowLimb2.localRotation = Quaternion.RotateTowards(m_LowerBowLimb2.localRotation, m_LowerBowLimb2_DefaultRot, m_InnerLimbRotationRate * Time.deltaTime);
        }
    }

    // Bend the bow, arc is determined by the horizontal draw distance
    private void BendBow()
    {
        float bendAngle = GetHorizontalDrawDistance() * 25;
        bendAngle = Mathf.Clamp(bendAngle, 0, 20);

        m_UpperBowLimb1.localRotation = m_UpperBowLimb1_DefaultRot;
        m_UpperBowLimb1.Rotate(0, 0, bendAngle, Space.Self);
        m_UpperBowLimb2.localRotation = m_UpperBowLimb2_DefaultRot;
        m_UpperBowLimb2.Rotate(0, 0, bendAngle / 2, Space.Self);
        m_LowerBowLimb1.localRotation = m_LowerBowLimb1_DefaultRot;
        m_LowerBowLimb1.Rotate(0, 0, -bendAngle, Space.Self);
        m_LowerBowLimb2.localRotation = m_LowerBowLimb2_DefaultRot;
        m_LowerBowLimb2.Rotate(0, 0, -bendAngle / 2, Space.Self);
    }

    private float GetHorizontalDrawDistance()
    {
        return Mathf.Abs(transform.localPosition.x);
    }

}
